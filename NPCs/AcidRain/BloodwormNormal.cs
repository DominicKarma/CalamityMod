﻿using CalamityMod.Items.SummonItems;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalamityMod.NPCs.AcidRain
{
    public class BloodwormNormal : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloodworm");
            Main.npcFrameCount[NPC.type] = 7;
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.width = 36;
            NPC.height = 16;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.catchItem = (short)ModContent.ItemType<BloodwormItem>();
            NPC.dontTakeDamageFromHostiles = true;
            NPC.rarity = 4;
        }

        public override void AI()
        {
            if (NPC.collideY)
            {
                if (NPC.ai[0] == 0f)
                {
                    NPC.ai[0] = Main.rand.NextBool(2).ToDirectionInt();
                    NPC.netUpdate = true;
                }
                if (NPC.collideX)
                {
                    NPC.ai[0] *= -1;
                }
            }
            float xSpeed = 3f;
            NPC.velocity.X = xSpeed * NPC.ai[0];
            NPC.spriteDirection = (int)NPC.ai[0];
            bool flee = false;
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead && Vector2.Distance(player.Center, NPC.Center) <= 220f)
                {
                    flee = true;
                    break;
                }
            }
            int timeBeforeFlee = 60;
            if (flee && NPC.ai[1] < timeBeforeFlee)
            {
                NPC.ai[1] += 1f;
            }
            if (NPC.ai[1] == timeBeforeFlee && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.position.Y += 16f;
                NPC.Transform(ModContent.NPCType<BloodwormFleeing>());
                NPC.netUpdate = true;
                return;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				//BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SulphurousSea,

				// Will move to localization whenever that is cleaned up.
				new FlavorTextBestiaryInfoElement("A ripe-looking worm large enough to curl around your arm. A fish would have to be either mad enough or wise enough, to go after one of these.")
            });
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection, -1f, 0, default, 1f);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Player.Calamity().ZoneSulphur || CalamityWorld.rainingAcid)
                return 0f;

            // Increase bloodworm spawn rate relative to the number of existing bloodworms, parabolic multiplier ranging from 5x spawn rate with 0 blood worms to 1x with 5 or more
            int bloodwormAmt = NPC.CountNPCS(NPC.type);
            float spawnMult = bloodwormAmt > 5 ? 1f : (float)(0.16 * Math.Pow(5 - bloodwormAmt, 2)) + 1f;
            float baseSpawnRate = CalamityWorld.encounteredOldDuke ? 2.569f : 5.138f;
            float spawnRate = SpawnCondition.WormCritter.Chance * baseSpawnRate * spawnMult;

            return spawnRate;
        }

        public override void OnCatchNPC(Player player, Item item)
        {
            try
            {

            }
            catch
            {
                return;
            }
        }
    }
}
