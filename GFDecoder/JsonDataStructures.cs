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
        public float maxlife;
        public float dodge;
        public float pow;
        public float hit;
        public float armor_piercing;
        public float armor;
        public float shield;
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
            result.maxlife = (int)Math.Ceiling(Math.Round(lv_to.maxlife / lv_from.maxlife * this.maxlife * number) / number);
            //result.maxlife = (int) Math.Round(lvX.maxlife / lv100.maxlife * this.maxlife);
            result.pow = (int)Math.Round(lv_to.pow / lv_from.pow * this.pow);
            result.hit = (int)Math.Round(lv_to.hit / lv_from.hit * this.hit);
            result.dodge = (int)Math.Round(lv_to.dodge / lv_from.dodge * this.dodge);
            result.range = this.range;
            result.speed = this.speed;
            result.number = number;
            result.angle = this.angle;
            result.armor_piercing = (int)Math.Round(lv_to.armor_piercing / lv_from.armor_piercing * this.armor_piercing);
            result.armor = (int)Math.Round(lv_to.armor / lv_from.armor * this.armor);
            result.shield = (int)Math.Round(lv_to.shield / lv_from.shield * this.shield);
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
        // imcomplete list
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

}
