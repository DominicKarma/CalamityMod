﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Items.Placeables.Banners;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.NPCs.Astral
{
    public class AstralSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Astral Slime");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.damage = 40;
            npc.width = 44;
            npc.height = 28;
            npc.aiStyle = 1;
            npc.defense = 8;
            npc.lifeMax = 200;
            npc.knockBackResist = 0f;
            npc.value = Item.buyPrice(0, 0, 10, 0);
            npc.alpha = 60;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            animationType = NPCID.BlueSlime;
            banner = npc.type;
            bannerItem = ModContent.ItemType<AstralSlimeBanner>();
            if (CalamityWorld.downedAstrageldon)
            {
                npc.damage = 65;
                npc.defense = 18;
                npc.lifeMax = 310;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            //DO DUST
            Dust d = CalamityGlobalNPC.SpawnDustOnNPC(npc, 44, frameHeight, ModContent.DustType<AstralOrange>(), new Rectangle(4, 4, 36, 24), Vector2.Zero, 0.15f, true);
            if (d != null)
            {
                d.customData = 0.04f;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            CalamityGlobalNPC.DoHitDust(npc, hitDirection, ModContent.DustType<AstralOrange>(), 1f, 4, 24);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.ZoneTowerStardust || spawnInfo.player.ZoneTowerSolar || spawnInfo.player.ZoneTowerVortex || spawnInfo.player.ZoneTowerNebula)
            {
                return 0f;
            }
            else if (spawnInfo.player.Calamity().ZoneAstral && (spawnInfo.player.ZoneOverworldHeight || spawnInfo.player.ZoneSkyHeight))
            {
                return 0.21f;
            }
            return 0f;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120, true);
        }

        public override void NPCLoot()
        {
            int oreMin = Main.expertMode ? 11 : 8;
            int oreMax = Main.expertMode ? 16 : 12;
            DropHelper.DropItemCondition(npc, ModContent.ItemType<AstralOre>(), CalamityWorld.downedStarGod, oreMin, oreMax);
			float slimeStaffDrop = (CalamityWorld.defiled ? DropHelper.DefiledDropRateFloat : 0.03f);
            DropHelper.DropItemCondition(npc, ModContent.ItemType<AbandonedSlimeStaff>(), CalamityWorld.downedAstrageldon, slimeStaffDrop, 1, 1);
        }
    }
}
