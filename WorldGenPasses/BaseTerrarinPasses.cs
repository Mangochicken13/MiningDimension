using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace MiningDimension.WorldGenPasses
{
    // This is an unreadable mess, hopefully the class names are descriptive enough

    // Probably combine a lot of these into one pass, check if that's an appropriate choice
    // One of the important parts here is the use of System.Random.Shared.Next() over Main.rand.Next() or WorldGen.genRand.Next()
    // The latter two would give consistent results between entering and exiting subworlds
    public class BaseDirtPass : GenPass
    {
        public BaseDirtPass() : base("Terrain_Dirt", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            // copied from subworld library wiki example
            progress.Message = "Generating terrain";
            Main.worldSurface = 0;
            Main.rockLayer = Main.maxTilesY / 3;

            // TODO: Try and remove the space layer, or push it above the world at least.

            Main.spawnTileX = Main.maxTilesX / 2 + Main.rand.Next(-50, 50);
            Main.spawnTileY = Main.maxTilesY / 12 + Main.rand.Next(-2, 11);

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    progress.Set((j + i * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY)); // Controls the progress bar, should only be set between 0f and 1f
                    Tile tile = Main.tile[i, j];
                    tile.HasTile = true;
                    tile.TileType = TileID.Dirt;
                }
            }
        }
    }

    public class StonePatchesPass : GenPass
    {
        public StonePatchesPass() : base("MiningDimStone", 1) { }
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Stoning the Ground";

            // Generate stone in a psuedo gradient over the subworld, increasing the frequency going down
            #region Stone Gradient
            // y = 0 -> 80
            int yRangeOld;
            int yRange = (int)(Main.maxTilesY * (1 / 15f)); // For a world with height 1200, this should be 80
            int patches = (int)(Main.maxTilesX * yRange * 0.01);
            int i = 0;

            for (; i < patches; i++)
            {
                int x = Random.Shared.Next(1, Main.maxTilesX);
                int y = Random.Shared.Next(1, yRange);

                WorldGen.TileRunner(x, y, Random.Shared.Next(5, 9), Random.Shared.Next(10, 14), TileID.Stone);
            }

            // y = 80 -> 400
            yRangeOld = yRange; // Get the previous value, should be 80
            yRange = (int)(Main.maxTilesY * (5 / 15f)); // get a new value, should come to 600 for this mod.
            patches = (int)(Main.maxTilesX * yRange * 0.015); // should be about 

            for (; i < patches; i++)
            {
                int x = Random.Shared.Next(1, Main.maxTilesX);
                int y = Random.Shared.Next(yRangeOld, yRange); // Generate a value between the old (80) and new (400) heights

                WorldGen.TileRunner(x, y, Random.Shared.Next(7, 9), Random.Shared.Next(13, 18), TileID.Stone);
            }

            // y = 400 -> 720
            yRangeOld = yRange; // Get the previous value, should be 400
            yRange = (int)(Main.maxTilesY * (9 / 15f)); // get a new value, should come to 600 for this mod.
            patches = (int)(Main.maxTilesX * yRange * 0.017);

            for (; i < patches; i++)
            {
                int x = Random.Shared.Next(1, Main.maxTilesX);
                int y = Random.Shared.Next(yRangeOld, yRange); // Generate a value between the old (80) and new (400) heights

                WorldGen.TileRunner(x, y, Random.Shared.Next(7, 10), Random.Shared.Next(13, 18), TileID.Stone);
            }

            // y = 720 -> 1200
            yRangeOld = yRange;
            yRange = Main.maxTilesY; // maybe do another for 5/6 here instead
            patches = (int)(Main.maxTilesX * yRange * 0.02);

            for (; i < patches; i++)
            {
                int x = Random.Shared.Next(1, Main.maxTilesX);
                int y = Random.Shared.Next(yRangeOld, yRange);

                WorldGen.TileRunner(x, y, Random.Shared.Next(8, 12), Random.Shared.Next(15, 20), TileID.Stone);
            }
            #endregion
        }
    }

    public class CavingPass : GenPass
    {
        public CavingPass() : base("MiningDimCaves", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Scooping out the Terrain";

            //int MediumCaves = (int)(Main.maxTilesX * Main.maxTilesY * 0.0003);
            int MediumCaves = (int)(Main.maxTilesX * Main.maxTilesY * 0.0002);

            for (int i = 0, j = 0; i < MediumCaves;)
            {
                int x = Random.Shared.Next(0, Main.maxTilesX);
                int y = Random.Shared.Next(0, Main.maxTilesY);

                // Only continue if the selected tile isn't air
                if (!Main.tile[x, y].HasTile && j < 500)
                {
                    j++;
                    continue;
                }

                // Note to Self: Try to make your own cave methods, for different shaped variants
                WorldGen.Caverer(x, y);
                progress.Set(i / MediumCaves);

                j = 0;
                i++;
            }

            for (int x = 0; x < Main.maxTilesX; x++)
            {
                // Remove all liquid. Change this in future to generate some areas of liquid and settle it
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Main.tile[x, y].LiquidAmount = 0;
                }
            }
        }
    }

    public class OreGenerationPass : GenPass
    {
        // TODO: make use of a noise map to generate caves
        // also TODO: find out how to do that :smile:
        public OreGenerationPass(int subworldIndex) : base("MiningDimOres", 4)
        {
            SubworldIndex = subworldIndex;
        }

        public int SubworldIndex { get; }
        public Dictionary<int, int> OreTiers { get; set; }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating Ores";
            OreTiers = new Dictionary<int, int>(); // Make sure the dictionary isn't null before using
            MiningDimensionUtils.InitializeOreTiers(OreTiers, SubworldIndex);

            foreach (var pair in OreTiers)
            {
                int oreType = pair.Key;
                int oreCount = pair.Value;

                progress.Message = "Generating " + TileID.Search.GetName(oreType);

                for (int k = 0; k < oreCount; k++)
                {
                    int x = Random.Shared.Next(1, Main.maxTilesX);
                    int y = Random.Shared.Next((int)GenVars.worldSurfaceLow, Main.maxTilesY);

                    WorldGen.OreRunner(x, y, Random.Shared.Next(4, 9), Random.Shared.Next(4, 8), (ushort)oreType);

                }
            }
        }
    }
}
