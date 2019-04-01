using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
                    {
                        string name = m.Groups[1].Value;
                        if (!result.ContainsKey(name))
                            result.Add(name, line);
                    }
                }
            }
            return result;
        }

        public static void SaveSplitedJsonFiles(Dictionary<string, string> jsons, string outFolder)
        {
            Directory.CreateDirectory(outFolder);
            foreach (var pair in jsons)
            {
                var tmp = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(pair.Value);
                var formatted = JsonConvert.SerializeObject(tmp, Formatting.Indented);
                File.WriteAllText(Path.Combine(outFolder, pair.Key + ".json"), formatted);
            }
        }

        public static void SplitJsonFile(string infilePath, string outFolder)
        {
            Dictionary<string, string> jsons = LoadCatchDataJsonFile(new FileStream(infilePath, FileMode.Open));
            SaveSplitedJsonFiles(jsons, outFolder);
        }

        public static Dictionary<S, T> LoadSingleJsonData<T, S>(string jsonData, string keyName)
        {
            string classname = typeof(T).Name;
            var a = JsonConvert.DeserializeObject<Dictionary<string, List<T>>>(jsonData);

            var result = new Dictionary<S, T>();
            var keyField = typeof(T).GetField(keyName);
            foreach (T item in a[classname])
            {
                result.Add((S)keyField.GetValue(item), item);
            }
            return result;
        }

        public static Dictionary<S, T> LoadSingleJsonData<T, S>(Dictionary<string, string> jsons, string keyName)
        {
            string classname = typeof(T).Name;
            string jsonData = jsons[classname];
            return LoadSingleJsonData<T, S>(jsonData, keyName);
        }

        public static Dictionary<S, T> LoadSingleJsonDataFromFolder<T, S>(string jsonFolder, string keyName)
        {
            string classname = typeof(T).Name;
            string text = File.ReadAllText(Path.Combine(jsonFolder, classname + ".json"));
            return LoadSingleJsonData<T, S>(text, keyName);
        }

        public static string SaveSingleJsonData<T, S>(Dictionary<S, T> data, bool indent)
        {
            var formatting = indent ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(data, formatting);
        }

        public static void SaveSingleJsonDataToFolder<T, S>(string jsonFolder, Dictionary<S, T> data)
        {
            string classname = typeof(T).Name;
            string datastring = SaveSingleJsonData(data, true);
            File.WriteAllText(Path.Combine(jsonFolder, classname + ".json"), datastring);

            //Directory.CreateDirectory(Path.Combine(jsonFolder, "min"));
            //datastring = SaveSingleJsonData(data, false);
            //File.WriteAllText(Path.Combine(jsonFolder, "min", classname + ".json"), datastring);
        }

        public static void ProcessJsonData(Dictionary<string, string> jsons, string outputpath)
        {
            var enemyAttrInfo = LoadSingleJsonData<enemy_standard_attribute_info, int>(jsons, "level");
            var missionInfo = LoadSingleJsonData<mission_info, int>(jsons, "id");
            var spotInfo = LoadSingleJsonData<spot_info, int>(jsons, "id");
            var enemyTeamInfo = LoadSingleJsonData<enemy_team_info, int>(jsons, "id");
            var enemyInTeamInfo = LoadSingleJsonData<enemy_in_team_info, int>(jsons, "id");
            var enemyCharInfo = LoadSingleJsonData<enemy_character_type_info, int>(jsons, "id");
            var allyTeamInfo = LoadSingleJsonData<ally_team_info, int>(jsons, "id");
            var gunInAllyInfo = LoadSingleJsonData<gun_in_ally_info, int>(jsons, "id");
            var gameConfigInfo = LoadSingleJsonData<game_config_info, string>(jsons, "parameter_name");
            var gunInfo = LoadSingleJsonData<gun_info, int>(jsons, "id");
            var equipInfo = LoadSingleJsonData<equip_info, int>(jsons, "id");
            var buildingInfo = LoadSingleJsonData<building_info, int>(jsons, "id");
            var operationInfo = LoadSingleJsonData<operation_info, int>(jsons, "id");
            var theaterInfo = LoadSingleJsonData<theater_info, int>(jsons, "id");
            var theaterAreaInfo = LoadSingleJsonData<theater_area_info, int>(jsons, "id");
            var trialInfo = LoadSingleJsonData<trial_info, int>(jsons, "id");

            var eventCampaignInfo = LoadSingleJsonDataFromFolder<event_campaign_info, int>("supplemental", "id");
            var missionExtraTeamInfo = LoadSingleJsonDataFromFolder<mission_extra_enemy_team_info, int>("supplemental", "enemy_team_id_from");
            var campaignInfo = new Dictionary<int, campaign_info>();

            const int THEATER_MISSION_ID = 900000;
            const int DEFENCE_TRIAL_CAMPAIGN_ID = 200;
            const int DEFENCE_TRIAL_MISSION_ID = 200000;

            StringBuilder debugLog = new StringBuilder();

            var eventCampaignLookup = new Dictionary<int, Tuple<int, string, int>>(); // campaign_id: (id, name, chapter)
            foreach (var eventCam in eventCampaignInfo.Values)
                for (int i = 0; i < eventCam.campaign.Length; i++)
                    eventCampaignLookup[eventCam.campaign[i]] = new Tuple<int, string, int>(eventCam.id, eventCam.name, i + 1);

            // this involve in changing names, must come first
            foreach (var enemyChar in enemyCharInfo.Values)
            {
                enemyChar.name = "enemy_char_name." + enemyChar.id;
            }

            foreach (var mission in missionInfo.Values)
            {
                if (mission.duplicate_type == 0)
                {
                    if (mission.campaign >= 0)
                    {
                        // main
                        string suffix;
                        int sortmulti;
                        switch (mission.if_emergency)
                        {
                            case 0:
                                suffix = "";
                                sortmulti = 0;
                                break;
                            case 1:
                                suffix = "E";
                                sortmulti = 1;
                                break;
                            case 3:
                                suffix = "N";
                                sortmulti = 2;
                                break;
                            default:
                                suffix = "";
                                sortmulti = 0;
                                break;
                        }
                        mission.index_text = String.Format("{0}-{1}{2}", mission.campaign, mission.sub, suffix);
                        mission.index_sort = sortmulti* 1000000 + mission.campaign * 1000 + mission.sub;

                        int campaign_id = mission.campaign;
                        string campaign_name = "campaign.main" + mission.campaign;
                        if (!campaignInfo.ContainsKey(campaign_id))
                            campaignInfo[campaign_id] = new campaign_info(campaign_id, 0, campaign_name);
                        campaignInfo[campaign_id].mission_ids.Add(mission.id);
                    }
                    else
                    {
                        // event
                        if (eventCampaignLookup.ContainsKey(mission.campaign))
                        {
                            // only process those listed
                            var t = eventCampaignLookup[mission.campaign];
                            int campaign_id = 100 + t.Item1;
                            string campaign_name = t.Item2;
                            int chapter = t.Item3;
                            mission.index_text = String.Format("{0}-{1}", chapter, mission.sub);
                            mission.index_sort = chapter * 1000 + mission.sub;

                            if (!campaignInfo.ContainsKey(campaign_id))
                                campaignInfo[campaign_id] = new campaign_info(campaign_id, 1, campaign_name);
                            campaignInfo[campaign_id].mission_ids.Add(mission.id);
                        }

                    }
                }
                else if (mission.duplicate_type > 0)
                {
                    // simulation
                    mission.index_text = mission.sub.ToString();
                    mission.index_sort = mission.sub;

                    int campaign_id = 200 + mission.campaign;
                    string campaign_name = "campaign.simulation" + mission.campaign;
                    if (!campaignInfo.ContainsKey(campaign_id))
                        campaignInfo[campaign_id] = new campaign_info(campaign_id, 2, campaign_name);
                    campaignInfo[campaign_id].mission_ids.Add(mission.id);
                }

                string[] mapinfo = mission.map_information.Split('|');
                int[] mapOverallSize = BreakStringArray(mapinfo[0], s => int.Parse(s));
                int[] mapChopSize = BreakStringArray(mapinfo[1], s => int.Parse(s));
                int[] mapChopOffset = BreakStringArray(mapinfo[2], s => int.Parse(s));
                mission.map_all_width = mapOverallSize[0];
                mission.map_all_height = mapOverallSize[1];
                mission.map_eff_width = mapChopSize[0];
                mission.map_eff_height = mapChopSize[1];
                mission.map_offset_x = mapChopOffset[0];
                mission.map_offset_y = mapChopOffset[1];

                foreach (var win_obj in BreakStringArray(mission.type, s => int.Parse(s), ';'))
                {
                    mission.win_objs.Add(win_obj);
                }
                mission.has_medal_obj = mission.special_type == 0 && mission.if_emergency != 2;
    }

            foreach (var spot in spotInfo.Values)
            {
                if (spot.mission_id <= 0 || !missionInfo.ContainsKey(spot.mission_id))
                    continue;

                var mission = missionInfo[spot.mission_id];
                mission.spot_ids.Add(spot.id);

                int enemy_team_id;
                if (spot.enemy_team_id != 0)
                    enemy_team_id = spot.enemy_team_id;
                else if (spot.ally_team_id != 0)
                    enemy_team_id = allyTeamInfo[spot.ally_team_id].enemy_team_id;
                else
                    enemy_team_id = 0;

                if (enemy_team_id != 0)
                {
                    if (!mission.enemy_team_count.ContainsKey(enemy_team_id))
                        mission.enemy_team_count[enemy_team_id] = 0;
                    mission.enemy_team_count[enemy_team_id]++;
                    enemyTeamInfo[enemy_team_id].spot_id = spot.id;
                    spot.enemy_team_id = enemy_team_id;
                }

                spot.coordinator_x = Math.Abs(mission.map_eff_width) / 2 + (spot.coordinator_x - mission.map_offset_x);
                spot.coordinator_y = Math.Abs(mission.map_eff_height) / 2  - (spot.coordinator_y - mission.map_offset_y);

                foreach (var route_id in BreakStringArray(spot.route, s => int.Parse(s)))
                {
                    if (spot.route_types.ContainsKey(route_id))
                        continue;

                    var other = spotInfo[route_id];
                    if (!other.route_types.ContainsKey(spot.id))
                    {
                        var other_route_ids = BreakStringArray(other.route, s => int.Parse(s)).ToList();
                        if (other_route_ids.Contains(spot.id))
                            spot.route_types.Add(route_id, 2); // 2 way
                        else
                            spot.route_types.Add(route_id, 1); // 1 way
                    }
                }
            }

            foreach (var info in missionExtraTeamInfo.Values)
            {
                var mission = missionInfo[info.mission_id];
                for (int enemy_team_id = info.enemy_team_id_from; enemy_team_id <= info.enemy_team_id_to; enemy_team_id++)
                {
                    if (!mission.enemy_team_count.ContainsKey(enemy_team_id))
                        mission.enemy_team_count[enemy_team_id] = 0;
                    enemyTeamInfo[enemy_team_id].spot_id = -1;
                }
            }

            foreach (var team in enemyTeamInfo.Values)
            {
                var lv_ups = BreakStringArray(team.correction_turn, s => s.Split(':'));
                int curTurn = 1;
                int lastUp = 0;
                foreach (var lv_up in lv_ups)
                {
                    var turn = int.Parse(lv_up[0]);
                    var up = int.Parse(lv_up[1]);
                    for (; curTurn < turn; curTurn++)
                        team.lv_up_array.Add(lastUp);
                    team.lv_up_array.Add(up);
                    lastUp = up;
                    curTurn++;
                }

                foreach (var id in BreakStringArray(team.limit_guns, s => int.Parse(s)).ToList())
                {
                    if (gunInfo.ContainsKey(id))
                        team.drops_limit.Add(gunInfo[id].name);
                    else
                        team.drops_limit.Add(id.ToString());
                }
                foreach (var id in BreakStringArray(team.limit_equips, s => int.Parse(s)).ToList())
                {
                    if (equipInfo.ContainsKey(id))
                        team.drops_limit.Add(equipInfo[id].name);
                    else
                        team.drops_limit.Add(id.ToString());
                }
                foreach (var id in BreakStringArray(team.reward_gun_pool, s => int.Parse(s)).ToList())
                {
                    if (gunInfo.ContainsKey(id))
                        team.drops_reg_count[gunInfo[id].rank - 1]++;
                }
                foreach (var id in BreakStringArray(team.equip_s_probability, s => int.Parse(s)).ToList())
                {
                    if (equipInfo.ContainsKey(id))
                        team.drops_reg_count[equipInfo[id].rank - 1]++;
                }
            }

            foreach (var member in enemyInTeamInfo.Values)
            {
                if (!enemyCharInfo.ContainsKey(member.enemy_character_type_id))
                    continue;

                if (enemyTeamInfo.ContainsKey(member.enemy_team_id))
                    enemyTeamInfo[member.enemy_team_id].member_ids.Add(member.id);
            }

            foreach (var team in allyTeamInfo.Values)
            {
                team.leader_id = 0;
                var ally_gun_ids = BreakStringArray(team.guns, s => int.Parse(s)).ToList();
                foreach (var ally_gun_id in ally_gun_ids)
                {
                    var gia = gunInAllyInfo[ally_gun_id];
                    if (gia.location == 1)
                    {
                        team.leader_id = gia.gun_id;
                        break;
                    }
                }
                // warning: should check source code
                if (team.leader_id == 0 && ally_gun_ids.Count != 0)
                    team.leader_id = gunInAllyInfo[ally_gun_ids[0]].gun_id;
            }

            foreach (var team in enemyTeamInfo.Values)
            {
                string mission_id = "";
                if (team.spot_id > 0)
                    mission_id = spotInfo[team.spot_id].mission_id.ToString();
                debugLog.AppendLine(string.Format("team_in_mission,{0},{1}", team.id, mission_id));
            }

            foreach (var oper in operationInfo.Values)
            {
                oper.total = oper.mp + oper.ammo + oper.mre + oper.part;
                oper.duration_h = oper.duration / 3600d;
                oper.mp_h = oper.mp / oper.duration_h;
                oper.ammo_h = oper.ammo / oper.duration_h;
                oper.mre_h = oper.mre / oper.duration_h;
                oper.part_h = oper.part / oper.duration_h;

                var items = BreakStringArray(oper.item_pool, s => int.Parse(s)).ToList();
                foreach (var item in items)
                    if (item != 0)
                        oper.item_list.Add(item);
            }

            foreach (var theater in theaterInfo.Values)
            {
                int campaign_id = 300 + theater.id;
                if (campaignInfo.ContainsKey(campaign_id))
                    throw new Exception("Compaign exist for theater " + theater.id);
                campaignInfo[campaign_id] = new campaign_info(campaign_id, 3, theater.name);

                var area_ids = BreakStringArray(theater.area, s => int.Parse(s)).ToList();
                for (var i = 0; i < area_ids.Count; i++)
                {
                    var area = theaterAreaInfo[area_ids[i]];

                    var mission = new mission_info
                    {
                        id = THEATER_MISSION_ID + area.id,
                        name = area.name,
                        index_text = (i + 1).ToString(),
                        index_sort = i,
                        no_map = true,
                    };

                    var enemy_lvs = BreakStringArray(area.enemy_lv, s => int.Parse(s)).ToList();
                    var occupied_enemy_lv = BreakStringArray(area.occupied_enemy_lv, s => int.Parse(s)).ToList();
                    mission.turn_limit = enemy_lvs.Count + occupied_enemy_lv.Count;
                    var lv_up_array = enemy_lvs.Concat(occupied_enemy_lv).ToList();

                    var enemy_infos = BreakStringArray(area.enemy_group, s => s).ToList();
                    if (area.boss != "")
                        enemy_infos.Add(area.boss);
                    for (var j = 0; j < enemy_infos.Count; j++)
                    {
                        var info_tokens = enemy_infos[j].Split('-');
                        var original_team_id = int.Parse(info_tokens[0]);
                        var is_night = int.Parse(info_tokens[1]) == 1;
                        var original_team = enemyTeamInfo[original_team_id];

                        var team = new enemy_team_info
                        {
                            id = mission.id * 100 + j + 1,
                            enemy_leader = original_team.enemy_leader,
                            no_map = true,
                            is_night = is_night,
                        };
                        team.member_ids.AddRange(original_team.member_ids);

                        if (area.boss != "" && j == enemy_infos.Count - 1)
                        {
                            // special case: boss enemy team have wrong leader, need to correct it
                            team.enemy_leader = original_team.member_ids[0];
                        }
                        else
                        {
                            // non-boss, level will change
                            team.lv_up_array = lv_up_array;
                        }

                        enemyTeamInfo[team.id] = team;
                        mission.enemy_team_count[team.id] = 0;
                    }

                    missionInfo[mission.id] = mission;
                    campaignInfo[campaign_id].mission_ids.Add(mission.id);
                }
            }

            { // defence trial
                int campaign_id = DEFENCE_TRIAL_CAMPAIGN_ID;
                if (campaignInfo.ContainsKey(campaign_id))
                    throw new Exception("Compaign exist for defence");
                campaignInfo[campaign_id] = new campaign_info(campaign_id, 2, "campaign.defense_drill");

                int GROUPING = 10;
                int mission_id = DEFENCE_TRIAL_MISSION_ID;
                int enemy_in_team_id = DEFENCE_TRIAL_MISSION_ID * 100 + 1;
                var mission = new mission_info
                {
                    id = mission_id,
                    name = "campaign.defense_drill_lv",
                    no_map = true,
                };
                int trial_id;
                for (trial_id = 1; trial_id <= trialInfo.Count; trial_id++)
                {
                    var trial = trialInfo[trial_id];

                    var original_team = enemyTeamInfo[trial.enemy_team_id];
                    var team = new enemy_team_info
                    {
                        id = DEFENCE_TRIAL_MISSION_ID * 100 + trial_id,
                        enemy_leader = original_team.enemy_leader,
                        no_map = true,
                        is_night = trial.is_night == 1,
                    };
                    if (trial.enemy_level != 0)
                    {
                        foreach (var member_id in original_team.member_ids)
                        {
                            var newMember = enemy_in_team_info.CopyFrom(enemyInTeamInfo[member_id], enemy_in_team_id);
                            enemy_in_team_id++;
                            newMember.level = trial.enemy_level;
                            newMember.enemy_team_id = team.id;

                            if (enemyInTeamInfo.ContainsKey(newMember.id))
                                throw new Exception("enemyInTeamInfo exist for " + (newMember.id));
                            enemyInTeamInfo[newMember.id] = newMember;
                            team.member_ids.Add(newMember.id);
                        }
                    }
                    else
                    {
                        team.member_ids.AddRange(original_team.member_ids);
                    }

                    if (enemyTeamInfo.ContainsKey(team.id))
                        throw new Exception("enemyTeamInfo exist for " + (team.id));
                    enemyTeamInfo[team.id] = team;
                    mission.enemy_team_count[team.id] = 0;

                    if (trial_id % GROUPING == 0)
                    {
                        mission.index_text = String.Format("{0}~{1}", trial_id - GROUPING + 1, trial_id);
                        mission.index_sort = trial_id / GROUPING;

                        missionInfo[mission.id] = mission;
                        campaignInfo[campaign_id].mission_ids.Add(mission.id);

                        mission_id++;
                        mission = new mission_info
                        {
                            id = mission_id,
                            name = "campaign.defense_drill_lv",
                            no_map = true,
                        };
                    }
                }

                trial_id--;
                if (trial_id % GROUPING != 0)
                {
                    int start = trial_id / GROUPING * GROUPING + 1;
                    mission.index_text = String.Format("{0}~{1}", start, trial_id);
                    mission.index_sort = trial_id / GROUPING + 1;

                    missionInfo[mission.id] = mission;
                    campaignInfo[campaign_id].mission_ids.Add(mission.id);
                }

            }

            //GunRateTest(debugLog);

            Directory.CreateDirectory(outputpath);
            SaveSingleJsonDataToFolder(outputpath, gameConfigInfo);
            SaveSingleJsonDataToFolder(outputpath, enemyAttrInfo);
            SaveSingleJsonDataToFolder(outputpath, missionInfo);
            SaveSingleJsonDataToFolder(outputpath, spotInfo);
            SaveSingleJsonDataToFolder(outputpath, enemyTeamInfo);
            SaveSingleJsonDataToFolder(outputpath, enemyInTeamInfo);
            SaveSingleJsonDataToFolder(outputpath, enemyCharInfo);
            SaveSingleJsonDataToFolder(outputpath, campaignInfo);
            SaveSingleJsonDataToFolder(outputpath, gunInfo);
            SaveSingleJsonDataToFolder(outputpath, allyTeamInfo);
            SaveSingleJsonDataToFolder(outputpath, buildingInfo);
            SaveSingleJsonDataToFolder(outputpath, operationInfo);

            File.WriteAllText(Path.Combine(outputpath, "debug_log.txt"), debugLog.ToString());
        }

        public static T[] BreakStringArray<T>(string str, Converter<string, T> converter, char seperator = ',')
        {
            if (str == "")
                return new T[0];

            string[] tokens = str.Split(seperator);
            return Array.ConvertAll(tokens, converter); ;
        }

        public static void Json2Csv(string inputpath, string outputpath)
        {
            string jsonData = File.ReadAllText(inputpath);
            var tmp = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(jsonData);
            string name = tmp.Keys.First();
            var json = tmp[name];
            Json2Csv(json, outputpath);
        }
        
        public static void Json2Csv(List<Dictionary<string, string>> json, string outputpath)
        {
            if (json.Count == 0)
                return;

            StringBuilder sb = new StringBuilder();
            // title line
            var line = string.Join(",", json.First().Keys.ToArray());
            sb.AppendLine(line);
            foreach (var entry in json)
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

            File.WriteAllText(outputpath, sb.ToString(), Encoding.UTF8);

        }
        
        public static void GunRateTest(StringBuilder debugLog)
        {
            float fixedDeltaTime = (float)1.0 / 30;
            for (int i = 15; i <= 150; i++)
            {
                int rate = (int)(50f / (float)i / fixedDeltaTime);
                debugLog.AppendLine(string.Format("gun_rate,{0},{1}", i, rate));
            }
        }

        public static void DoSplitAndProcess(string jsonpath, string splitpath, string processpath)
        {
            var jsons = LoadCatchDataJsonFile(new FileStream(jsonpath, FileMode.Open));

            if (splitpath != null)
            {
                SaveSplitedJsonFiles(jsons, splitpath);
                foreach (var f in Directory.GetFiles(splitpath, "*.json"))
                {
                    string outputpath = Path.Combine(Path.GetDirectoryName(f), Path.GetFileNameWithoutExtension(f) + ".csv");
                    Json2Csv(f, outputpath);
                }
            }

            if (processpath != null)
            {
                ProcessJsonData(jsons, processpath);
            }
        }
    }
}
