﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Hybrid;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Sounds;

namespace CalamityMod.Items.Weapons.Typeless
{
    public class AethersWhisper : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aether's Whisper");
            Tooltip.SetDefault("Inflicts long-lasting shadowflame and splits on tile hits\n" +
                "Projectiles gain damage as they travel\n" +
                "Right click to change from magic to ranged damage\n" +
                "Right click consumes no mana");
            SacrificeTotal = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 504;
            Item.knockBack = 5.5f;
            Item.useTime = Item.useAnimation = 24;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<AetherBeam>();
            Item.mana = 30;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;

            Item.width = 134;
            Item.height = 44;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CommonCalamitySounds.LaserCannonSound;

            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.DamageType = DamageClass.Ranged;
                // item.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
            }
            else
            {
                // item.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
                Item.DamageType = DamageClass.Magic;
            }
            return base.CanUseItem(player);
        }

        public override bool OnPickup(Player player)
        {
            // item.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
            Item.DamageType = DamageClass.Magic;
            return true;
        }

        public override void UpdateInventory(Player player)
        {
            // Reset to magic if not using an item to prevent sorting bugs.
            if (player.itemAnimation <= 0)
            {
                // item.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
                Item.DamageType = DamageClass.Magic;
            }
        }

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (player.altFunctionUse == 2)
                mult *= 0f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is coded weirdly because it defaults to using magic boosts without this.
            int dmg = player.GetWeaponDamage(player.ActiveItem());
            float ai0 = player.altFunctionUse == 2 ? 1f : 0f;
            Projectile.NewProjectile(source, position, velocity, type, dmg, knockback, player.whoAmI, ai0, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlasmaRod>().
                AddIngredient<Lazhar>().
                AddIngredient<SpectreRifle>().
                AddIngredient<TwistingNether>(3).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
