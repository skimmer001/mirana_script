
using System.Collections.Generic;
using Ensage;
using Ensage.Common.Menu;
using SharpDX;

namespace mirana_script_for_SD
{
    internal class Variables
    {
        public const string AssemblyName = "mirana_script_for_SD";
        public static string heroName;

        public static string[] modifierNames =
        {
            "modifier_medusa_stone_gaze_stone", 
            "modifier_winter_wyvern_winters_curse", 
            "modifier_item_lotus_orb_active"
        };

        public static Dictionary<string, Ability> Abilities;
         

        public static Dictionary<string, bool> abilitiesDictionary = new Dictionary<string, bool>
        {
            {"mirana_invis", true},
            {"mirana_leap", true},
            {"mirana_arrow", true},
            {"mirana_starfall", true}
        };


        public static Dictionary<string, bool> itemsDictionary = new Dictionary<string, bool>
        {
            {"item_shivas_guard", true},
            {"item_sheepstick", true},
            {"item_dagon", true},
            {"item_ethereal_blade", true},
            {"item_orchid", true},
            {"item_bloodthorn", true},
            {"item_veil_of_discord", true}

        };


        public static Menu Menu;
        public static Menu items;
        public static Menu abilities;
        public static Menu noCastUlti;
        public static Menu targetOptions;

        public static MenuItem comboKey;
        public static MenuItem useBlink;
        public static MenuItem checkBladeMail;
        public static MenuItem autoArrowKey;
        public static MenuItem drawTarget;
        public static MenuItem moveMode;
        public static MenuItem closestToMouseRange;
        public static MenuItem nocastulti;


        public static bool loaded;

        public static Ability invisUlt,
                              leap, 
                              arrow, 
                              starfall;

        public static Item veil, 
                           bloodthorn, 
                           orchid, 
                           ethereal, 
                           dagon, 
                           scythe, 
                           shivas,
                           blink;


        public static Hero me, 
                           target;


        public static Vector2 iconSize, screenPosition;
        public static Vector3 predictXYZ;
        public static DotaTexture heroIcon;
        public static ParticleEffect circle;


    }
}
