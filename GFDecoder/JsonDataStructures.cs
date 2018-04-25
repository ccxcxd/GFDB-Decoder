using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFDecoder
{
    public class enemy_standard_attribute_info
    {
        public int level;
        public double maxlife;
        public double dodge;
        public double pow;
        public double hit;
        public double armor_piercing;
        public double armor;
        public double shield;
    }

    public class enemy_team_info
    {
        public int id;
        public int enemy_leader;
        public string correction_belong;
        public string correction_turn;
        public string limit_guns;
        public string limit_equips;
        public int ai;
        public string ai_content;

        public List<int> member_ids = new List<int>();
        public int difficulty = 0;
        public int spot_id = 0;
        public List<string> drops = new List<string>();
    }

    public class enemy_in_team_info
    {
        public int id;
        public int enemy_team_id;
        public int enemy_character_type_id;
        public int coordinator_x;
        public int coordinator_y;
        public int level;
        public int number;
        public int is_advance;

        public int difficulty = 0;
        public enemy_character_type_info enemy_character = null;
    }

    public class enemy_character_type_info
    {
        public int id;
        public int type;
        public string name;
        public string code;
        public int maxlife;
        public int pow;
        public int hit;
        public int dodge;
        public int range;
        public int speed;
        public int number;
        public int angle;
        public int armor_piercing;
        public int armor;
        public int shield;
        public int rate;
        public int boss_hp;
        public float debuff_resistance;
        public int level;
        public string character;
        public int special_attack;
        public int normal_attack;
        public string passive_skill;
        public float effect_ratio;
        public string voice;

        public enemy_character_type_info get_info_at_level(int level, int number,
            Dictionary<int, enemy_standard_attribute_info> lv_info)
        {
            enemy_character_type_info result = new enemy_character_type_info();
            enemy_standard_attribute_info lv_from = lv_info[this.level];
            enemy_standard_attribute_info lv_to = lv_info[level];

            result.id = this.id;
            result.type = this.type;
            result.name = this.name;
            result.code = this.code;
            result.maxlife = GFDecoder.UERound(this.maxlife * lv_to.maxlife / lv_from.maxlife * number);
            result.pow = GFDecoder.UERound(this.pow * lv_to.pow / lv_from.pow);
            result.hit = GFDecoder.UERound(this.hit * lv_to.hit / lv_from.hit);
            result.dodge = GFDecoder.UERound(this.dodge * lv_to.dodge / lv_from.dodge);
            result.range = this.range;
            result.speed = this.speed;
            result.number = number;
            result.angle = this.angle;
            result.armor_piercing = GFDecoder.UERound(this.armor_piercing * lv_to.armor_piercing / lv_from.armor_piercing);
            result.armor = GFDecoder.UERound(this.armor * lv_to.armor / lv_from.armor);
            result.shield = GFDecoder.UERound(this.shield * lv_to.shield / lv_from.shield);
            result.rate = this.rate;
            result.boss_hp = this.boss_hp;
            result.debuff_resistance = this.debuff_resistance;
            result.level = level;
            result.character = this.character;
            result.special_attack = this.special_attack;
            result.normal_attack = this.normal_attack;
            result.passive_skill = this.passive_skill;
            result.effect_ratio = this.effect_ratio;
            result.voice = this.voice;

            return result;
        }
    }

    public class trial_info
    {
        public int id;
        public int enemy_team_id;
        public int enemy_level;
        public int enemy_type;
        public int is_night;
        public int reward_voucher;
        public int prize_id;
    }

    public class mission_info
    {
        public int id;
        public int duplicate_type;
        public int campaign;
        public int sub;
        public int endless_mode;
        public int if_emergency;
        public string name;
        public int difficulty;
        public int exp_parameter;
        public int type;
        public int enemy_ai_type;
        public int special_spot_id;
        public int turn_duration;
        public int boss_team_id;
        public int ally_boss_team_id;
        public int coin_type;
        public int costbp;
        public int expect_enemy_die_num;
        public int expect_gun_die_num;
        public int expect_turn;
        public int coin_ap;
        public int special_type;
        public int turn_limit;
        public string limit_gun_pool;
        public string limit_equip_pool;
        public int limit_team;
        public string map_res_name;
        public string map_information;
        public int is_hide;
        public int is_snow;
        public string adaptive_gun;
        public string fog_length;
        public string fog_color;
        public int support_available;
        public int enemy_quickmove;
        public int expect_defend_line_turn;
        public string expect_hostage_num;
        public string title_logo;
        public string random_line_spot;
        public int order;
        public int reinforce_ally_team;
        public int reinforce_ally_turn;
        public int reinforce_ally_spot;
        public string ally_code;
        public string supply_parameter;
        public string close_missions;
        public int mission_group_id;
        public string mission_group_draw_event;
        public string drop_mission_key;
        public string open_mission_keys;
        public string mission_describe;

        public string index_text = "X-X";
        public List<int> spot_ids = new List<int>();
        public Dictionary<int, int> enemy_team_count = new Dictionary<int, int>();
        public int map_all_width = 0;
        public int map_all_height = 0;
        public int map_eff_width = 0;
        public int map_eff_height = 0;
        public int map_offset_x = 0;
        public int map_offset_y = 0;
    }

    public class spot_info
    {
        public int id;
        public int mission_id;
        public int type;
        public string route;
        public int coordinator_x;
        public int coordinator_y;
        public int belong;
        public string hostage_info;
        public int enemy_team_id;
        public int ally_team_id;
        public int map_type;
        public int special_eft;
        public string curve_control;
        public string active_cycle;
        public int durability;
        public string map_route;
        public string map_code;
        public int if_random;
    }

    public class ally_team_info
    {
        public int id;
        public string code;
        public string ui_image_icon;
        public string name;
        public string guns;
        public int fairy;
        public int enemy_team_id;
        public int initial_type;
        public string ai;
        public string ai_content;
        public string betray_condition;
        public string betray_number;
        public string betray_result;
        public string transform_condition;
        public string transform_number;
        public string transform_result;
        public string icon;
    }

    public class game_config_info
    {
        public string parameter_name;
        public string parameter_type;
        public string parameter_value;
        public int client_require;
    }

    public class gun_info
    {
        public int id;
        public string name;
        public string en_name;
        public string introduce;
        public string en_introduce;
        public string code;
    }

    public class equip_info
    {
        public int id;
        public string name;
    }

    /** Custom Supplymental Data **/
    public class event_campaign_info
    {
        public int id;
        public string name;
        public int[] campaign;
    }

    public class campaign_info
    {
        public int id;
        public int type; // 0=main, 1=event, 2=simulation
        public string name;
        public List<int> mission_ids = new List<int>();

        public campaign_info(int id, int type, string name)
        {
            this.id = id;
            this.type = type;
            this.name = name;
        }
    }

    public class mission_extra_enemy_team_info
    {
        public int enemy_team_id_from;
        public int enemy_team_id_to;
        public int mission_id;
    }

}
