
using Ensage;
using Ensage.Common.Menu;

namespace mirana_script_for_SD
{
    internal class Options : Variables
    {
        public static void MenuInit()
        {

            


            heroName = "npc_dota_hero_mirana";
            Menu = new Menu(AssemblyName, AssemblyName, true, heroName, true);
            comboKey = new MenuItem("comboKey", "COMBO").SetValue(new KeyBind(70, KeyBindType.Press)).SetTooltip("Full Combo in Logical Order");
            useBlink = new MenuItem("useBlink", "Auto Blink").SetValue(false).SetTooltip("Use Blink During Comob");
            checkBladeMail = new MenuItem("checkBladeMail", "Check Blade Mail").SetValue(false).SetTooltip("Check for BladeMail");
            autoArrowKey = new MenuItem("autoArrowKey", "Auto Arrow Hero").SetValue(new KeyBind(68, KeyBindType.Toggle)).SetTooltip("Auto Arrow Hero Euls/Astral/Imprison within range");
            drawTarget = new MenuItem("drawTarget", "Highlight Target").SetValue(true).SetTooltip("Highlights the Target being Comboed");
            moveMode = new MenuItem("moveMode", "Orb Walk").SetValue(true).SetTooltip("Auto Move While Chasing Enemy");
            closestToMouseRange = new MenuItem("closestToMouseRange", "Target Selection Within This Range").SetValue(new Slider(600, 500, 1200)).SetTooltip("Will Look for Enemy in Selected Range Around Your Mouse Pointer");
            nocastulti = new MenuItem("noCastUlti", "AutoCast Ulti").SetValue(false).SetTooltip("Activate Ulti When Ally is Under Health Threshold");

            noCastUlti = new Menu("Ultimate", "Ultimate");
            items = new Menu("Items", "Items");
            abilities = new Menu("Abilities", "Abilities");
            targetOptions = new Menu("Target  Options", "Target Options");


            Menu.AddItem(comboKey);

            Menu.AddSubMenu(items);
            Menu.AddSubMenu(abilities);
            Menu.AddSubMenu(noCastUlti);
            Menu.AddSubMenu(targetOptions);


            items.AddItem(new MenuItem("items", "Items").SetValue(new AbilityToggler(itemsDictionary)));
            items.AddItem(useBlink);
            items.AddItem(checkBladeMail);
            abilities.AddItem(new MenuItem("abilities", "Abilities").SetValue(new AbilityToggler(abilitiesDictionary)));
            noCastUlti.AddItem(nocastulti);
            targetOptions.AddItem(moveMode);
            targetOptions.AddItem(closestToMouseRange);
            targetOptions.AddItem(drawTarget);


            Menu.AddToMainMenu();

        }

    }

}
