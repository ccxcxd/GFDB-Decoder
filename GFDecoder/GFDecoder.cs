using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GFDecoder
{
    class GFDecoder
    {
        /// <summary>
        /// Load catchdata json file and split it into seperate jsons
        /// </summary>
        /// <param name="input">the content of catchdata json file</param>
        /// <returns>(infoname : infojson) dictionary</returns>
        public static Dictionary<string, string> LoadCatchDataJsonFile(Stream input)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            using (StreamReader reader = new StreamReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.Length == 0)
                        continue;

                    var m = Regex.Match(line, @"{""(\w+)"":");
                    if (m.Success)
                        result.Add(m.Groups[1].Value, line);
                }
            }
            return result;
        }

        public static void SaveSplitedJsonFiles(Dictionary<string, string> jsons, string outFolder)
        {
            Directory.CreateDirectory(outFolder);
            foreach (var pair in jsons)
                File.WriteAllText(Path.Combine(outFolder, pair.Key + ".json"), pair.Value);
        }

        public static void SplitJsonFile(string infilePath, string outFolder)
        {
            Dictionary<string, string> jsons = LoadCatchDataJsonFile(new FileStream(infilePath, FileMode.Open));
            SaveSplitedJsonFiles(jsons, outFolder);
        }

        public static Dictionary<int, T> LoadSingleJsonData<T>(string jsonData, string keyName)
        {
            string classname = typeof(T).Name;
            var a = JsonConvert.DeserializeObject<Dictionary<string, List<T>>>(jsonData);

            var result = new Dictionary<int, T>();
            var keyField = typeof(T).GetField(keyName);
            foreach (T item in a[classname])
            {
                result.Add((int)keyField.GetValue(item), item);
            }
            return result;
        }

        public static Dictionary<int, T> LoadSingleJsonData<T>(Dictionary<string, string> jsons, string keyName)
        {
            string classname = typeof(T).Name;
            string jsonData = jsons[classname];
            return LoadSingleJsonData<T>(jsonData, keyName);
        }

        public static Dictionary<int, T> LoadSingleJsonDataFromFolder<T>(string jsonFolder, string keyName)
        {
            string classname = typeof(T).Name;
            string text = File.ReadAllText(Path.Combine(jsonFolder, classname + ".json"));
            return LoadSingleJsonData<T>(text, keyName);
        }

        public static string SaveSingleJsonData<T>(Dictionary<int, T> data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static void SaveSingleJsonDataToFolder<T>(string jsonFolder, Dictionary<int, T> data)
        {
            string datastring = SaveSingleJsonData(data);
            string classname = typeof(T).Name;
            File.WriteAllText(Path.Combine(jsonFolder, classname + ".json"), datastring);
        }

        public static void ProcessJsonData(Dictionary<string, string> jsons, string outputpath)
        {
            var enemyAttrInfo = LoadSingleJsonData< enemy_standard_attribute_info>(jsons, "level");
            var missionInfo = LoadSingleJsonData<mission_info>(jsons, "id");
            var spotInfo = LoadSingleJsonData<spot_info>(jsons, "id");
            var enemyTeamInfo = LoadSingleJsonData<enemy_team_info>(jsons, "id");
            var enemyInTeamInfo = LoadSingleJsonData<enemy_in_team_info>(jsons, "id");
            var enemyCharInfo = LoadSingleJsonData<enemy_character_type_info>(jsons, "id");
            var allyTeamInfo = LoadSingleJsonData<ally_team_info>(jsons, "id");

            var eventCampaignInfo = LoadSingleJsonDataFromFolder<event_campaign_info>("supplemental", "id");
            var campaignInfo = new Dictionary<int, campaign_info>();

            var eventCampaignLookup = new Dictionary<int, Tuple<int, string, int>>(); // campaign_id: (id, name, chapter)
            foreach (var eventCam in eventCampaignInfo.Values)
                for (int i = 0; i < eventCam.campaign.Length; i++)
                    eventCampaignLookup[eventCam.campaign[i]] = new Tuple<int, string, int>(eventCam.id, eventCam.name, i + 1);

            foreach (var mission in missionInfo.Values)
            {
                if (mission.duplicate_type == 0)
                {
                    if (mission.campaign >= 0)
                    {
                        // main
                        string suffix;
                        switch (mission.if_emergency)
                        {
                            case 0:
                                suffix = "";
                                break;
                            case 1:
                                suffix = "E";
                                break;
                            case 3:
                                suffix = "N";
                                break;
                            default:
                                suffix = "";
                                break;
                        }
                        mission.index_text = String.Format("{0}-{1}{2}", mission.campaign, mission.sub, suffix);

                        int campaign_id = mission.campaign;
                        string campaign_name = "campaign.main" + mission.campaign;
                        if (!campaignInfo.ContainsKey(campaign_id))
                            campaignInfo[campaign_id] = new campaign_info(campaign_id, 0, campaign_name);
                        campaignInfo[campaign_id].mission_ids.Add(mission.id);
                    }
                    else
                    {
                        // event
                        var t = eventCampaignLookup[mission.campaign];
                        int campaign_id = 100 + t.Item1;
                        string campaign_name = t.Item2;
                        int chapter = t.Item3;
                        mission.index_text = String.Format("{0}-{1}", chapter, mission.sub);

                        if (!campaignInfo.ContainsKey(campaign_id))
                            campaignInfo[campaign_id] = new campaign_info(campaign_id, 1, campaign_name);
                        campaignInfo[campaign_id].mission_ids.Add(mission.id);

                    }
                }
                else if (mission.duplicate_type > 0)
                {
                    // simulation
                    mission.index_text = "";

                    int campaign_id = 200 + mission.campaign;
                    string campaign_name = "campaign.simulation" + mission.campaign;
                    if (!campaignInfo.ContainsKey(campaign_id))
                        campaignInfo[campaign_id] = new campaign_info(campaign_id, 2, campaign_name);
                    campaignInfo[campaign_id].mission_ids.Add(mission.id);
                }
            }

            foreach (var member in enemyInTeamInfo.Values)
            {
                if (!enemyCharInfo.ContainsKey(member.enemy_character_type_id))
                    continue;

                member.enemy_character = enemyCharInfo[member.enemy_character_type_id].get_info_at_level(member.level, member.number, enemyAttrInfo);
                member.difficulty = CalculateDifficulty(member.enemy_character);

                if (!enemyTeamInfo.ContainsKey(member.enemy_team_id))
                    continue;

                enemyTeamInfo[member.enemy_team_id].member_ids.Add(member.id);
                enemyTeamInfo[member.enemy_team_id].difficulty += member.difficulty;
            }

            Directory.CreateDirectory(outputpath);
            SaveSingleJsonDataToFolder(outputpath, missionInfo);
            SaveSingleJsonDataToFolder(outputpath, spotInfo);
            SaveSingleJsonDataToFolder(outputpath, enemyTeamInfo);
            SaveSingleJsonDataToFolder(outputpath, enemyInTeamInfo);
            SaveSingleJsonDataToFolder(outputpath, enemyCharInfo);
            SaveSingleJsonDataToFolder(outputpath, allyTeamInfo);
            SaveSingleJsonDataToFolder(outputpath, campaignInfo);
        }

        public static int CalculateDifficulty(enemy_character_type_info enemy)
        {
            return CalculateDifficulty(enemy.maxlife, enemy.dodge, enemy.pow, enemy.hit, enemy.rate, 0);
        }

        public static int CalculateDifficulty(int maxlife, int dodge, int pow, int hit, int rate, int crit)
        {
            return 1;
        }

        public static void Json2Csv(string inputpath, string outputpath)
        {
            string jsonData = File.ReadAllText(inputpath);

            var tmp = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(jsonData);
            string name = tmp.Keys.First();
            var a = tmp[name];

            StringBuilder sb = new StringBuilder();
            // title line
            var line = string.Join(",", a.First().Keys.ToArray());
            sb.AppendLine(line);
            foreach (var entry in a)
            {
                line = string.Join(",", entry.Select(x => {
                    // quote lines with comma
                    if (x.Value.Contains(","))
                        return "\"" + x.Value + "\"";
                    else
                        return x.Value;
                }));
                sb.AppendLine(line);
            }

            File.WriteAllText(outputpath, sb.ToString());

        }

    }
}
