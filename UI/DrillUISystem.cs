using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace MiningDimension.UI
{
    // A lot of this is magic wizardry to me, Majority is copied from the advanced UI wiki
    public class DrillUISystem : ModSystem
    {
        internal UserInterface DrillInterface;

        internal DrillevatorUI DrillUI;

        internal ConfirmationUI ConfirmEnterUI;

        // this may have unintended side effects, make sure to test all the ui layers in here to make sure they don't break
        const string vanillaText = "Vanilla: ";
        private static readonly string[] vanillaLayers = new string[]
        {
            vanillaText + "Laser Ruler",
            vanillaText + "Ruler",
            vanillaText + "Fancy UI",
            vanillaText + "Map / Minimap",
            vanillaText + "Inventory",
            vanillaText + "Info Accessories Bar",
            vanillaText + "Builder Accessories Bar",
            vanillaText + "Resource Bars",
            vanillaText + "Hotbar",
            vanillaText + "Player Chat"
        };

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

                ConfirmEnterUI = new ConfirmationUI();
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

            if (DrillInterface?.CurrentState == ConfirmEnterUI)
            {
                for (int i = 0; i < vanillaLayers.Length; i++)
                {
                    int layer = layers.FindIndex(layer => layer.Name.Equals(vanillaLayers[i]));
                    if (layer != -1)
                    {
                        layers.RemoveAt(layer);
                    }
                }
            }
        }

        public override void PostUpdatePlayers()
        {
            if (!Main.playerInventory && DrillInterface?.CurrentState == DrillUI)
                ModContent.GetInstance<DrillUISystem>().HideMyUI();
        }

        internal void ShowSelectionUI()
        {
            DrillInterface?.SetState(DrillUI);
        }

        internal void ShowConfirmationUI(int id)
        {
            DrillInterface?.SetState(ConfirmEnterUI);
            ConfirmEnterUI.SubworldToEnter = id;
        }

        internal void HideMyUI()
        {
            DrillInterface?.SetState(null);
        }
    }
}
