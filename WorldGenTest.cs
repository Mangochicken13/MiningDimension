using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;

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

#pragma warning disable CA1822 // Mark members as static
        private void TestMethod(int x, int y)
#pragma warning restore CA1822 // Mark members as static
        {
            Dust.QuickBox(new Vector2(x, y) * 16, new Vector2(x + 1, y + 1) * 16, 2, Color.YellowGreen, null);

            // Code to test placed here: Chuck whatever you want here, it won't be present in a full release anyway
            //WorldGen.Caverer(x, y);

            //WorldGen.digTunnel(x, y, 2 * (Random.Shared.NextDouble() - 0.5), 2 * (Random.Shared.NextDouble() - 0.5), 25, 3);

            //WorldGen.TileRunner(x - 1, y, WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(2, 8), TileID.CobaltBrick);
        }
    }
}
