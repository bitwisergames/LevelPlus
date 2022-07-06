﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace levelplus.UI {
    internal enum Stat {
        CONSTITUTION,
        STRENGTH,
        INTELLIGENCE,
        CHARISMA,
        DEXTERITY,
        MOBILITY,
        EXCAVATION,
        ANIMALIA,
        LUCK,
        MYSTICISM
    }

    class StatButton : UIElement {

        public Stat type { get; private set; }
        private float height;
        private float width;
        private UITexture button;
        private UIText points;

        public StatButton(Stat type, float diameter) : this(type, diameter, diameter) { }

        public StatButton(Stat type, float height, float width) {
            this.type = type;
            this.height = height;
            this.width = width;
        }

        public override void OnInitialize() {
            base.OnInitialize();

            Height.Set(height, 0f);
            Width.Set(width, 0f);

            button = new UITexture("levelplus/Textures/UI/Circle", true);

            button.SetPadding(0);
            button.Left.Set(0f, 0f);
            button.Top.Set(0f, 0f);
            button.Width.Set(width, 0f);
            button.Height.Set(height, 0f);

            switch (type) {
                case Stat.CONSTITUTION:
                    button.backgroundColor = Color.LimeGreen; //green
                    break;
                case Stat.STRENGTH:
                    button.backgroundColor = Color.Red; //red
                    break;
                case Stat.INTELLIGENCE:
                    button.backgroundColor = Color.Blue; //blue	
                    break;
                case Stat.CHARISMA:
                    button.backgroundColor = Color.Purple; //purple
                    break;
                case Stat.DEXTERITY:
                    button.backgroundColor = Color.Yellow; //yellow
                    break;
                case Stat.MOBILITY:
                    break;
                case Stat.EXCAVATION:
                    break;
                case Stat.ANIMALIA:
                    break;
                case Stat.LUCK:
                    break;
                case Stat.MYSTICISM:
                    break;
                default:
                    break;
            }

            points = new UIText("0"); //text for showing values
            points.Width.Set(width, 0f);
            points.Height.Set(height, 0f);
            points.Top.Set(height / 2 - points.MinHeight.Pixels / 2, 0f); //center the text, because I'm not a heathen
            this.OnClick += new MouseEvent(pointSpend);


            button.Append(points);
            base.Append(button);
        }

        public override void OnDeactivate() {
            base.OnDeactivate();

            button = null;
            points = null;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            string text;
            int rarity;

            levelplusModPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<levelplusModPlayer>();

            switch (type) {
                case Stat.CONSTITUTION:
                    points.SetText("" + modPlayer.constitution);
                    text = "Constitution:\n\n"
                        + "  +" + (modPlayer.constitution * levelplusConfig.Instance.HealthPerPoint) + " life (+" + (modPlayer.level * levelplusConfig.Instance.HealthPerLevel) + " from level)\n"
                        + "  +" + (modPlayer.constitution / levelplusConfig.Instance.DefensePerPoint) + " defense\n"
                        + "  +" + (modPlayer.constitution / levelplusConfig.Instance.HRegenPerPoint) + " life regen";
                    rarity = 7; //lime
                    break;
                case Stat.STRENGTH:
                    points.SetText("" + modPlayer.strength);
                    text = "Strength:\n\n"
                        + "  +" + ((int) (modPlayer.strength * (levelplusConfig.Instance.MeleeDamagePerPoint * 100))) + "% melee damage\n"
                        + "  +" + (modPlayer.strength / levelplusConfig.Instance.MeleeCritPerPoint) + "% melee crit chance";
                    rarity = 10; //red
                    break;
                case Stat.INTELLIGENCE:
                    points.SetText("" + modPlayer.intelligence);
                    text = "Intelligence:\n\n"
                        + "  +" + ((int) (modPlayer.intelligence * (levelplusConfig.Instance.MagicDamagePerPoint * 100))) + "% magic damage\n"
                        + "  +" + (modPlayer.intelligence / levelplusConfig.Instance.MagicCritPerPoint) + "% magic crit chance";
                    rarity = 9; //cyan
                    break;
                case Stat.CHARISMA:
                    points.SetText("" + modPlayer.charisma);
                    text = "Charisma:\n\n"
                        + "  +" + ((int) (modPlayer.charisma * (levelplusConfig.Instance.SummonDamagePerPoint * 100))) + "% minion damage\n"
                        + "  +" + (modPlayer.charisma / levelplusConfig.Instance.SummonCritPerPoint) + "% minion crit chance";
                    rarity = 6; //light purple
                    break;
                case Stat.DEXTERITY:
                    points.SetText("" + modPlayer.dexterity);
                    text = "Dexterity:\n\n"
                        + "  +" + ((int) (modPlayer.dexterity * (levelplusConfig.Instance.RangedDamagePerPoint * 100))) + "% ranged damage\n"
                        + "  +" + (modPlayer.dexterity / levelplusConfig.Instance.RangedCritPerPoint) + "% ranged crit chance";
                    rarity = 8; //yellow
                    break;
                case Stat.MOBILITY:
                    points.SetText("" + modPlayer.mobility);
                    text = "Mobility:\n\n"
                        + "  +" + ((int) (modPlayer.mobility * (levelplusConfig.Instance.AccelPerPoint * 100))) + "% acceleration\n"
                        + "  +" + ((int) (modPlayer.mobility * (levelplusConfig.Instance.WingPerPoint * 100))) + "% wing time\n"
                        + "  +" + ((int) (modPlayer.mobility * (levelplusConfig.Instance.RunSpeedPerPoint * 100))) + "% max run speed";
                    rarity = 0; //white
                    break;
                case Stat.EXCAVATION:
                    points.SetText("" + modPlayer.excavation);
                    text = "Excavation:\n\n"
                        + "  +" + ((int) (modPlayer.excavation * (levelplusConfig.Instance.PickSpeedPerPoint * 100))) + "% pick speed\n"
                        + "  +" + ((int) (modPlayer.excavation * (levelplusConfig.Instance.BuildSpeedPerPoint * 100))) + "% place speed\n"
                        + "  +" + (modPlayer.excavation / levelplusConfig.Instance.RangePerPoint) + " block range";
                    rarity = 0; //white
                    break;
                case Stat.ANIMALIA:
                    points.SetText("" + modPlayer.animalia);
                    text = "Animalia:\n\n"
                        + "  +" + ((int) (modPlayer.animalia * (levelplusConfig.Instance.FishSkillPerPoint * 100))) + "% better fishing\n"
                        + "  +" + (modPlayer.animalia / levelplusConfig.Instance.MinionPerPoint) + " minion slots\n";
                    //+ "  +" + ((int)(modPlayer.animalia * (levelplusConfig.Instance.MinionKnockBack * 100))) + "% minion knockback";
                    rarity = 0; //white
                    break;
                case Stat.LUCK:
                    points.SetText("" + modPlayer.luck);
                    text = "Luck:\n\n"
                        + "  +" + ((int) (modPlayer.luck * (levelplusConfig.Instance.XPPerPoint * 100))) + "% xp gain\n"
                        + "  +" + ((int) ((modPlayer.luck * 100) / levelplusConfig.Instance.AmmoPerPoint)) + "% chance not to consume ammo";
                    rarity = 0; //white
                    break;
                case Stat.MYSTICISM:
                    points.SetText("" + modPlayer.mysticism);
                    text = "Mysticism:\n\n"
                        + "  +" + (modPlayer.mysticism * levelplusConfig.Instance.ManaPerPoint) + " max mana (+" + (modPlayer.level * levelplusConfig.Instance.ManaPerLevel) + " from level)\n"
                        + "  +" + (modPlayer.mysticism / levelplusConfig.Instance.ManaRegPerPoint) + " mana regen\n"
                        + "  -" + System.Math.Clamp((int) (modPlayer.mysticism * (levelplusConfig.Instance.ManaCostPerPoint * 100)), 0f, 99.0f) + "% mana cost (can't be reduced below 1%)";
                    rarity = 0; //white
                    break;
                default:
                    text = "";
                    rarity = 0;
                    break;
            }

            if (this.IsMouseHovering) {
                Main.instance.MouseText(text, rarity);
            }
        }

        private void pointSpend(UIMouseEvent evt, UIElement listeningElement) {
            SoundEngine.PlaySound(SoundID.MenuTick);
            levelplusModPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<levelplusModPlayer>();
            modPlayer.spend(type, (ushort) (levelplus.SpendModFive.Current ? 5 : levelplus.SpendModTen.Current ? 10 : levelplus.SpendModTwentyFive.Current ? 25 : 1));
        }
    }
}