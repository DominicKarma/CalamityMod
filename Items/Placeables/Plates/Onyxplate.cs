﻿using CalamityMod.Items.Placeables.Walls;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Placeables.Plates
{
    public class Onyxplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Onyxplate");
            Tooltip.SetDefault("It resonates with otherworldly energy.");
            SacrificeTotal = 100;
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.Plates.Onyxplate>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 13;
            Item.height = 10;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 3);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            CreateRecipe(25).
                AddIngredient(ItemID.Obsidian, 50).
                AddTile(TileID.Hellforge).
                Register();
            CreateRecipe().
                AddIngredient<OnyxplateWall>(4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}