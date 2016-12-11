using System;
using System.Linq;
using System.Reflection;
using Ensage;
using Ensage.Common;
using Ensage.Common.Menu;
using Ensage.Common.Extensions;
using SharpDX;

namespace mirana_script_for_SD
{


    internal class mirana_script_for_SD : Variables
    {

        public static void Init()
        {
            Game.PrintMessage("I AM WORKING", MessageType.LogMessage);

            Options.MenuInit();

            Events.OnLoad += OnLoad;
            Events.OnClose += OnClose;

        }

        private static void OnClose(object sender, EventArgs e)
        {
            Game.OnUpdate -= ComboUsage;
            Drawing.OnDraw -= TargetIndicator;
            loaded = false;
            me = null;
            target = null;

        }

        private static void OnLoad(object sender, EventArgs e)
        {
            if (!loaded)
            {
                me = ObjectManager.LocalHero;
                 
                if (!Game.IsInGame || me == null || me.Name != heroName)
                {
                    return;
                }

                loaded = true;
                Game.PrintMessage("<font face='Calibri Bold'><font color='#04B404'>" + AssemblyName +
                    " Loaded.</font> (coded by <font color='#0404B4'>theChode</font>) v" + Assembly.GetExecutingAssembly().GetName().Version,
                    MessageType.LogMessage);

                GetAbilities();
                Game.OnUpdate += ComboUsage;
                Drawing.OnDraw += TargetIndicator;

            }

            if (me == null || !me.IsValid)
            {
                loaded = false;
            }
        }


        private static void ComboUsage(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsPaused || Game.IsWatchingGame || Game.IsChatOpen)
                return;

            target = me.ClosestToMouseTarget(closestToMouseRange.GetValue<Slider>().Value);

            if (Game.IsKeyDown(comboKey.GetValue<KeyBind>().Key))
            {
                GetAbilities();

                if (target == null || !target.IsValid || !target.IsVisible || target.IsIllusion || !target.IsAlive ||
                    me.IsChanneling() || target.IsInvul() || HasModifiers()) return;

                if (!Utils.SleepCheck("miranaScriptSleep")) return;

                Orbwalk();

                if (!target.UnitState.HasFlag(UnitState.Hexed) && !target.UnitState.HasFlag(UnitState.Stunned))
                    UseItem(scythe, scythe.GetCastRange());

                UseBlink();
                

                UseItem(veil, veil.GetCastRange());
                UseItem(bloodthorn, bloodthorn.GetCastRange());
                UseItem(orchid, orchid.GetCastRange());
                UseItem(ethereal, ethereal.GetCastRange());

                //CastAbility(starfall, starfall.GetCastRange());
                starfall.UseAbility();

                UseDagon();

                //CastUltimate();

                UseItem(shivas, shivas.GetCastRange());

                Utils.Sleep(150, "miranaScriptSleep");

            }

            
        }


        private static void GetAbilities()
        {
            if (!Utils.SleepCheck("miranaScriptGetAbilities")) return;


            bloodthorn = me.FindItem("item_bloodthorn");
            orchid = me.FindItem("item_orchid");
            ethereal = me.FindItem("item_ethereal_blade");
            dagon = me.GetDagon();
            scythe = me.FindItem("item_sheepstick");
            shivas = me.FindItem("item_shivas_gaurd");
            blink = me.FindItem("item_blink");


            invisUlt = me.FindSpell("mirana_invis");
            leap = me.FindSpell("mirana_leap");
            arrow = me.FindSpell("mirana_arrow");
            starfall = me.FindSpell("mirana_starfall");

            Utils.Sleep(1000, "miranaScriptGetAbilities");

        }


        private static bool HasModifiers()
        {
    
            if (target.HasModifiers(modifierNames, false) ||
                (checkBladeMail.GetValue<bool>() && target.HasModifier("modifier_item_blade_mail_reflect")) ||
                !Utils.SleepCheck("miranaScripSleep"))
                return true;
            Utils.Sleep(100, "miranaScriptHasModifiers");
            return false;
          
        }



        private static void TargetIndicator(EventArgs args)
        {
            if (!drawTarget.GetValue<bool>())
            {
                if (circle == null) return;
                circle.Dispose();
                circle = null;
                return;
            }

            if (target != null && target.IsValid && !target.IsIllusion && target.IsAlive && target.IsVisible
                && me.IsAlive)
            {
                DrawTarget();

            }

            else if(circle != null)
            {
                circle.Dispose();
                circle = null;
            }
        }

        private static void DrawTarget()
        {
            heroIcon = Drawing.GetTexture("materials/ensage_ui/miniheroes/mirana");
            iconSize = new Vector2(HUDInfo.GetHpBarSizeY() * 2);

            if (!Drawing.WorldToScreen(target.Position + new Vector3(0, 0, target.HealthBarOffset / 3), out screenPosition))
                return;

            screenPosition += new Vector2(-iconSize.X, 0);
            Drawing.DrawRect(screenPosition, iconSize, heroIcon);


            if (circle == null)
            {
                circle = new ParticleEffect(@"particles\ui_mouseactions\range_finder_tower_aoe.vpcf", target);
                circle.SetControlPoint(2, me.Position);
                circle.SetControlPoint(6, new Vector3(1, 0, 0));
                circle.SetControlPoint(7, target.Position);
            }

            else
            {
                circle.SetControlPoint(2, me.Position);
                circle.SetControlPoint(6, new Vector3(1, 0, 0));
                circle.SetControlPoint(7, target.Position);
            }

        }

