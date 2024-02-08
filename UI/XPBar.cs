using LevelPlus.Utility;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using Terraria.Localization;

namespace LevelPlus.UI {
    class XPBar : UIElement {
        private bool dragging;
        private ResourceBar bar;
        private XPBarButton button;
        private float width;
        private float height;
        private Vector2 offset;

        public XPBar(float width, float height) {
            this.width = width;
            this.height = height;
            offset = Vector2.Zero;
        }

        public override void OnInitialize() {
            base.OnInitialize();

            Height.Set(height, 0f);
            Width.Set(width, 0f);

            button = new XPBarButton(height);
            bar = new ResourceBar(ResourceBarMode.XP, width - (height), height);

            button.Left.Set(0f, 0f);
            button.Top.Set(0f, 0f);

            bar.Left.Set(height, 0f);
            bar.Top.Set(0f, 0f);

            Append(bar);
            Append(button);
        }

        public override void OnDeactivate() {
            base.OnDeactivate();
            bar = null;
            button = null;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            
            if (ClientConfig.Instance.xpBarLocked) {
                return;
            }

            if (dragging) {
                Left.Set(Main.mouseX - offset.X, 0f);
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate();
            }

            Rectangle parentSpace = Parent.GetDimensions().ToRectangle();
            if (!GetDimensions().ToRectangle().Intersects(parentSpace)) {
                Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
                Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
                Recalculate();
            }
        }

        public override void LeftMouseDown(UIMouseEvent evt) {
            base.LeftMouseDown(evt);
            if (ClientConfig.Instance.xpBarLocked) {
                return;
            }
            DragStart(evt);
        }

        public override void LeftMouseUp(UIMouseEvent evt) {
            base.LeftMouseUp(evt);
            DragEnd(evt);
        }

        private void DragStart(UIMouseEvent evt) {
            offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt) {
            if (!dragging) {
                return;
            }
            Vector2 end = evt.MousePosition;
            dragging = false;

            Left.Set(end.X - offset.X, 0f);
            Top.Set(end.Y - offset.Y, 0f);

            Recalculate();
            
            ClientConfig.Instance.xpBarLeft = (int)Left.Pixels;
            ClientConfig.Instance.xpBarTop = (int)Top.Pixels;
        }
    }

    internal class XPBarButton : UIElement {
        private UITexture button;
        private UIText level;
        private float height;
        private float width;

        public XPBarButton(float height) {
            this.height = height;
            this.width = height;
        }

        public override void OnInitialize() {
            base.OnInitialize();

            Height.Set(height, 0f);
            Width.Set(width, 0f);

            button = new UITexture("levelplus/Textures/UI/Hollow_Start", true); //create button
            button.Left.Set(0f, 0f);
            button.Top.Set(0f, 0f);
            button.Width.Set(width, 0f);
            button.Height.Set(height, 0f);
            button.OnLeftClick += new MouseEvent(OpenLevelClicked);

            level = new UIText("0"); //text for showing values
            level.Width.Set(width, 0f);
            level.Height.Set(height, 0f);
            level.Top.Set(height / 2 - level.MinHeight.Pixels / 2, 0f);

            button.Append(level);
            base.Append(button);
        }

        public override void OnDeactivate() {
            base.OnDeactivate();
            level = null;
            button = null;
        }

        public override void Update(GameTime time) {
            base.Update(time);

            LevelPlusModPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<LevelPlusModPlayer>();
            level.SetText("" + (modPlayer.level + 1));
            if (this.IsMouseHovering) {
                int numPlayers = 0;
                float averageLevel = 0;

                foreach (Player i in Main.player)
                    if (i.active) {
                        numPlayers++;
                        averageLevel += i.GetModPlayer<LevelPlusModPlayer>().level + 1;
                    }

                averageLevel /= numPlayers;

                Main.instance.MouseText(Language.GetTextValue("Mods.LevelPlus.UI.XPBar.Local", (modPlayer.level + 1), modPlayer.statPoints) + "\n" + ((Main.netMode == NetmodeID.MultiplayerClient) ? Language.GetTextValue("Mods.LevelPlus.UI.XPBar.Multiplayer", numPlayers, ((int) averageLevel)) : ""));
                //Main.instance.MouseText("Level: " + (modPlayer.level + 1) + "\n" + modPlayer.statPoints + " unspent points\n" + ((Main.netMode == NetmodeID.MultiplayerClient) ? numPlayers + " players online\nAverage Level: " + ((int) averageLevel) : ""));
            }
        }

        private void OpenLevelClicked(UIMouseEvent evt, UIElement listeningElement) {
            SoundEngine.PlaySound(SoundID.MenuTick);
            SpendUI.visible = !SpendUI.visible;
        }
    }
}
