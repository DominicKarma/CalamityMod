using CalamityMod.DataStructures;
using CalamityMod.Items.Materials;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.IO;

namespace CalamityMod.Items.Weapons.Melee
{
    public class BiomeBlade : ModItem
    {
        public Attunement mainAttunement = null;
        public Attunement secondaryAttunement = null;
        public int Combo = 0;
        public float ComboResetTimer = 0f;
        public int CanLunge = 1;

        #region stats
        public static int BaseDamage = 47;

        public static int DefaultAttunement_BaseDamage = 47;

        public static int EvilAttunement_BaseDamage = 63;
        public static int EvilAttunement_Lifesteal = 2;
        public static int EvilAttunement_BounceIFrames = 10;

        public static int ColdAttunement_BaseDamage = 58;
        public static float ColdAttunement_SecondSwingBoost = 1.14f; 
        public static float ColdAttunement_ThirdSwingBoost = 1.4f;

        public static int HotAttunement_BaseDamage = 70;
        public static int HotAttunement_FullChargeDamage = 105;
        public static int HotAttunement_ShredIFrames = 8;
        public static int HotAttunement_LocalIFrames = 30; //Be warned its got one extra update so all the iframes should be divided in 2
        public static int HotAttunement_LocalIFramesCharged = 16;
        public static float HotAttunement_ShredDecayRate = 0.7f; //How much charge is lost per frame.

        public static int TropicalAttunement_BaseDamage = 65;
        public static float TropicalAttunement_ChainDamageReduction = 0.6f;
        public static float TropicalAttunement_SweetSpotDamageMultiplier = 1.2f; //It also crits, so be mindful of that
        public static int TropicalAttunement_LocalIFrames = 60; //Be warned its got 2 extra updates so all the iframes should be divided in 3
        #endregion

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Biome Blade"); //Broken Ecoliburn lmfao. Tbh a proper name instead of just "biome blade" may be neat given the importance of the sword
            Tooltip.SetDefault("FUNCTION_DESC\n" +
                               "Hold down RMB while standing still on flat ground to attune the weapon to the powers of the surrounding biome\n" +
                               "Using RMB otherwise switches between the current attunement and an extra stored one\n" +
                               "Main Attunement : [None]\n" +
                               "Secondary Attunement: [None]\n"); //Theres potential for flavor text as well but im not a writer
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            if (list == null)
                return;

            SafeCheckAttunements();

            Player player = Main.player[Main.myPlayer];
            if (player is null)
                return;

            var effectDescTooltip = list.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");
            var mainAttunementTooltip = list.FirstOrDefault(x => x.Name == "Tooltip3" && x.mod == "Terraria");
            var secondaryAttunementTooltip = list.FirstOrDefault(x => x.Name == "Tooltip4" && x.mod == "Terraria");

            //Default stuff
            effectDescTooltip.text = "Does nothing..yet";
            effectDescTooltip.overrideColor = new Color(163, 163, 163);

            mainAttunementTooltip.text = "Main Attunement : [None]";
            mainAttunementTooltip.overrideColor = new Color(163, 163, 163);

            secondaryAttunementTooltip.text = "Secondary Attunement : [None]";
            secondaryAttunementTooltip.overrideColor = new Color(163, 163, 163);

            //If theres a main attunement
            if (mainAttunement != null)
            {
                effectDescTooltip.text = mainAttunement.function_description;
                effectDescTooltip.overrideColor = mainAttunement.tooltipColor;

                mainAttunementTooltip.text = "Main Attunement : [" + mainAttunement.name + "]";
                mainAttunementTooltip.overrideColor = mainAttunement.tooltipColor;
            }

            //If theres a secondary attunement
            if (secondaryAttunement != null)
            {
                secondaryAttunementTooltip.text = "Secondary Attunement : [" + secondaryAttunement.name + "]";
                secondaryAttunementTooltip.overrideColor = Color.Lerp(secondaryAttunement.tooltipColor, Color.Gray, 0.5f);
            }
        }

