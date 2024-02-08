using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using Terraria.Localization;

namespace LevelPlus.UI {
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
            this.OnLeftClick += new MouseEvent(pointSpend);


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

            LevelPlusModPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<LevelPlusModPlayer>();

            switch (type) {
                case Stat.CONSTITUTION:
                    points.SetText("" + modPlayer.constitution);
                    text = Language.GetTextValue("Mods.LevelPlus.UI.StatButton.Constitution",
                        modPlayer.constitution * LevelPlusConfig.Instance.HealthPerPoint,
                        modPlayer.level * LevelPlusConfig.Instance.HealthPerLevel,
                        modPlayer.constitution / LevelPlusConfig.Instance.DefensePerPoint,
                        modPlayer.constitution / LevelPlusConfig.Instance.HRegenPerPoint);
                    rarity = 7; //lime
                    break;
                case Stat.STRENGTH:
                    points.SetText("" + modPlayer.strength);
                    text = Language.GetTextValue("Mods.LevelPlus.UI.StatButton.Strength",
                        (int) (modPlayer.strength * (LevelPlusConfig.Instance.MeleeDamagePerPoint * 100)),
                        modPlayer.strength / LevelPlusConfig.Instance.MeleeCritPerPoint);
                    rarity = 10; //red
                    break;
                case Stat.INTELLIGENCE:
                    points.SetText("" + modPlayer.intelligence);
                    text = Language.GetTextValue("Mods.LevelPlus.UI.StatButton.Intelligence",
                        (int) (modPlayer.intelligence * (LevelPlusConfig.Instance.MagicDamagePerPoint * 100)),
                        modPlayer.intelligence / LevelPlusConfig.Instance.MagicCritPerPoint);
                    rarity = 9; //cyan
                    break;
                case Stat.CHARISMA:
                    points.SetText("" + modPlayer.charisma);
                    text = text = Language.GetTextValue("Mods.LevelPlus.UI.StatButton.Charisma",
                        (int) (modPlayer.charisma * (LevelPlusConfig.Instance.SummonDamagePerPoint * 100)),
                        modPlayer.charisma / LevelPlusConfig.Instance.SummonCritPerPoint);
                    rarity = 6; //light purple
                    break;
                case Stat.DEXTERITY:
                    points.SetText("" + modPlayer.dexterity);
                    text = Language.GetTextValue("Mods.LevelPlus.UI.StatButton.Dexterity",
                        (int) (modPlayer.dexterity * (LevelPlusConfig.Instance.RangedDamagePerPoint * 100)),
                        modPlayer.dexterity / LevelPlusConfig.Instance.RangedCritPerPoint);
                    rarity = 8; //yellow
                    break;
                case Stat.MOBILITY:
                    points.SetText("" + modPlayer.mobility);
                    text = Language.GetTextValue("Mods.LevelPlus.UI.StatButton.Mobility",
                        (int) (modPlayer.mobility * (LevelPlusConfig.Instance.AccelPerPoint * 100)),
                        (int) (modPlayer.mobility * (LevelPlusConfig.Instance.WingPerPoint * 100)),
                        (int) (modPlayer.mobility * (LevelPlusConfig.Instance.RunSpeedPerPoint * 100)));
                    rarity = 0; //white
                    break;
                case Stat.EXCAVATION:
                    points.SetText("" + modPlayer.excavation);
                    text = Language.GetTextValue("Mods.LevelPlus.UI.StatButton.Excavation",
                        (int) (modPlayer.excavation * (LevelPlusConfig.Instance.PickSpeedPerPoint * 100)),
                        (int) (modPlayer.excavation * (LevelPlusConfig.Instance.BuildSpeedPerPoint * 100)),
                        modPlayer.excavation / LevelPlusConfig.Instance.RangePerPoint);
                    rarity = 0; //white
                    break;
                case Stat.ANIMALIA:
                    points.SetText("" + modPlayer.animalia);
                    text = Language.GetTextValue("Mods.LevelPlus.UI.StatButton.Animalia",
                        (int) (modPlayer.animalia * (LevelPlusConfig.Instance.FishSkillPerPoint * 100)),
                        modPlayer.animalia / LevelPlusConfig.Instance.MinionPerPoint);
                    //+ "  +" + ((int)(modPlayer.animalia * (levelplusConfig.Instance.MinionKnockBack * 100))) + "% minion knockback";
                    rarity = 0; //white
                    break;
                case Stat.LUCK:
                    points.SetText("" + modPlayer.luck);
                    text = Language.GetTextValue("Mods.LevelPlus.UI.StatButton.Luck",
                        (int) (modPlayer.luck * (LevelPlusConfig.Instance.XPPerPoint * 100)),
                        (int) ((modPlayer.luck * 100) / LevelPlusConfig.Instance.AmmoPerPoint));
                    rarity = 0; //white
                    break;
                case Stat.MYSTICISM:
                    points.SetText("" + modPlayer.mysticism);
                    text = Language.GetTextValue("Mods.LevelPlus.UI.StatButton.Mysticism",
                        modPlayer.mysticism * LevelPlusConfig.Instance.ManaPerPoint,
                        modPlayer.level * LevelPlusConfig.Instance.ManaPerLevel,
                        modPlayer.mysticism / LevelPlusConfig.Instance.ManaRegPerPoint,
                        System.Math.Clamp((int) (modPlayer.mysticism * (LevelPlusConfig.Instance.ManaCostPerPoint * 100)), 0f, 99.0f));
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
            LevelPlusModPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<LevelPlusModPlayer>();
            modPlayer.spend(type, (ushort) (LevelPlus.SpendModFive.Current ? 5 : LevelPlus.SpendModTen.Current ? 10 : LevelPlus.SpendModAll.Current ? modPlayer.statPoints : 1));
        }
    }
}
