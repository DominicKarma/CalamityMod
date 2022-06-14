﻿using CalamityMod.Items.Placeables.Walls.DraedonStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Items.Placeables.DraedonStructures
{
    public class RustedPlating : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.DraedonStructures.RustedPlating>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(25).
                AddRecipeGroup("IronBar").
                AddRecipeGroup("AnyStoneBlock", 3).
                AddTile(TileID.HeavyWorkBench).
                Register();

            CreateRecipe(1).
                AddIngredient(ModContent.ItemType<LaboratoryPlating>()).
                AddTile(TileID.Anvils).
                Register();

            CreateRecipe(1).
                AddIngredient(ModContent.ItemType<RustedShelf>(), 2).
                Register();

            CreateRecipe(1).
                AddIngredient(ModContent.ItemType<RustedPlatingWall>(), 4).
                AddTile(TileID.WorkBenches).
                Register();

            CreateRecipe(1).
                AddIngredient(ModContent.ItemType<RustedPlateBeam>(), 4).
                AddTile(TileID.WorkBenches).
                Register();

            CreateRecipe(1).
                AddIngredient(ModContent.ItemType<RustedPlatePillar>(), 4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