        public override void SetDefaults()
        {
            item.width = item.height = 36;
            item.damage = BaseDamage;
            item.melee = true;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useTurn = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.shoot = ProjectileID.PurificationPowder; 
            item.knockBack = 5f;
            item.autoReuse = true;
            item.value = Item.buyPrice(0, 4, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.shootSpeed = 12f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("AnyWoodenSword");
            recipe.AddIngredient(ItemType<AerialiteBar>(), 5);
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddIngredient(ItemID.DirtBlock, 50);
            recipe.AddIngredient(ItemID.StoneBlock, 50);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        #region Saving and syncing attunements
        public override bool CloneNewInstances => true;

        public override ModItem Clone(Item item) 
        {
            var clone = base.Clone(item);

            if (Main.mouseItem.type == ItemType<BiomeBlade>())
                item.modItem.HoldItem(Main.player[Main.myPlayer]);

            (clone as BiomeBlade).mainAttunement = (item.modItem as BiomeBlade).mainAttunement;
            (clone as BiomeBlade).secondaryAttunement = (item.modItem as BiomeBlade).secondaryAttunement;

            //As funny as a Broken Broken Biome Blade would be, its also quite funny to make it turn into that. This is only done for a new instance of the item since the goblin tinkerer changes prevent it from happening through reforging
            if (clone.item.prefix == PrefixID.Broken)
            {
                clone.item.Prefix(PrefixID.Legendary);
                clone.item.prefix = PrefixID.Legendary;
            }

            return clone;
        }

        public override ModItem Clone() //ditto
        {
            var clone = base.Clone();

            (clone as BiomeBlade).mainAttunement = mainAttunement;
            (clone as BiomeBlade).secondaryAttunement = secondaryAttunement;

            if (clone.item.prefix == PrefixID.Broken)
            {
                clone.item.Prefix(PrefixID.Legendary);
                clone.item.prefix = PrefixID.Legendary;
            }

            return clone;
        }

        public override TagCompound Save()
        {
            int attunement1 = mainAttunement == null? -1 : (int)mainAttunement.id;
            int attunement2 = secondaryAttunement == null ? -1 : (int)secondaryAttunement.id;
            TagCompound tag = new TagCompound
            {
                { "mainAttunement", attunement1 },
                { "secondaryAttunement", attunement2 }
            };
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            int attunement1 = tag.GetInt("mainAttunement");
            int attunement2 = tag.GetInt("secondaryAttunement");

            mainAttunement = Attunement.attunementArray[attunement1 != -1 ? attunement1 : Attunement.attunementArray.Length - 1];
            secondaryAttunement = Attunement.attunementArray[attunement2 != -1 ? attunement2 : Attunement.attunementArray.Length - 1];

            if (mainAttunement == secondaryAttunement)
                secondaryAttunement = null;

            SafeCheckAttunements();
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(mainAttunement != null ? (byte)mainAttunement.id : Attunement.attunementArray.Length - 1);
            writer.Write(secondaryAttunement != null ? (byte)secondaryAttunement.id : Attunement.attunementArray.Length - 1);
        }

        public override void NetRecieve(BinaryReader reader)
        {
            mainAttunement = Attunement.attunementArray[reader.ReadInt32()];
            secondaryAttunement = Attunement.attunementArray[reader.ReadInt32()];
        }
        #endregion

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            if (mainAttunement == null)
                return;

            mult += mainAttunement.DamageMultiplier - 1;
        }

        public void SafeCheckAttunements()
        {
            if (mainAttunement != null)
                mainAttunement = Attunement.attunementArray[(int)MathHelper.Clamp((float)mainAttunement.id, (float)AttunementID.Default, (float)AttunementID.Evil)];

            if (secondaryAttunement != null)
                secondaryAttunement = Attunement.attunementArray[(int)MathHelper.Clamp((float)secondaryAttunement.id, (float)AttunementID.Default, (float)AttunementID.Evil)];
        }

        public override void HoldItem(Player player)
        {

            player.Calamity().rightClickListener = true;
            player.Calamity().mouseWorldListener = true;

            if (player.velocity.Y == 0) //Reset the lunge ability on ground contact
                CanLunge = 1;

            //Change the swords function based on its attunement
            if (mainAttunement == null)
            {
                item.noUseGraphic = false;
                item.useStyle = ItemUseStyleID.SwingThrow;
                item.noMelee = false;
                item.channel = false;
                item.shoot = ProjectileID.PurificationPowder;
                item.shootSpeed = 12f;
                item.UseSound = SoundID.Item1;
                Combo = 0;
            }

            else
                mainAttunement.ApplyStats(item);

            if (mainAttunement != null && mainAttunement.id != AttunementID.Cold)
                Combo = 0;

            if (player.Calamity().mouseRight && CanUseItem(player) && player.whoAmI == Main.myPlayer && !Main.mapFullscreen)
            {
                //Don't shoot out a visual blade if you already have one out
                if (Main.projectile.Any(n => n.active && n.type == ProjectileType<BrokenBiomeBladeHoldout>() && n.owner == player.whoAmI))
                    return;


                bool mayAttune = player.StandingStill() && !player.mount.Active && player.CheckSolidGround(1, 3);
                Vector2 displace = new Vector2(18f, 0f);
                Projectile.NewProjectile(player.Top + displace, Vector2.Zero, ProjectileType<BrokenBiomeBladeHoldout>(), 0, 0, player.whoAmI, mayAttune ? 0f : 1f);
            }
        }

        public override void UpdateInventory(Player player)
        {
            SafeCheckAttunements();

            if (mainAttunement != null && mainAttunement.id == AttunementID.Cold && CanUseItem(player))
                ComboResetTimer -= 0.02f; //Make the combo counter get closer to being reset

            if (ComboResetTimer < 0)
                Combo = 0;
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.projectile.Any(n => n.active && n.owner == player.whoAmI &&
            (n.type == ProjectileType<BitingEmbrace>() || 
             n.type == ProjectileType<GrovetendersTouch>() || 
             n.type == ProjectileType<AridGrandeur>()));
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (mainAttunement == null)
                return false;


            int powerLungeCounter = 0; //Unused here
            ComboResetTimer = 1f;
            return mainAttunement.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack, ref Combo, ref CanLunge, ref powerLungeCounter);
        }

