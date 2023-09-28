﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using SubworldLibrary;
using MiningDimension.Subworlds;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Audio;
using MiningDimension.Systems;

namespace MiningDimension.Tiles
{
    public class DrillevatorTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;

            // wizardry
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);

            // tile entity stuff
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<DrillevatorEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.newTile.UsesCustomCanPlace = true;

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(210, 17, 51), Language.GetText("ItemName.CavernPylon"));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<DrillevatorEntity>().Kill(i, j);
        }

        public override bool RightClick(int i, int j)
        {
            // Example mod code
            // Note to self: Check that this works properly, 
            Player player = Main.LocalPlayer;

            // Should your tile entity bring up a UI, this line is useful to prevent item slots from misbehaving
            Main.mouseRightRelease = false;

            // The following four (4) if-blocks are recommended to be used if your multitile opens a UI when right clicked:
            if (player.sign > -1)
            {
                SoundEngine.PlaySound(SoundID.MenuClose);
                player.sign = -1;
                Main.editSign = false;
                Main.npcChatText = string.Empty;
            }
            if (Main.editChest)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Main.editChest = false;
                Main.npcChatText = string.Empty;
            }
            if (player.editedChestName)
            {
                NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
                player.editedChestName = false;
            }
            if (player.talkNPC > -1)
            {
                player.SetTalkNPC(-1);
                Main.npcChatCornerItem = 0;
                Main.npcChatText = string.Empty;
            }

            if (TileUtils.TryGetTileEntityAs(i, j, out TileEntity entity))
            {
                
                // Do things to your entity here
            }
            
            Main.playerInventory = true;
            ModContent.GetInstance<DrillUISystem>().ShowMyUI();

            return true;
        }
    }

    public class DrillevatorEntity : ModTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];

            return tile.HasTile && tile.TileType == ModContent.TileType<DrillevatorTile>();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                int width = 2;
                int height = 2;
                NetMessage.SendTileSquare(Main.myPlayer, i, j, width, height);

                NetMessage.SendData(MessageID.TileEntityPlacement, number: i, number2: j, number3: type);
            }

            Point16 tileOrigin = new Point16(1, 1);
            int placedEntity = Place(i - tileOrigin.X, j - tileOrigin.Y);
            return placedEntity;
        }

        public override void OnNetPlace()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
            }
        }
    }
}