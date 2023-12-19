using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Pets
{
    public class LeviPet : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Pets";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 6)
            .WithOffset(-18f, -12f).WithSpriteDirection(-1).WhenNotSelected(0, 0);
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            if (player.dead)
            {
                modPlayer.leviPet = false;
            }
            if (modPlayer.leviPet)
            {
                Projectile.timeLeft = 2;
            }
            Projectile.FloatingPetAI(false, 0.05f);
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 4)
            {
                Projectile.frame = 0;
            }
        }
    }
}
