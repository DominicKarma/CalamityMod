﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace CalamityMod.Items.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    [LegacyName("CelestialTracers")]
    public class TracersSeraph : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories.Wings";
        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(250, 12f, 3f);
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<Violet>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.controlJump && player.wingTime > 0f && !player.canJumpAgain_Cloud && player.jump == 0 && player.velocity.Y != 0f && !hideVisual)
            {
                int num59 = 4;
                if (player.direction == 1)
                {
                    num59 = -40;
                }
                int num60 = Dust.NewDust(new Vector2(player.position.X + (float)(player.width / 2) + (float)num59, player.position.Y + (float)(player.height / 2) - 15f), 30, 30, 91, 0f, 0f, 100, default, 2.4f);
                Main.dust[num60].noGravity = true;
                Main.dust[num60].velocity *= 0.3f;
                if (Main.rand.NextBool(10))
                {
                    Main.dust[num60].fadeIn = 2f;
                }
                Main.dust[num60].shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
            }
            CalamityPlayer modPlayer = player.Calamity();
            player.accRunSpeed = 10f;
            player.moveSpeed += 0.24f;
            player.iceSkate = true;
            player.waterWalk = true;
            player.fireWalk = true;
            player.lavaImmune = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.noFallDmg = true;
            modPlayer.tracersDust = !hideVisual;
            modPlayer.elysianWingsDust = !hideVisual;
            modPlayer.tracersSeraph = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 1f; //0.85
            ascentWhenRising = 0.175f; //0.15
            maxCanAscendMultiplier = 1.2f; //1
            maxAscentMultiplier = 3.25f; //3
            constantAscend = 0.15f; //0.135
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TracersElysian>().
                AddIngredient<DrewsWings>().
                AddIngredient<AuricBar>(5).
                AddTile<CosmicAnvil>().
                Register();
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            CalamityUtils.DrawInventoryCustomScale(
                spriteBatch,
                texture: TextureAssets.Item[Type].Value,
                position,
                frame,
                drawColor,
                itemColor,
                origin,
                scale,
                wantedScale: 0.8f,
                drawOffset: new(1f, 0f)
            );
            return false;
        }
    }
}