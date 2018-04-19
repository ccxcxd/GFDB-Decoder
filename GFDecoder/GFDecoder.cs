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
