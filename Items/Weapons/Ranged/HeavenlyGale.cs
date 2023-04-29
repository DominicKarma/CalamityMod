﻿using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class HeavenlyGale : ModItem
    {
        public const int ShootDelay = 32;

        public const int ArrowsPerBurst = 10;

        public const int ArrowShootRate = 4;

        public const int ArrowShootTime = ArrowsPerBurst * ArrowShootRate;

        public const int MaxChargeTime = 300;

        public const float ArrowTargetingRange = 1100f;

        public const float MaxChargeDamageBoost = 4.5f;

        public const float LightningDamageFactor = 0.36f;

        public const float ChargeLightningCreationThreshold = 0.8f;

        public static readonly SoundStyle FireSound = new("CalamityMod/Sounds/Item/HeavenlyGaleFire");

        public static readonly SoundStyle LightningStrikeSound = new("CalamityMod/Sounds/Custom/HeavenlyGaleLightningStrike");

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Heavenly Gale");
            /* Tooltip.SetDefault("Fires a rapid stream of supercharged exo-crystals\n" +
                "Holding the bow and waiting for some time before firing causes the crystals to become more powerful\n" +
                "If the crystals are sufficiently powerful enough they will summon torrents of exo-lightning above whatever target they hit"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 300;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 138;
            Item.height = 176;
            Item.useAnimation = Item.useTime = ArrowShootTime; // 40
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
            Item.useTurn = true;
            Item.rare = ModContent.RarityType<Violet>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.Calamity().canFirePointBlankShots = true;
        }

        // Shoot via the projectile only
        public override bool CanShoot(Player player) => false;

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        => Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Ranged/HeavenlyGaleGlow").Value);

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlanetaryAnnihilation>().
                AddIngredient<TelluricGlare>().
                AddIngredient<ClockworkBow>().
                AddIngredient<TheBallista>().
                AddIngredient<MiracleMatter>().
                AddTile<DraedonsForge>().
                Register();
        }
    }
}