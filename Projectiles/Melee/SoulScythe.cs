﻿using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Melee
{
    public class SoulScythe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scythe");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50;
            projectile.aiStyle = 18;
            projectile.alpha = 55;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 3;
            projectile.timeLeft = 420;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            aiType = ProjectileID.DeathSickle;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0f, 0.5f, 0f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 6;
            target.AddBuff(BuffID.OnFire, 300);
            target.AddBuff(ModContent.BuffType<Plague>(), 300);
            if (target.life <= (target.lifeMax * 0.15f))
            {
                Main.PlaySound(2, (int)target.position.X, (int)target.position.Y, 14);
                if (projectile.owner == Main.myPlayer)
                {
					Projectile.NewProjectile(target.Center.X, target.Center.Y, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, ModContent.ProjectileType<SoulScytheExplosion>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(projectile, lightColor, ProjectileID.Sets.TrailingMode[projectile.type], 2);
            return false;
        }
    }
}
