using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace MiningDimension
{
    public class WorldGenTest : ModSystem
    {
        public static bool JustPressed(Keys key)
        {
            return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
        }

        public override void PostUpdateInput()
        {
            if (JustPressed(Keys.D1))
                TestMethod(Main.MouseWorld.ToTileCoordinates().X, Main.MouseWorld.ToTileCoordinates().Y);
        }

        private void TestMethod(int x, int y)
        {
            Dust.QuickBox(new Vector2(x, y) * 16, new Vector2(x + 1, y + 1) * 16, 2, Color.YellowGreen, null);

            // Code to test placed here:
            //WorldGen.Caverer(x, y);

            //WorldGen.digTunnel(x, y, 2 * (Random.Shared.NextDouble() - 0.5), 2 * (Random.Shared.NextDouble() - 0.5), 25, 3);

            //WorldGen.TileRunner(x - 1, y, WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(2, 8), TileID.CobaltBrick);
        }
    }
}
