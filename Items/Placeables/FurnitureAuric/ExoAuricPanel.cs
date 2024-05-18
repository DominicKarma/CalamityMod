﻿using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Tiles.FurnitureAuric;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Items.Placeables.FurnitureAuric
{
    public class ExoAuricPanel : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeables";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<ExoAuricPanelTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(400).
                AddIngredient<AuricPanel>(400).
                AddIngredient<ExoPrism>().
                AddTile<DraedonsForge>().
                Register();
            /*CreateRecipe().
                AddIngredient<ExoAuricPanelWallItem>(4).
                AddTile(TileID.WorkBenches).
                Register();*/
        }
    }
}