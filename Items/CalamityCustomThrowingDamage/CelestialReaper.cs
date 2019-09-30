using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.CalamityCustomThrowingDamage
{
    public class CelestialReaper : CalamityDamageItem
    {
        public const int BaseDamage = 90;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Celestial Reaper");
            Tooltip.SetDefault("Throws a fast homing scythe\n" +
                               "The scythe can hit an enemy six times\n" +
                               "On hitting an enemy, the scythe will bounce backwards from the enemy" +
                               "Stealth Strike Effect: Scythe creates damaging afterimages");
        }

        public override void SafeSetDefaults()
        {
            item.damage = BaseDamage;
            item.width = 66;
            item.height = 76;
            item.useAnimation = 31;
            item.useTime = 31;
            item.noMelee = true;
			item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6f;
            item.rare = 10;
            item.UseSound = SoundID.Item71;
			item.autoReuse = true;
            item.value = Item.buyPrice(platinum:2); //sell price of 40 gold
			item.shoot = mod.ProjectileType("CelestialReaperProjectile");
			item.shootSpeed = 20f;
			item.GetGlobalItem<CalamityGlobalItem>(ModLoader.GetMod("CalamityMod")).rogue = true;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 velocity = new Vector2(speedX, speedY);
            float strikeValue = player.GetCalamityPlayer().StealthStrikeAvailable().ToInt(); //0 if false, 1 if true
            Projectile.NewProjectile(position, velocity, mod.ProjectileType("CelestialReaperProjectile"), damage, knockBack, player.whoAmI, strikeValue);
            return false;
        }
    }
}
