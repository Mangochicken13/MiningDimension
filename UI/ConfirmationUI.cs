using Microsoft.Xna.Framework;
using MiningDimension.Players;
using MiningDimension.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace MiningDimension.UI
{
    public class ConfirmationUI : UIState
    {

        public int SubworldToEnter = 0;

        public override void OnInitialize()
        {
            UIPanel panel = new();
            panel.Width.Set(500, 0);
            panel.Height.Set(250, 0);
            panel.VAlign = 0.4f;
            panel.HAlign = 0.5f;
            Append(panel);

            UIText header = new("Are you sure you want to go?");
            header.HAlign = 0.5f;
            header.VAlign = 0.3f;
            panel.Append(header);

            UIButtonEnterSubworld confirm = new("Yes", ConfirmEnterSubworld);
            confirm.Width.Set(60, 0);
            confirm.Height.Set(40, 0);
            confirm.HAlign = 0.35f;
            confirm.VAlign = 0.55f;
            panel.Append(confirm);

            UIButtonEnterSubworld deny = new("No", DontEnterSubworld);
            deny.Width.Set(60, 0);
            deny.Height.Set(40, 0);
            deny.HAlign = 0.65f;
            deny.VAlign = 0.55f;
            panel.Append(deny);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }

        private void ConfirmEnterSubworld(UIMouseEvent evt, UIElement listeningElement)
        {
            // Item recovery when entering subworlds
            var player = Main.LocalPlayer;
            if (!player.inventory[58].IsAir)
            {
                var item = player.inventory[58];
                player.GetModPlayer<ItemRetainer>().ItemToRetain = item;
                ModContent.GetInstance<MiningDimension>().Logger.Info($"Attempted to retain item: {item.HoverName}");
            }

            switch (SubworldToEnter) // SubworldToEnter is assigned when this UI is opened: see ShowConformationUI() in UI/DrillUISystem.cs
            {
                case MiningSubworldID.PreBoss:
                    SubworldSystem.Enter<PreBossSubworld>();
                    break;
                case MiningSubworldID.EvilOres:
                    SubworldSystem.Enter<EvilOresSubworld>();
                    break;
                case (_):
                    ModContent.GetInstance<MiningDimension>().Logger.Warn($"Tried to enter an invalid subworld ID: {SubworldToEnter}");
                    break;
            }
            SubworldToEnter = 0;
            ModContent.GetInstance<DrillUISystem>().HideMyUI();
        }

        private void DontEnterSubworld(UIMouseEvent evt, UIElement listeningElement)
        {
            SubworldToEnter = 0;
            ModContent.GetInstance<DrillUISystem>().ShowSelectionUI();
        }
    }
}
