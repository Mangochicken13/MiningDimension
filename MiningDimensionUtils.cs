using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MiningDimension
{
    public class MiningDimensionUtils
    {
        #region Ore Gen Utils
        public static void InitializeOreTiers(Dictionary<int, int> OreTiers, int subworldIndex)
        {
            OreTiers.Clear(); // Make sure ores don't generate in subworlds that shouldn't have them

            int amount = (int)(Main.maxTilesY * Main.maxTilesX * 0.00019); // magic number here, 

            switch (subworldIndex) // only PreBoss and EvilOres have subworlds assigned to them as of now.
            {
                case MiningSubworldID.PreBoss: // preboss ores, copper -> gold tiers
                    //Main.NewText("pre hardmode ores"); // The text here is just for debugging, to make sure the right subworld was entered
                    AddPreHardmodeOres(OreTiers, amount);
                    break;
                case MiningSubworldID.EvilOres: // also adds demonite + crimtane
                    //Main.NewText("evil ores");
                    AddPreHardmodeOres(OreTiers, (int)(amount * 0.95f)); // slightly reducing the frequency or 
                    AddPreWoFOres(OreTiers, amount);
                    break;
                case MiningSubworldID.Hardmode: // also does cobalt -> titanium (both types)
                    Main.NewText("pre mech ores");
                    AddPreHardmodeOres(OreTiers, (int)(amount * 0.9f));
                    AddPreWoFOres(OreTiers, (int)(amount * 0.95f));
                    AddHardmodeOres(OreTiers, amount);
                    break;
                case MiningSubworldID.Jungle: // Only does jungle resources, do custom world gen for this number
                    Main.NewText("Chlorophyte ore");
                    AddChlorophyte(OreTiers, amount);
                    break;
                case MiningSubworldID.Hell: // Only does hell, same as above here too
                    break;
                case MiningSubworldID.Luminite: // Luminite ore gen, post moonlord, do something fancy here. (maybe similar to clam planetoids or 
                    Main.NewText("Luminite ore");
                    AddLuminite(OreTiers, amount);
                    break;

            }
        }

        // These blocks are pretty simple, just adding the ores to the dictionary
        // The base amount can be changed to whatever feels right, either in the method call or in the method itself
        public static void AddPreHardmodeOres(Dictionary<int, int> OreTiers, int amount)
        {
            OreTiers.Add(TileID.Copper, amount);
            OreTiers.Add(TileID.Tin, amount);

            OreTiers.Add(TileID.Iron, amount);
            OreTiers.Add(TileID.Lead, amount);

            OreTiers.Add(TileID.Silver, amount);
            OreTiers.Add(TileID.Tungsten, amount);

            OreTiers.Add(TileID.Gold, amount);
            OreTiers.Add(TileID.Platinum, amount);
        }

        public static void AddPreWoFOres(Dictionary<int, int> OreTiers, int amount)
        {
            OreTiers.Add(TileID.Demonite, (int)(amount * 0.45f));
            OreTiers.Add(TileID.Crimtane, (int)(amount * 0.45f));
            //OreTiers.Add(TileID.Hellstone, (int)(amount * 0.2f)); // Potentially move this to it's own subworld, wait for feedback
        }

        public static void AddHardmodeOres(Dictionary<int, int> OreTiers, int amount)
        {
            OreTiers.Add(TileID.Cobalt, amount);
        }

        public static void AddChlorophyte(Dictionary<int, int> OreTiers, int amount)
        {
            OreTiers.Add(TileID.Chlorophyte, (int)(amount * 0.3f));
        }

        public static void AddLuminite(Dictionary<int, int> OreTiers, int amount)
        {
            OreTiers.Add(TileID.LunarOre, (int)(amount * 0.2f));
        }

        #endregion
    }

    public static class TileUtils // Copied from a wiki, used for the drill (temp sprite) tile entity
    {
        /// <summary>
        /// Atttempts to find the top-left corner of a multitile at location (<paramref name="x"/>, <paramref name="y"/>)
        /// </summary>
        /// <param name="x">The tile X-coordinate</param>
        /// <param name="y">The tile Y-coordinate</param>
        /// <returns>The tile location of the multitile's top-left corner, or the input location if no tile is present or the tile is not part of a multitile</returns>
        public static Point16 GetTopLeftTileInMultitile(int x, int y)
        {
            Tile tile = Main.tile[x, y];

            int frameX = 0;
            int frameY = 0;

            if (tile.HasTile)
            {
                int style = 0, alt = 0;
                TileObjectData.GetTileInfo(tile, ref style, ref alt);
                TileObjectData data = TileObjectData.GetTileData(tile.TileType, style, alt);

                if (data != null)
                {
                    int size = 16 + data.CoordinatePadding;

                    frameX = tile.TileFrameX % (size * data.Width) / size;
                    frameY = tile.TileFrameY % (size * data.Height) / size;
                }
            }

            return new Point16(x - frameX, y - frameY);
        }

        /// <summary>
        /// Uses <seealso cref="GetTopLeftTileInMultitile(int, int)"/> to try to get the entity bound to the multitile at (<paramref name="i"/>, <paramref name="j"/>).
        /// </summary>
        /// <typeparam name="T">The type to get the entity as</typeparam>
        /// <param name="i">The tile X-coordinate</param>
        /// <param name="j">The tile Y-coordinate</param>
        /// <param name="entity">The found <typeparamref name="T"/> instance, if there was one.</param>
        /// <returns><see langword="true"/> if there was a <typeparamref name="T"/> instance, or <see langword="false"/> if there was no entity present OR the entity was not a <typeparamref name="T"/> instance.</returns>
        public static bool TryGetTileEntityAs<T>(int i, int j, out T entity) where T : TileEntity
        {
            Point16 origin = GetTopLeftTileInMultitile(i, j);

            // TileEntity.ByPosition is a Dictionary<Point16, TileEntity> which contains all placed TileEntity instances in the world
            // TryGetValue is used to both check if the dictionary has the key, origin, and get the value from that key if it's there
            if (TileEntity.ByPosition.TryGetValue(origin, out TileEntity existing) && existing is T existingAsT)
            {
                entity = existingAsT;
                return true;
            }

            entity = null;
            return false;
        }
    }

    public class MiscUtils
    {
        public static string GetPrefixName(int prefixId)
        {
            if (prefixId == 0)
                return "none";
            return PrefixLoader.GetPrefix(prefixId)?.Name ?? PrefixID.Search.GetName(prefixId);
        }
    }
}
