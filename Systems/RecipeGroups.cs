using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;

namespace MiningDimension.Systems
{
    public class RecipeGroups : ModSystem
    {
        public override void AddRecipeGroups()
        {
            // Copper || Tin
            RecipeGroup group = new(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CopperBar)}", ItemID.CopperBar, ItemID.TinBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.CopperBar), group);
            
            // Silver || Tungsten
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.SilverBar)}", ItemID.SilverBar, ItemID.TungstenBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.SilverBar), group);

            // Gold || Platinum
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.GoldBar)}", ItemID.GoldBar, ItemID.PlatinumBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.GoldBar), group);

            // Demonite || Crimtane
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.DemoniteBar)}", ItemID.DemoniteBar, ItemID.CrimtaneBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.DemoniteBar), group);

            // Cobalt || Palladium
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CobaltBar)}", ItemID.CobaltBar, ItemID.PalladiumBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.CobaltBar), group);

            // Mythril || Orichalcum
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.MythrilBar)}", ItemID.MythrilBar, ItemID.OrichalcumBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.MythrilBar), group);

            // Titanium || Adamantite
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.AdamantiteBar)}", ItemID.AdamantiteBar, ItemID.TitaniumBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.AdamantiteBar), group);
        }
    }
}
