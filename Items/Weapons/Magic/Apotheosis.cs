﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class Apotheosis : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Apotheosis");
            Tooltip.SetDefault("Eat worms\n" +
                "Unleashes interdimensional projection magic");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 77;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 42;
            Item.width = 30;
            Item.height = 34;
            Item.useTime = Item.useAnimation = 177;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.knockBack = 6.9f;

            Item.value = CalamityGlobalItem.Rarity16BuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;

            Item.UseSound = SoundID.Item92;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ApotheosisWorm>();
            Item.shootSpeed = 42.0f;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Magic/ApotheosisGlow").Value);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SpellTome).
                AddIngredient<CosmicDischarge>().
                AddIngredient<StaffoftheMechworm>(2).
                AddIngredient<Excelsus>(2).
                AddIngredient<AscendantSpiritEssence>(11).
                AddIngredient<CosmiliteBar>(33).
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
