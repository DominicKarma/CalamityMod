﻿using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod.World;
using CalamityMod.CalPlayer;
using CalamityMod.Utilities;

namespace CalamityMod.NPCs.BrimstoneWaifu
{
	[AutoloadBossHead]
	public class BrimstoneElemental : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brimstone Elemental");
			Main.npcFrameCount[npc.type] = 12;
		}

		public override void SetDefaults()
		{
			npc.npcSlots = 64f;
			npc.damage = 60;
			npc.width = 100;
			npc.height = 150;
			npc.defense = 15;
			npc.lifeMax = CalamityWorld.revenge ? 35708 : 26000;
			if (CalamityWorld.death)
			{
				npc.lifeMax = 54050;
			}
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			aiType = -1;
			npc.value = Item.buyPrice(0, 12, 0, 0);
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
			}
			npc.buffImmune[BuffID.Ichor] = false;
			npc.buffImmune[mod.BuffType("MarkedforDeath")] = false;
			npc.buffImmune[BuffID.CursedInferno] = false;
			npc.buffImmune[BuffID.Daybreak] = false;
			npc.buffImmune[mod.BuffType("AbyssalFlames")] = false;
			npc.buffImmune[mod.BuffType("ArmorCrunch")] = false;
			npc.buffImmune[mod.BuffType("DemonFlames")] = false;
			npc.buffImmune[mod.BuffType("GodSlayerInferno")] = false;
			npc.buffImmune[mod.BuffType("HolyLight")] = false;
			npc.buffImmune[mod.BuffType("Nightwither")] = false;
			npc.buffImmune[mod.BuffType("Plague")] = false;
			npc.buffImmune[mod.BuffType("Shred")] = false;
			npc.buffImmune[mod.BuffType("WhisperingDeath")] = false;
			npc.buffImmune[mod.BuffType("SilvaStun")] = false;
			npc.boss = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.netAlways = true;
			npc.HitSound = SoundID.NPCHit23;
			npc.DeathSound = SoundID.NPCDeath39;
			Mod calamityModMusic = ModLoader.GetMod("CalamityModMusic");
			if (calamityModMusic != null)
				music = calamityModMusic.GetSoundSlot(SoundType.Music, "Sounds/Music/LeftAlone");
			else
				music = MusicID.Boss4;
			bossBag = mod.ItemType("BrimstoneWaifuBag");
			if (CalamityWorld.downedProvidence)
			{
				npc.damage = 210;
				npc.defense = 120;
				npc.lifeMax = 300000;
				npc.value = Item.buyPrice(0, 35, 0, 0);
			}
			if (CalamityWorld.bossRushActive)
			{
				npc.lifeMax = CalamityWorld.death ? 2300000 : 2000000;
			}
			double HPBoost = (double)Config.BossHealthPercentageBoost * 0.01;
			npc.lifeMax += (int)((double)npc.lifeMax * HPBoost);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(npc.chaseable);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			npc.chaseable = reader.ReadBoolean();
		}

		public override void AI()
		{
			CalamityGlobalNPC.brimstoneElemental = npc.whoAmI;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 2f, 0f, 0f);
			Player player = Main.player[npc.target];
			CalamityPlayer modPlayer = player.GetCalamityPlayer();
			bool provy = (CalamityWorld.downedProvidence && !CalamityWorld.bossRushActive);
			bool expertMode = (Main.expertMode || CalamityWorld.bossRushActive);
			bool revenge = (CalamityWorld.revenge || CalamityWorld.bossRushActive);
			bool calamity = modPlayer.ZoneCalamity;
			npc.TargetClosest(true);
			Vector2 center = new Vector2(npc.Center.X, npc.Center.Y);
			Vector2 vectorCenter = npc.Center;
			float xDistance = player.Center.X - center.X;
			float yDistance = player.Center.Y - center.Y;
			float totalDistance = (float)Math.Sqrt((double)(xDistance * xDistance + yDistance * yDistance));
			int dustAmt = (npc.ai[0] == 2f) ? 2 : 1;
			int size = (npc.ai[0] == 2f) ? 50 : 35;
			float speed = expertMode ? 5f : 4.5f;
			if (CalamityWorld.death || CalamityWorld.bossRushActive)
			{
				speed = 5.5f;
			}
			for (int num1011 = 0; num1011 < 2; num1011++)
			{
				if (Main.rand.Next(3) < dustAmt)
				{
					int dust = Dust.NewDust(npc.Center - new Vector2((float)size), size * 2, size * 2, 235, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 90, default, 1.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.2f;
					Main.dust[dust].fadeIn = 1f;
				}
			}
			if (Vector2.Distance(player.Center, vectorCenter) > 5600f)
			{
				if (npc.timeLeft > 10)
				{
					npc.timeLeft = 10;
				}
			}
			else if (npc.timeLeft > 1800)
			{
				npc.timeLeft = 1800;
			}
			if (npc.GetGlobalNPC<CalamityGlobalNPC>(mod).enraged || (Config.BossRushXerocCurse && CalamityWorld.bossRushActive))
			{
				speed = 11f;
			}
			else if (!calamity)
			{
				speed = 7f;
			}
			else if ((double)npc.life <= (double)npc.lifeMax * 0.65)
			{
				speed = expertMode ? 6f : 5f;
			}
			if (npc.ai[0] <= 2f)
			{
				npc.rotation = npc.velocity.X * 0.04f;
				npc.spriteDirection = ((npc.direction > 0) ? 1 : -1);
				totalDistance = speed / totalDistance;
				xDistance *= totalDistance;
				yDistance *= totalDistance;
				npc.velocity.X = (npc.velocity.X * 50f + xDistance) / 51f;
				npc.velocity.Y = (npc.velocity.Y * 50f + yDistance) / 51f;
			}
			if (npc.ai[0] == 0f)
			{
				npc.defense = provy ? 120 : 20;
				npc.chaseable = true;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.localAI[1] += 1f;
					if (npc.justHit)
					{
						npc.localAI[1] += 1f;
					}
					if (npc.localAI[1] >= 240f)
					{
						npc.localAI[1] = 0f;
						npc.TargetClosest(true);
						int timer = 0;
						int playerPosX;
						int playerPosY;
						while (true)
						{
							timer++;
							playerPosX = (int)player.Center.X / 16;
							playerPosY = (int)player.Center.Y / 16;

							int min = 16;
							int max = 19;

							if (Main.rand.NextBool(2))
								playerPosX += Main.rand.Next(min, max);
							else
								playerPosX -= Main.rand.Next(min, max);

							if (Main.rand.NextBool(2))
								playerPosY += Main.rand.Next(min, max);
							else
								playerPosY -= Main.rand.Next(min, max);

							if (!WorldGen.SolidTile(playerPosX, playerPosY))
								break;

							if (timer > 100)
								return;
						}
						npc.ai[0] = 1f;
						npc.ai[1] = (float)playerPosX;
						npc.ai[2] = (float)playerPosY;
						npc.netUpdate = true;
					}
				}
			}
			else if (npc.ai[0] == 1f)
			{
				npc.defense = provy ? 120 : 20;
				npc.chaseable = true;
				npc.alpha += 25;
				if (npc.alpha >= 255)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient && NPC.CountNPCS(mod.NPCType("Brimling")) < 2 && revenge)
					{
						NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Brimling"), 0, 0f, 0f, 0f, 0f, 255);
					}
					Main.PlaySound(SoundID.Item8, npc.Center);
					npc.alpha = 255;
					npc.position.X = npc.ai[1] * 16f - (float)(npc.width / 2);
					npc.position.Y = npc.ai[2] * 16f - (float)(npc.height / 2);
					npc.ai[0] = 2f;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == 2f)
			{
				npc.alpha -= 25;
				if (npc.alpha <= 0)
				{
					npc.defense = provy ? 120 : 20;
					npc.chaseable = true;
					npc.ai[3] += 1f;
					npc.alpha = 0;
					if (npc.ai[3] >= 2f)
					{
						npc.ai[0] = 3f;
						npc.ai[1] = 0f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
					}
					else
					{
						npc.ai[0] = 0f;
					}
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == 3f)
			{
				npc.defense = provy ? 120 : 20;
				npc.chaseable = true;
				npc.rotation = npc.velocity.X * 0.04f;
				npc.spriteDirection = ((npc.direction > 0) ? 1 : -1);
				Vector2 shootFromVectorX = new Vector2(npc.position.X + (float)(npc.width / 2), npc.position.Y + (float)(npc.height / 2));
				npc.ai[1] += 1f;
				bool shootProjectile = false;
				if (npc.GetGlobalNPC<CalamityGlobalNPC>(mod).enraged || (Config.BossRushXerocCurse && CalamityWorld.bossRushActive))
				{
					if (npc.ai[1] % 10f == 9f)
					{
						shootProjectile = true;
					}
				}
				else if (CalamityWorld.bossRushActive)
				{
					if (npc.ai[1] % 15f == 14f)
					{
						shootProjectile = true;
					}
				}
				else if ((double)npc.life < (double)npc.lifeMax * 0.1)
				{
					if (npc.ai[1] % 20f == 19f)
					{
						shootProjectile = true;
					}
				}
				else if ((double)npc.life < (double)npc.lifeMax * 0.5)
				{
					if (npc.ai[1] % 25f == 24f)
					{
						shootProjectile = true;
					}
				}
				else if (npc.ai[1] % 30f == 29f)
				{
					shootProjectile = true;
				}
				if (shootProjectile && Collision.CanHit(shootFromVectorX, 1, 1, player.position, player.width, player.height))
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						float projectileSpeed = 6f; //changed from 10
						if (npc.GetGlobalNPC<CalamityGlobalNPC>(mod).enraged || (Config.BossRushXerocCurse && CalamityWorld.bossRushActive))
						{
							projectileSpeed += 4f;
						}
						if (revenge)
						{
							projectileSpeed += 1f;
						}
						if ((double)npc.life <= (double)npc.lifeMax * 0.5 || CalamityWorld.bossRushActive)
						{
							projectileSpeed += 1f; //changed from 3 not a prob
						}
						if ((double)npc.life <= (double)npc.lifeMax * 0.1 || CalamityWorld.bossRushActive)
						{
							projectileSpeed += 1f;
						}
						if (!calamity)
						{
							projectileSpeed += 2f;
						}
						float relativeSpeedX = player.position.X + (float)player.width * 0.5f - shootFromVectorX.X;
						float relativeSpeedY = player.position.Y + (float)player.height * 0.5f - shootFromVectorX.Y;
						float totalRelativeSpeed = (float)Math.Sqrt((double)(relativeSpeedX * relativeSpeedX + relativeSpeedY * relativeSpeedY));
						totalRelativeSpeed = projectileSpeed / totalRelativeSpeed;
						relativeSpeedX *= totalRelativeSpeed;
						relativeSpeedY *= totalRelativeSpeed;
						int projectileDamage = expertMode ? 28 : 35;
						int projectileType = mod.ProjectileType("BrimstoneHellfireball");
						int projectileShot = Projectile.NewProjectile(shootFromVectorX.X, shootFromVectorX.Y, relativeSpeedX, relativeSpeedY, projectileType, projectileDamage + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, 0f);
						Main.projectile[projectileShot].timeLeft = 240;
					}
				}
				if (npc.position.Y > player.position.Y - 150f) //200
				{
					if (npc.velocity.Y > 0f)
					{
						npc.velocity.Y = npc.velocity.Y * 0.98f;
					}
					npc.velocity.Y = npc.velocity.Y - 0.1f;
					if (npc.velocity.Y > 3f)
					{
						npc.velocity.Y = 3f;
					}
				}
				else if (npc.position.Y < player.position.Y - 350f) //500
				{
					if (npc.velocity.Y < 0f)
					{
						npc.velocity.Y = npc.velocity.Y * 0.98f;
					}
					npc.velocity.Y = npc.velocity.Y + 0.1f;
					if (npc.velocity.Y < -3f)
					{
						npc.velocity.Y = -3f;
					}
				}
				if (npc.position.X + (float)(npc.width / 2) > player.position.X + (float)(player.width / 2) + 150f) //100
				{
					if (npc.velocity.X > 0f)
					{
						npc.velocity.X = npc.velocity.X * 0.985f;
					}
					npc.velocity.X = npc.velocity.X - 0.1f;
					if (npc.velocity.X > 8f)
					{
						npc.velocity.X = 8f;
					}
				}
				if (npc.position.X + (float)(npc.width / 2) < player.position.X + (float)(player.width / 2) - 150f) //100
				{
					if (npc.velocity.X < 0f)
					{
						npc.velocity.X = npc.velocity.X * 0.985f;
					}
					npc.velocity.X = npc.velocity.X + 0.1f;
					if (npc.velocity.X < -8f)
					{
						npc.velocity.X = -8f;
					}
				}
				if (npc.ai[1] > 300f)
				{
					npc.ai[0] = 4f;
					npc.ai[1] = 0f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == 4f)
			{
				npc.defense = 99999;
				npc.chaseable = false;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.localAI[0] += (float)Main.rand.Next(4);
					if (npc.GetGlobalNPC<CalamityGlobalNPC>(mod).enraged || (Config.BossRushXerocCurse && CalamityWorld.bossRushActive))
					{
						npc.localAI[0] += 3f;
					}
					if (CalamityWorld.death || !calamity)
					{
						npc.localAI[0] += 2f;
					}
					if (npc.localAI[0] >= 140f)
					{
						npc.localAI[0] = 0f;
						npc.TargetClosest(true);
						float projectileSpeed = revenge ? 8f : 6f;
						Vector2 shootFromVector = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
						float num180 = player.position.X + (float)player.width * 0.5f - shootFromVector.X;
						float num181 = Math.Abs(num180) * 0.1f;
						float num182 = player.position.Y + (float)player.height * 0.5f - shootFromVector.Y - num181;
						float num183 = (float)Math.Sqrt((double)(num180 * num180 + num182 * num182));
						npc.netUpdate = true;
						num183 = projectileSpeed / num183;
						num180 *= num183;
						num182 *= num183;
						int num184 = expertMode ? 25 : 30;
						int num185 = mod.ProjectileType("BrimstoneHellblast");
						shootFromVector.X += num180;
						shootFromVector.Y += num182;
						for (int num186 = 0; num186 < 6; num186++)
						{
							num180 = player.position.X + (float)player.width * 0.5f - shootFromVector.X;
							num182 = player.position.Y + (float)player.height * 0.5f - shootFromVector.Y;
							num183 = (float)Math.Sqrt((double)(num180 * num180 + num182 * num182));
							num183 = projectileSpeed / num183;
							num180 += (float)Main.rand.Next(-80, 81);
							num182 += (float)Main.rand.Next(-80, 81);
							num180 *= num183;
							num182 *= num183;
							int projectile = Projectile.NewProjectile(shootFromVector.X, shootFromVector.Y, num180, num182, num185, num184 + (provy ? 30 : 0), 0f, Main.myPlayer, 1f, 0f);
							Main.projectile[projectile].timeLeft = 300;
							Main.projectile[projectile].tileCollide = false;
						}

						int totalProjectiles = 12;
						float spread = MathHelper.ToRadians(30); // 30 degrees in radians = 0.523599
						double startAngle = Math.Atan2(npc.velocity.X, npc.velocity.Y) - spread / 2; // Where the projectiles start spawning at, don't change this
						double deltaAngle = spread / (float)totalProjectiles; // Angle between each projectile, 0.04363325
						double offsetAngle;
						float velocity = 6f;
						int i;
						for (i = 0; i < 6; i++)
						{
							offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i; // Used to be 32
							Projectile.NewProjectile(shootFromVector.X, shootFromVector.Y, (float)(Math.Sin(offsetAngle) * velocity), (float)(Math.Cos(offsetAngle) * velocity), mod.ProjectileType("BrimstoneBarrage"), num184 + (provy ? 30 : 0), 0f, Main.myPlayer, 1f, 0f);
							Projectile.NewProjectile(shootFromVector.X, shootFromVector.Y, (float)(-Math.Sin(offsetAngle) * velocity), (float)(-Math.Cos(offsetAngle) * velocity), mod.ProjectileType("BrimstoneBarrage"), num184 + (provy ? 30 : 0), 0f, Main.myPlayer, 1f, 0f);
						}
					}
				}
				npc.TargetClosest(true);
				npc.ai[1] += 1f;
				npc.velocity *= 0.95f;
				npc.rotation = npc.velocity.X * 0.04f;
				npc.spriteDirection = ((npc.direction > 0) ? 1 : -1);
				if (npc.ai[1] > 300f)
				{
					npc.ai[0] = 0f;
					npc.ai[1] = 0f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
					npc.netUpdate = true;
				}
			}
		}

		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			if (CalamityWorld.revenge)
			{
				player.AddBuff(mod.BuffType("Horror"), 300, true);
			}
		}

		public override void FindFrame(int frameHeight) //9 total frames
		{
			npc.frameCounter += 1.0;
			if (npc.ai[0] <= 2f)
			{
				if (npc.frameCounter > 12.0)
				{
					npc.frame.Y = npc.frame.Y + frameHeight;
					npc.frameCounter = 0.0;
				}
				if (npc.frame.Y >= frameHeight * 4)
				{
					npc.frame.Y = 0;
				}
			}
			else if (npc.ai[0] == 3f)
			{
				if (npc.frameCounter > 12.0)
				{
					npc.frame.Y = npc.frame.Y + frameHeight;
					npc.frameCounter = 0.0;
				}
				if (npc.frame.Y < frameHeight * 4)
				{
					npc.frame.Y = frameHeight * 4;
				}
				if (npc.frame.Y >= frameHeight * 8)
				{
					npc.frame.Y = frameHeight * 4;
				}
			}
			else
			{
				if (npc.frameCounter > 12.0)
				{
					npc.frame.Y = npc.frame.Y + frameHeight;
					npc.frameCounter = 0.0;
				}
				if (npc.frame.Y < frameHeight * 8)
				{
					npc.frame.Y = frameHeight * 8;
				}
				if (npc.frame.Y >= frameHeight * 12)
				{
					npc.frame.Y = frameHeight * 8;
				}
			}
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}

		public override void NPCLoot()
		{
			// redo the rest of this drop code later
			if (Main.rand.NextBool(10))
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BrimstoneElementalTrophy"));
			}
			if (CalamityWorld.armageddon)
			{
				for (int i = 0; i < 5; i++)
				{
					npc.DropBossBags();
				}
			}
			if (Main.expertMode)
			{
				npc.DropBossBags();
			}
			else
			{
				if (CalamityWorld.downedProvidence)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Bloodstone"), Main.rand.Next(20, 31));
				}
				if (Main.rand.NextBool(10))
				{
					npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("RoseStone"), 1, true);
				}
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SoulofFright, Main.rand.Next(20, 41));
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EssenceofChaos"), Main.rand.Next(4, 9));
				switch (Main.rand.Next(3))
				{
					case 0:
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Abaddon"));
						break;
					case 1:
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Brimlance"));
						break;
					case 2:
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SeethingDischarge"));
						break;
				}
			}

			DropHelper.DropResidentEvilAmmo(npc, CalamityWorld.downedBrimstoneElemental, 4, 2, 1);
			DropHelper.DropItemCondition(npc, mod.ItemType("KnowledgeBrimstoneCrag"), true, !CalamityWorld.downedBrimstoneElemental);
			DropHelper.DropItemCondition(npc, mod.ItemType("KnowledgeBrimstoneElemental"), true, !CalamityWorld.downedBrimstoneElemental);

			// if prime hasn't been killed and this is the first time killing brimmy, do the message
			string key = "Mods.CalamityMod.SteelSkullBossText";
			Color messageColor = Color.Crimson;
			if (!NPC.downedMechBoss3 && !CalamityWorld.downedBrimstoneElemental)
			{
				if (Main.netMode == NetmodeID.SinglePlayer)
					Main.NewText(Language.GetTextValue(key), messageColor);
				else if (Main.netMode == NetmodeID.Server)
					NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
			}

			// mark brimmy as dead
			CalamityWorld.downedBrimstoneElemental = true;
            CalamityMod.UpdateServerBoolean();
        }

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.8f);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 235, hitDirection, -1f, 0, default, 1f);
			}
			if (npc.life <= 0)
			{
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 200;
				npc.height = 150;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 40; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 235, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.NextBool(2))
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 60; num623++)
				{
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 235, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 235, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}
				float randomSpread = (float)(Main.rand.Next(-200, 200) / 100);
				Gore.NewGore(npc.position, npc.velocity * randomSpread, mod.GetGoreSlot("Gores/BrimstoneGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity * randomSpread, mod.GetGoreSlot("Gores/BrimstoneGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity * randomSpread, mod.GetGoreSlot("Gores/BrimstoneGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity * randomSpread, mod.GetGoreSlot("Gores/BrimstoneGore4"), 1f);
			}
		}
	}
}
