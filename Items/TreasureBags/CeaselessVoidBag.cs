﻿using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Vanity;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs.CeaselessVoid;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.TreasureBags
{
    public class CeaselessVoidBag : ModItem
    {
        public override int BossBagNPC => ModContent.NPCType<CeaselessVoid>();

        public override void SetStaticDefaults()
        {
            SacrificeTotal = 3;
            DisplayName.SetDefault("Treasure Bag (Ceaseless Void)");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
        }

        public override bool CanRightClick() => true;

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return CalamityUtils.DrawTreasureBagInWorld(Item, spriteBatch, ref rotation, ref scale, whoAmI);
        }

        public override void OpenBossBag(Player player)
        {
            // IEntitySource my beloathed
            var s = player.GetSource_OpenItem(Item.type);

            player.TryGettingDevArmor(s);

            // Materials
            DropHelper.DropItem(s, player, ModContent.ItemType<DarkPlasma>(), 6, 9);

            // Weapons
            DropHelper.DropItemChance(s, player, ModContent.ItemType<MirrorBlade>(), DropHelper.BagWeaponDropRateInt);
            DropHelper.DropItemChance(s, player, ModContent.ItemType<VoidConcentrationStaff>(), DropHelper.BagWeaponDropRateInt);

            // Equipment
            DropHelper.DropItem(s, player, ModContent.ItemType<TheEvolution>());

            // Vanity
            DropHelper.DropItemChance(s, player, ModContent.ItemType<CeaselessVoidMask>(), 7);
            if (Main.rand.NextBool(20))
            {
                DropHelper.DropItem(s, player, ModContent.ItemType<AncientGodSlayerHelm>());
                DropHelper.DropItem(s, player, ModContent.ItemType<AncientGodSlayerChestplate>());
                DropHelper.DropItem(s, player, ModContent.ItemType<AncientGodSlayerLeggings>());
            }
        }
    }
}
