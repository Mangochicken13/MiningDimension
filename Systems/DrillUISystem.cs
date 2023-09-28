using Microsoft.Xna.Framework;
using MiningDimension.UI;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace MiningDimension.Systems
{
    // A lot of this is magic wizardry to me, Majority is copied from the advanced UI wiki
    public class DrillUISystem : ModSystem
    {
        internal UserInterface DrillInterface;

        internal DrillevatorUI DrillUI;

        private GameTime _lastUpdateUiGameTime;

        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (DrillInterface?.CurrentState != null)
            {
                DrillInterface.Update(gameTime);
            }
        }

        public override void Load()
        {
            if (!Main.dedServ)
            {
                DrillInterface = new UserInterface();

                DrillUI = new DrillevatorUI();
            }
            base.Load();
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "MiningDimension: DrillInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && DrillInterface?.CurrentState != null)
                        {
                            DrillInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        public override void PostUpdatePlayers()
        {
            if ((!Main.playerInventory) && DrillInterface?.CurrentState != null)
                ModContent.GetInstance<DrillUISystem>().HideMyUI();
        }

        internal void ShowMyUI()
        {
            DrillInterface?.SetState(DrillUI);
            Main.NewText("Ui shown");
        }

        internal void HideMyUI()
        {
            DrillInterface?.SetState(null);
        }
    }
}
