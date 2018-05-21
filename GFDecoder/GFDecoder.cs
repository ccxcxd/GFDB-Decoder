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
                        result.Add(m.Groups[1].Value, line);
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

        public static string SaveSingleJsonData<T>(Dictionary<int, T> data, bool indent)
        {
            var formatting = indent ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(data, formatting);
        }

        public static void SaveSingleJsonDataToFolder<T>(string jsonFolder, Dictionary<int, T> data)
        {
            string classname = typeof(T).Name;
            string datastring = SaveSingleJsonData(data, true);
            File.WriteAllText(Path.Combine(jsonFolder, classname + ".json"), datastring);

            Directory.CreateDirectory(Path.Combine(jsonFolder, "min"));
            datastring = SaveSingleJsonData(data, false);
            File.WriteAllText(Path.Combine(jsonFolder, "min", classname + ".json"), datastring);
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
            var gameConfigInfo = LoadSingleJsonData<game_config_info, string>(jsons, "parameter_name");
            var gunInfo = LoadSingleJsonData<gun_info, int>(jsons, "id");
            var equipInfo = LoadSingleJsonData<equip_info, int>(jsons, "id");

            var eventCampaignInfo = LoadSingleJsonDataFromFolder<event_campaign_info, int>("supplemental", "id");
            var missionExtraTeamInfo = LoadSingleJsonDataFromFolder<mission_extra_enemy_team_info, int>("supplemental", "enemy_team_id_from");
            var campaignInfo = new Dictionary<int, campaign_info>();

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
                        if (eventCampaignLookup.ContainsKey(mission.campaign))
                        {
                            // only process those listed
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
            }

            foreach (var spot in spotInfo.Values)
            {
                if (spot.mission_id <= 0)
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

            foreach (var member in enemyInTeamInfo.Values)
            {
                if (!enemyCharInfo.ContainsKey(member.enemy_character_type_id))
                    continue;

                member.enemy_character = enemyCharInfo[member.enemy_character_type_id].get_info_at_level(member.level, member.number, enemyAttrInfo);
                member.difficulty = CalculateEnemyDifficulty(member.enemy_character, gameConfigInfo);

                if (enemyTeamInfo.ContainsKey(member.enemy_team_id))
                {
                    enemyTeamInfo[member.enemy_team_id].member_ids.Add(member.id);
                    enemyTeamInfo[member.enemy_team_id].difficulty += member.difficulty;
                }
            }

            foreach (var team in enemyTeamInfo.Values)
            {
                if (team.limit_guns != "")
                {
                    foreach (var id in BreakStringArray(team.limit_guns, s => int.Parse(s)))
                    {
                        if (gunInfo.ContainsKey(id))
                            team.drops.Add(gunInfo[id].name);
                        else
                            team.drops.Add("gun_id=" + id);
                    }
                }
                if (team.limit_equips != "")
                {
                    foreach (var id in BreakStringArray(team.limit_equips, s => int.Parse(s)))
                    {
                        if (equipInfo.ContainsKey(id))
                            team.drops.Add(equipInfo[id].name);
                        else
                            team.drops.Add("equip_id=" + id);
                    }
                }

                string mission_id = "";
                if (team.spot_id > 0)
                    mission_id = spotInfo[team.spot_id].mission_id.ToString();
                debugLog.AppendLine(string.Format("team_in_mission,{0},{1}", team.id, mission_id));
            }

            Directory.CreateDirectory(outputpath);
            SaveSingleJsonDataToFolder(outputpath, missionInfo);
            SaveSingleJsonDataToFolder(outputpath, spotInfo);
            SaveSingleJsonDataToFolder(outputpath, enemyTeamInfo);
            SaveSingleJsonDataToFolder(outputpath, enemyInTeamInfo);
            SaveSingleJsonDataToFolder(outputpath, enemyCharInfo);
            SaveSingleJsonDataToFolder(outputpath, campaignInfo);

            File.WriteAllText(Path.Combine(outputpath, "debug_log.txt"), debugLog.ToString());
        }

        public static int CalculateEnemyDifficulty(enemy_character_type_info enemy, Dictionary<string, game_config_info> game_config)
        {
            float[] eea  = BreakStringArray(game_config["enemy_effect_attack"].parameter_value, s => float.Parse(s));
            float[] eed = BreakStringArray(game_config["enemy_effect_defence"].parameter_value, s => float.Parse(s));
            if (eea.Length != 4 || eed.Length != 4)
                return -1;

            // effect_attack = 22 * 扩编* (伤害* 射速 / 50 * 命中 / (命中 + 35) + 2)
            int effect_attack = UECeiling(eea[0] * enemy.number * (enemy.pow * enemy.rate / eea[1] * enemy.hit / (enemy.hit + eea[2]) + eea[3]));
            // effect_defence = 0.25 * (总血量* (35 + 回避) / 35 * 200 / (200 - 护甲) + 100)
            int effect_defence = UECeiling(eed[0] * (enemy.maxlife * (eed[1] + enemy.dodge) / eed[1] * eed[2] / (eed[2] - enemy.armor) + eed[3]));
            int effect = UECeiling(enemy.effect_ratio * (effect_attack + effect_defence));

            return effect;
        }

        public static T[] BreakStringArray<T>(string str, Converter<string, T> converter, char seperator = ',')
        {
            if (str == "")
                return new T[0];

            string[] tokens = str.Split(seperator);
            return Array.ConvertAll(tokens, converter); ;
        }

        public static void ProcessImages(string jsonpath, string imageFodler, string outputFolder, int missionMin, int missionMax)
        {
            var missionInfo = JsonConvert.DeserializeObject<Dictionary<int, mission_info>>(File.ReadAllText(jsonpath));

            Directory.CreateDirectory(outputFolder);

            foreach (var mission in missionInfo.Values)
            {
                if (mission.id < missionMin || mission.id > missionMax)
                    continue;

                var w_all = mission.map_all_width;
                var h_all = mission.map_all_height;
                var w_chop = mission.map_eff_width;
                var h_chop = mission.map_eff_height;
                var x_off = mission.map_offset_x;
                var y_off = mission.map_offset_y;
                var pixelFormat = PixelFormat.Format24bppRgb;
                
                string imageInPath = Path.Combine(imageFodler, mission.map_res_name + ".png");
                Image originalImage = Image.FromFile(imageInPath);
                

                Bitmap scaledImage = new Bitmap(w_all, h_all, pixelFormat);

                //scaledImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (Graphics g = Graphics.FromImage(scaledImage))
                {
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        g.DrawImage(originalImage, new Rectangle(0, 0, w_all, h_all), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }
                originalImage.Dispose();

                if (w_chop < 0)
                {
                    scaledImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    w_chop = -w_chop;
                }
                if (h_chop < 0)
                {
                    scaledImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    h_chop = -h_chop;
                }

                Rectangle chopRect = ConvertMapCoorinates(x_off, y_off, w_chop, h_chop, w_all * 3, h_all * 3);
                Bitmap choppedImage = new Bitmap(chopRect.Width, chopRect.Height, pixelFormat);

                using (Graphics g = Graphics.FromImage(choppedImage))
                {
                    chopImageHelper(g, chopRect, scaledImage, new Rectangle(0, 0, w_all, h_all), RotateFlipType.RotateNoneFlipXY);
                    chopImageHelper(g, chopRect, scaledImage, new Rectangle(w_all, 0, w_all, h_all), RotateFlipType.RotateNoneFlipY);
                    chopImageHelper(g, chopRect, scaledImage, new Rectangle(w_all*2, 0, w_all, h_all), RotateFlipType.RotateNoneFlipXY);
                    chopImageHelper(g, chopRect, scaledImage, new Rectangle(0, h_all, w_all, h_all), RotateFlipType.RotateNoneFlipX);
                    chopImageHelper(g, chopRect, scaledImage, new Rectangle(w_all, h_all, w_all, h_all), RotateFlipType.RotateNoneFlipNone);
                    chopImageHelper(g, chopRect, scaledImage, new Rectangle(w_all*2, h_all, w_all, h_all), RotateFlipType.RotateNoneFlipX);
                    chopImageHelper(g, chopRect, scaledImage, new Rectangle(0, h_all*2, w_all, h_all), RotateFlipType.RotateNoneFlipXY);
                    chopImageHelper(g, chopRect, scaledImage, new Rectangle(w_all, h_all*2, w_all, h_all), RotateFlipType.RotateNoneFlipY);
                    chopImageHelper(g, chopRect, scaledImage, new Rectangle(w_all*2, h_all*2, w_all, h_all), RotateFlipType.RotateNoneFlipXY);
                }
                scaledImage.Dispose();

                string imageOutPath = Path.Combine(outputFolder, string.Format("mission{0}.png", mission.id));
                choppedImage.Save(imageOutPath);
                choppedImage.Dispose();
            }
        }

        private static void chopImageHelper(Graphics g, Rectangle chopRect, Bitmap orgImg, Rectangle regionRect, RotateFlipType rotateFlipType)
        {
            Rectangle intRect = Rectangle.Intersect(chopRect, regionRect);
            if (!intRect.IsEmpty)
            {
                using (Bitmap tmpImg = orgImg.Clone(new Rectangle(0, 0, orgImg.Width, orgImg.Height), orgImg.PixelFormat))
                {
                    tmpImg.RotateFlip(rotateFlipType);
                    Rectangle scrRect = new Rectangle(intRect.X % regionRect.Width, intRect.Y % regionRect.Height, intRect.Width, intRect.Height);
                    Rectangle destRect = new Rectangle(intRect.X - chopRect.X, intRect.Y - chopRect.Y, intRect.Width, intRect.Height);
                    g.DrawImage(tmpImg, destRect, scrRect, GraphicsUnit.Pixel);
                }
            }
        }

        public static Rectangle ConvertMapCoorinates(int game_x, int game_y, int width, int height, int mapWidth, int mapHeight)
        {
            int center_x = mapWidth / 2 + game_x;
            int center_y = mapHeight / 2 - game_y;
            int x = center_x - width / 2;
            int y = center_y - height/ 2;
            return new Rectangle(x, y, width, height);
        }

        public static void Json2Csv(string inputpath, string outputpath)
        {
            string jsonData = File.ReadAllText(inputpath);
            var tmp = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(jsonData);
            string name = tmp.Keys.First();
            var json = tmp[name];
            Json2Csv(json, outputpath);
        }

        public static void ProcessedJson2Csv(string inputpath, string outputpath)
        {
            throw new NotImplementedException();
            string jsonData = File.ReadAllText(inputpath);
            var tmp = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonData);
            var json = tmp.Values.ToList();
            Json2Csv(json, outputpath);
        }

        public static void Json2Csv(List<Dictionary<string, string>> json, string outputpath)
        {
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

            File.WriteAllText(outputpath, sb.ToString());

        }

        public static void ProcessTextTables(string inputpath, string outputpath)
        {
            string textFileListPath = Path.Combine("supplemental", "text_file_list.json");
            var textFileList = JsonConvert.DeserializeObject<Dictionary<string, List<string>>> (File.ReadAllText(textFileListPath));

            using (var output = new StreamWriter(outputpath))
            {
                foreach (var filename in textFileList.Keys)
                {
                    var filepath = Path.Combine(inputpath, filename);
                    var avglines = File.ReadAllLines(filepath);
                    var avgDict = new Dictionary<string, string>();
                    foreach (var line in avglines)
                    {
                        string[] items = line.Split(',');
                        if (items.Length != 2)
                            continue;

                        foreach (var prefix in textFileList[filename])
                        {
                            if (items[0].StartsWith(prefix))
                            {
                                avgDict.Add(items[0], items[1]);
                                break;
                            }
                        }
                    }
                    if (avgDict.Count > 0)
                    {
                        var json = JsonConvert.SerializeObject(avgDict, Formatting.Indented);
                        json = json.Replace("\n  ", "\n\t");
                        json = json.Replace("{", "");
                        json = json.Replace(Environment.NewLine + "}", ",");
                        output.Write(json);
                    }
                }
            }
        }

        public static void Avgtext2Js(string inputpath, string outputpath)
        {
            string[] inputs;
            if (Directory.Exists(inputpath))
            {
                inputs = Directory.GetFiles(inputpath, "*.txt");
            }
            else if (File.Exists(inputpath))
            {
                inputs = new string[1];
                inputs[0] = inputpath;
            }
            else
                throw new FileNotFoundException("File not found", inputpath);

            using (var output = new StreamWriter(outputpath))
            {
                foreach(var input in inputs)
                {
                    var avglines = File.ReadAllLines(input);
                    var avgDict = new Dictionary<string, string>();
                    foreach (var line in avglines)
                    {
                        string[] items = line.Split(',');
                        if (items.Length != 2)
                            continue;
                        avgDict.Add(items[0], items[1]);
                    }
                    if (avgDict.Count > 0)
                    {
                        var json = JsonConvert.SerializeObject(avgDict, Formatting.Indented);
                        json = json.Replace("\n  ", "\n\t");
                        json = json.Replace("{", "");
                        json = json.Replace(Environment.NewLine + "}", ",");
                        output.Write(json);
                    }
                }
            }
        }

        public static int UERound(double f)
        {
            return (int)Math.Round((float)f);
        }

        public static int UECeiling(double f)
        {
            return (int)Math.Ceiling((float)f);
        }
    }
}
