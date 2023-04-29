﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Plates;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    public class DimensionalSoulArtifact : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dimensional Soul Artifact");
            /* Tooltip.SetDefault("Power\n" +
                "Boosts all damage by 25%, but at what cost?\n" +
                "Increases all damage taken by 15%"); */
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.dArtifact = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ExodiumCluster>(25).
                AddIngredient<Elumplate>(25).
                AddIngredient<CosmiliteBar>(5).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
