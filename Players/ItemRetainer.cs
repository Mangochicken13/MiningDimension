using Terraria;
using Terraria.ModLoader;

namespace MiningDimension.Players
{
    public class ItemRetainer : ModPlayer
    {
        // Items can be lost when entering a subworld, due to the UI requiring the inventory to be open
        // and how UI can be clicked through unless you do some complicated shenanigans
        public Item ItemToRetain = null;

        public override void OnEnterWorld()
        {
            if (ItemToRetain != null)
            {
                var player = Main.LocalPlayer;
                player.QuickSpawnItem(player.GetSource_Misc("Recovering Lost Items"), ItemToRetain, ItemToRetain.stack);
                ItemToRetain = null;
            }
        }
    }
}
