using Terraria.ID;
using Terraria.ModLoader;

namespace MiningDimension.Items
{
    public class DrillevatorItem : ModItem
    {
        // Sprite isn't done yet, intention is some sort of placeable drill.
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DrillevatorTile>());
            Item.value = 150;
            Item.maxStack = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MiningHelmet)
                .AddIngredient(ItemID.StoneBlock, 200)
                .AddIngredient(ItemID.Glass, 20)
                .AddRecipeGroup(RecipeGroupID.Wood, 60)
                .AddRecipeGroup(RecipeGroupID.IronBar, 4)
                .AddTile(TileID.HeavyWorkBench)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }
}