        internal static ChargingEnergyParticleSet BiomeEnergyParticles = new ChargingEnergyParticleSet(-1, 2, Color.DarkViolet, Color.White, 0.04f, 20f);
        internal static void UpdateAllParticleSets()
        {
            BiomeEnergyParticles.Update();
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D itemTexture = Main.itemTexture[item.type];
            Rectangle itemFrame = (Main.itemAnimations[item.type] == null) ? itemTexture.Frame() : Main.itemAnimations[item.type].GetFrame(itemTexture);

            if (mainAttunement == null)
                return true;

            // Draw all particles.

            Vector2 particleDrawCenter = position + new Vector2(12f, 16f) * Main.inventoryScale;

            BiomeEnergyParticles.EdgeColor = mainAttunement.energyParticleEdgeColor;
            BiomeEnergyParticles.CenterColor = mainAttunement.energyParticleCenterColor;
            BiomeEnergyParticles.InterpolationSpeed = 0.1f;
            BiomeEnergyParticles.DrawSet(particleDrawCenter + Main.screenPosition);

            Vector2 displacement = Vector2.UnitX.RotatedBy(Main.GlobalTime * 3f) * 2f * (float)Math.Sin(Main.GlobalTime);

            spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.UIScaleMatrix);

            spriteBatch.Draw(itemTexture, position + displacement, itemFrame, BiomeEnergyParticles.CenterColor, 0f, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(itemTexture, position - displacement, itemFrame, BiomeEnergyParticles.CenterColor, 0f, origin, scale, SpriteEffects.None, 0f);

            spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);


            return true;
        }
    }
}


