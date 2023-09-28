using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using MiningDimension.UI;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;

namespace MiningDimension.Systems
{
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
