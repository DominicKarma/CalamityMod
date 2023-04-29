using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Items.Placeables.FurniturePlagued
{
    public class PlaguedPlateTable : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Plagued Table");
        }

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 10;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurniturePlaguedPlate.PlaguedPlateTable>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<PlaguedContainmentBrick>(), 8).AddIngredient(ModContent.ItemType<PlagueCellCanister>(), 2).AddTile(ModContent.TileType<PlagueInfuser>()).Register();
        }
    }
}
