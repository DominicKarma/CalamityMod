﻿using System;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class CometQuasher : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";

        internal const float ShootSpeed = 9f;

        internal const int TotalMeteors = 2;

        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 62;
            Item.damage = 141;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 22;
            Item.useTurn = true;
            Item.knockBack = 7.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = ShootSpeed;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 destination = Main.MouseWorld;
            Vector2 position = destination - (Vector2.UnitY * (Main.MouseWorld.Y - Main.screenPosition.Y + 80f));
            Vector2 cachedPosition = position;

            Vector2 velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.UnitY) * ShootSpeed;
            Vector2 cachedVelocity = velocity;

            int meteorDamage = player.CalcIntDamage<MeleeDamageClass>((int)(Item.damage * 0.5));
            float meteorKnockback = Item.knockBack * 0.5f;
            for (int i = 0; i < TotalMeteors; i++)
            {
                position += Vector2.UnitX * Main.rand.Next(-320, 321);
                velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.UnitY) * ShootSpeed * Main.rand.NextFloat(0.9f, 1.1f);
                int proj = Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, velocity, ModContent.ProjectileType<CometQuasherMeteor>(), meteorDamage, meteorKnockback, player.whoAmI, 0f, 0.5f + Main.rand.NextFloat() * 0.3f, target.Center.Y);
                Main.projectile[proj].Calamity().lineColor = Main.rand.Next(3);
                position = cachedPosition;
                velocity = cachedVelocity;
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            Vector2 destination = Main.MouseWorld;
            Vector2 position = destination - (Vector2.UnitY * (Main.MouseWorld.Y - Main.screenPosition.Y + 80f));
            Vector2 cachedPosition = position;

            Vector2 velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.UnitY) * ShootSpeed;
            Vector2 cachedVelocity = velocity;

            int meteorDamage = player.CalcIntDamage<MeleeDamageClass>((int)(Item.damage * 0.5));
            float meteorKnockback = Item.knockBack * 0.5f;
            for (int i = 0; i < TotalMeteors; i++)
            {
                position += Vector2.UnitX * Main.rand.Next(-320, 321);
                velocity *= Main.rand.NextFloat(0.9f, 1.1f);
                int proj = Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, velocity, ModContent.ProjectileType<CometQuasherMeteor>(), meteorDamage, meteorKnockback, player.whoAmI, 0f, 0.5f + Main.rand.NextFloat() * 0.3f, target.Center.Y);
                Main.projectile[proj].Calamity().lineColor = Main.rand.Next(3);
                position = cachedPosition;
                velocity = cachedVelocity;
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Torch);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MeteoriteBar, 25).
                AddIngredient(ItemID.Ectoplasm, 5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
