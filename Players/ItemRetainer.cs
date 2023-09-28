using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace MiningDimension.Players
{
    public class ItemRetainer : ModPlayer
    {
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