        private static void CastAbility(Ability ability, float range)
        {
            if (ability == null || !ability.CanBeCasted() || ability.IsInAbilityPhase ||
                !target.IsValidTarget(range, true, me.NetworkPosition) ||
                !Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(ability.Name))
                return;


            if(target.Distance2D(me.Position) > 650)
            {

                if (target.UnitState.HasFlag(UnitState.Hexed))
                {
                    if (!target.IsMagicImmune())
                    {
                        ability.UseAbility(Prediction.InFront(target, target.MovementSpeed - 100));
                    }
                }
                else
                {
                    if (!target.IsMagicImmune())
                    {
                        ability.UseAbility(Prediction.InFront(target, target.MovementSpeed - 100));
                    }
                }
            }
            else
            {
                if (!target.IsMagicImmune())
                {
                    ability.UseAbility(target.NetworkPosition);
                }
            }        
                                     
        }

        private static void UseDagon()
        {
            if (dagon == null
                || !dagon.CanBeCasted()
                || target.IsMagicImmune()
                || !(target.NetworkPosition.Distance2D(me) - target.RingRadius <= dagon.CastRange)
                || !Menu.Item("items").GetValue<AbilityToggler>().IsEnabled("item_dagon")
                || !IsFullDebuffed()
                || !Utils.SleepCheck("miranaScriptebsleep")) return;
            dagon.UseAbility(target);                                      
        }

        private static void UseBlink()
        {
            if (!useBlink.GetValue<bool>() || blink == null || !blink.CanBeCasted() ||
                target.Distance2D(me.Position) < 600 || !Utils.SleepCheck("miranaScriptUseBlink"))
                return;

            predictXYZ = target.NetworkActivity == NetworkActivity.Move
                ? Prediction.InFront(target,
                (float)(target.MovementSpeed * (Game.Ping / 1000 + 0.3 + target.GetTurnTime(target))))
                : target.Position;

            if (me.Position.Distance2D(predictXYZ) > 1200)
            {
                predictXYZ = (predictXYZ - me.Position) * 1200 / predictXYZ.Distance2D(me.Position) + me.Position;
            }

            blink.UseAbility(predictXYZ);
            Utils.Sleep(500, "miranaScriptUseBlink");
        }

        private static void UseItem(Item item, float range, int speed = 0)
        {
           
            if (item == null || !item.CanBeCasted() || target.IsMagicImmune() || target.MovementSpeed < speed ||
                target.HasModifier(item.Name) || !target.IsValidTarget(range, true, me.NetworkPosition) ||
                !Menu.Item("items").GetValue<AbilityToggler>().IsEnabled(item.Name))
                return;

            if (item.Name.Contains("ethereal") && IsFullDebuffed())
            {
                
                item.UseAbility(target);

                Utils.Sleep(me.NetworkPosition.Distance2D(target.NetworkPosition) / 1200 * 1000, "miranaScriptebsleep");
                return;
            }
           
            if (item.IsAbilityBehavior(AbilityBehavior.UnitTarget) && !item.Name.Contains("item_dagon"))
            {
                item.UseAbility(target);

                return;
            }
            Game.PrintMessage("I AM some shit", MessageType.LogMessage);
            if (item.IsAbilityBehavior(AbilityBehavior.Point))
            {
                //Game.PrintMessage("I AM WORKING ITEMS eblade", MessageType.LogMessage);
                item.UseAbility(target.NetworkPosition);

                return;
            }

            if (item.IsAbilityBehavior(AbilityBehavior.Immediate))
            {
                item.UseAbility();
          
            }
        }

        private static bool IsFullDebuffed()
        {
            if ((veil != null && veil.CanBeCasted() &&
                Menu.Item("items").GetValue<AbilityToggler>().IsEnabled(veil.Name) &&
                !target.HasModifier("modifier_item_veil_of_discord"))
                ||
                (bloodthorn != null && bloodthorn.CanBeCasted() &&
                Menu.Item("items").GetValue<AbilityToggler>().IsEnabled(bloodthorn.Name) &&
                !target.HasModifier("modifier_item_bloodthorn"))
                ||
                (orchid != null && orchid.CanBeCasted() &&
                Menu.Item("items").GetValue<AbilityToggler>().IsEnabled(orchid.Name) &&
                !target.HasModifier("modifier_item_orchid_malevolence"))
                ||
                (ethereal != null && ethereal.CanBeCasted() &&
                Menu.Item("items").GetValue<AbilityToggler>().IsEnabled(ethereal.Name) &&
                !target.HasModifier("modifier_item_ethereal_blade_slow")))
                return false;

            return true;

        }
        private static void Orbwalk()
        {
            switch (moveMode.GetValue<bool>())
            {
                case true:
                    Orbwalking.Orbwalk(target);
                    break;
                case false:
                    break;
           
            }

        }


        private static float GetDistance2D(Vector3 p1, Vector3 p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p1.Y, 2));
        } 
    }
}
