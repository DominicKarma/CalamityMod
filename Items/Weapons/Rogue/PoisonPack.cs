using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.CalPlayer;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class PoisonPack : RogueWeapon
    {
        private static int damage = 20;
        private static float knockBack = 1.8f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Poison Pack");
            Tooltip.SetDefault("Throws a poisonous spiky ball. Stacks up to 3.\n" +
                "Stealth strikes cause the balls to release spore clouds\n" +
				"Right click to delete all existing spiky balls");
        }

        public override void SafeSetDefaults()
        {
            item.damage = damage;
            item.crit = 4;
            item.Calamity().rogue = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.width = 14;
            item.height = 14;
            item.useTime = 19;
            item.useAnimation = 19;
            item.useStyle = 1;
            item.knockBack = knockBack;
            item.value = Item.buyPrice(0, 0, 33, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.maxStack = 3;

            item.shootSpeed = 7f;
            item.shoot = ModContent.ProjectileType<PoisonBol>();

        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.ownedProjectileCounts[item.shoot] > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
			else
			{
				int UseMax = item.stack;

				if (player.ownedProjectileCounts[item.shoot] >= UseMax)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            CalamityPlayer modPlayer = player.Calamity();
			modPlayer.killSpikyBalls = false;
            if (player.altFunctionUse == 2)
			{
				modPlayer.killSpikyBalls = true;
				return false;
			}
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.SpikyBall, 50);
            recipe.AddIngredient(ItemID.JungleSpore, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/
    }
}
