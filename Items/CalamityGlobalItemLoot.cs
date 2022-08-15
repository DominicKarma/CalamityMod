﻿using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.World;
using CalamityMod.Items.Placeables.Ores;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items
{
    public class CalamityGlobalItemLoot : GlobalItem
    {
        public override bool InstancePerEntity => false;

        #region Modify Item Loot Main Hook
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            List<IItemDropRule> rules = itemLoot.Get(false);

            switch (item.type)
            {
                #region Boss Treasure Bags
                /*
                case ItemID.KingSlimeBossBag:
                    DropHelper.DropItemChance(s, player, ModContent.ItemType<CrownJewel>(), 0.1f);
                    break;

                case ItemID.EyeOfCthulhuBossBag:
                    DropHelper.DropItemChance(s, player, ModContent.ItemType<DeathstareRod>(), DropHelper.BagWeaponDropRateInt);
                    DropHelper.DropItemChance(s, player, ModContent.ItemType<TeardropCleaver>(), 0.1f);
                    break;

                case ItemID.EaterOfWorldsBossBag:
                    DropHelper.DropItemCondition(s, player, ItemID.ShadowScale, CalamityWorld.revenge, 60, 120);
                    DropHelper.DropItemCondition(s, player, ItemID.DemoniteOre, CalamityWorld.revenge, 120, 240);
                    break;

                case ItemID.BrainOfCthulhuBossBag:
                    DropHelper.DropItemCondition(s, player, ItemID.TissueSample, CalamityWorld.revenge, 60, 120);
                    DropHelper.DropItemCondition(s, player, ItemID.CrimtaneOre, CalamityWorld.revenge, 100, 180);
                    break;

                case ItemID.QueenBeeBossBag:
                    // Drop weapons Calamity style instead of mutually exclusive.
                    int[] queenBeeWeapons = new int[]
                    {
                        ItemID.BeeKeeper,
                        ItemID.BeesKnees,
                        ItemID.BeeGun
                    };
                    DropHelper.DropEntireSet(s, player, DropHelper.BagWeaponDropRateInt, queenBeeWeapons);
                    DropHelper.BlockDrops(queenBeeWeapons);

                    DropHelper.DropItemChance(s, player, ModContent.ItemType<TheBee>(), 0.1f);

                    DropHelper.DropItem(s, player, ItemID.Stinger, 8, 12); // Extra stingers
                    DropHelper.DropItem(s, player, ModContent.ItemType<HardenedHoneycomb>(), 50, 75);
                    break;

                case ItemID.WallOfFleshBossBag:
                    // Drop weapons Calamity style instead of mutually exclusive -- this includes Calamity weapons.
                    int[] wofWeapons = new int[]
                    {
                        ItemID.BreakerBlade,
                        ItemID.ClockworkAssaultRifle,
                        ModContent.ItemType<Meowthrower>(),
                        ItemID.LaserRifle,
                        ModContent.ItemType<BlackHawkRemote>(),
                        ModContent.ItemType<BlastBarrel>()
                    };
                    DropHelper.DropEntireSet(s, player, DropHelper.BagWeaponDropRateInt, wofWeapons);
                    DropHelper.BlockDrops(wofWeapons);

                    DropHelper.DropItemChance(s, player, ModContent.ItemType<Carnage>(), 0.1f);

                    // Drop emblems Calamity style instead of mutually exclusive -- this includes the Rogue Emblem.
                    int[] emblems = new int[]
                    {
                        ItemID.WarriorEmblem,
                        ItemID.RangerEmblem,
                        ItemID.SorcererEmblem,
                        ItemID.SummonerEmblem,
                        ModContent.ItemType<RogueEmblem>(),
                    };
                    DropHelper.DropEntireSet(s, player, 0.25f, emblems);
                    DropHelper.BlockDrops(emblems);
                    break;

                case ItemID.QueenSlimeBossBag:
                    DropHelper.DropItemChance(s, player, ItemID.SoulofLight, 1f, 15, 20);
                    break;
                case ItemID.DestroyerBossBag:
                    // Only drop hallowed bars after all mechs are down.
                    if ((!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3) && CalamityConfig.Instance.EarlyHardmodeProgressionRework)
                        DropHelper.BlockDrops(ItemID.HallowedBar);

                    break;

                case ItemID.TwinsBossBag:
                    // Only drop hallowed bars after all mechs are down.
                    if ((!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3) && CalamityConfig.Instance.EarlyHardmodeProgressionRework)
                        DropHelper.BlockDrops(ItemID.HallowedBar);

                    DropHelper.DropItemChance(s, player, ModContent.ItemType<Arbalest>(), 0.1f);
                    break;

                case ItemID.SkeletronPrimeBossBag:
                    // Only drop hallowed bars after all mechs are down.
                    if ((!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3) && CalamityConfig.Instance.EarlyHardmodeProgressionRework)
                        DropHelper.BlockDrops(ItemID.HallowedBar);

                    break;

                case ItemID.PlanteraBossBag:
                    // Drop weapons Calamity style instead of mutually exclusive.
                    int[] planteraWeapons = new int[]
                    {
                        ItemID.FlowerPow,
                        ItemID.Seedler,
                        ItemID.GrenadeLauncher,
                        ItemID.VenusMagnum,
                        ItemID.LeafBlower,
                        ItemID.NettleBurst,
                        ItemID.WaspGun,
                        ModContent.ItemType<BloomStone>()
                    };
                    DropHelper.DropEntireSet(s, player, DropHelper.BagWeaponDropRateInt, planteraWeapons);
                    DropHelper.BlockDrops(planteraWeapons);

                    DropHelper.DropItemChance(s, player, ModContent.ItemType<BlossomFlux>(), 0.1f);

                    DropHelper.DropItem(s, player, ModContent.ItemType<LivingShard>(), 30, 35);
                    break;

                case ItemID.FairyQueenBossBag:
                    // Drop weapons Calamity style instead of mutually exclusive -- this includes Calamity weapons.
                    int[] empressWeapons = new int[]
                    {
                        ItemID.FairyQueenMagicItem,
                        ItemID.FairyQueenRangedItem,
                        ItemID.EmpressBlade,
                        ItemID.RainbowWhip,
                        ItemID.PiercingStarlight,
                        ItemID.RainbowWings
                    };
                    DropHelper.DropEntireSet(s, player, DropHelper.BagWeaponDropRateInt, empressWeapons);
                    DropHelper.BlockDrops(empressWeapons);
                    break;

                case ItemID.GolemBossBag:
                    // Drop loot Calamity style instead of mutually exclusive.
                    int[] golemItems = new int[]
                    {
                        ItemID.GolemFist,
                        ItemID.PossessedHatchet,
                        ItemID.Stynger,
                        ItemID.HeatRay,
                        ItemID.StaffofEarth,
                        ItemID.EyeoftheGolem,
                        ItemID.SunStone
                    };
                    DropHelper.DropEntireSet(s, player, DropHelper.BagWeaponDropRateInt, golemItems);
                    DropHelper.BlockDrops(golemItems);

                    DropHelper.DropItemChance(s, player, ModContent.ItemType<AegisBlade>(), 0.1f);
                    DropHelper.DropItem(s, player, ModContent.ItemType<EssenceofSunlight>(), 8, 13);
                    break;

                case ItemID.BossBagBetsy:
                    // Drop weapons Calamity style instead of mutually exclusive.
                    int[] betsyWeapons = new int[]
                    {
                        ItemID.DD2SquireBetsySword, // Flying Dragon
                        ItemID.MonkStaffT3, // Sky Dragon's Fury
                        ItemID.DD2BetsyBow, // Aerial Bane
                        ItemID.ApprenticeStaffT3, // Betsy's Wrath
                    };
                    DropHelper.DropEntireSet(s, player, DropHelper.BagWeaponDropRateInt, betsyWeapons);
                    DropHelper.BlockDrops(betsyWeapons);
                    break;

                case ItemID.FishronBossBag:
                    // Drop weapons Calamity style instead of mutually exclusive -- this includes Calamity weapons.
                    int[] dukeWeapons = new int[]
                    {
                        ItemID.Flairon,
                        ItemID.Tsunami,
                        ItemID.BubbleGun,
                        ItemID.RazorbladeTyphoon,
                        ItemID.TempestStaff,
                        ItemID.FishronWings,
                        ModContent.ItemType<DukesDecapitator>()
                    };
                    DropHelper.DropEntireSet(s, player, DropHelper.BagWeaponDropRateInt, dukeWeapons);
                    DropHelper.BlockDrops(dukeWeapons);

                    DropHelper.DropItemChance(s, player, ModContent.ItemType<BrinyBaron>(), 0.1f);
                    break;

                case ItemID.MoonLordBossBag:
                    // Drop weapons Calamity style instead of mutually exclusive -- this includes Calamity weapons.
                    int[] moonLordWeapons = new int[]
                    {
                        ItemID.Meowmere,
                        ItemID.StarWrath,
                        ItemID.Terrarian,
                        ItemID.FireworksLauncher, // Celebration
                        ItemID.Celeb2,
                        ItemID.SDMG,
                        ItemID.LastPrism,
                        ItemID.LunarFlareBook,
                        ItemID.MoonlordTurretStaff, // Lunar Portal Staff
                        ItemID.RainbowCrystalStaff,
                        ModContent.ItemType<UtensilPoker>(),
                    };
                    DropHelper.DropEntireSet(s, player, DropHelper.BagWeaponDropRateInt, moonLordWeapons);
                    DropHelper.BlockDrops(moonLordWeapons);

                    // The Celestial Onion only drops if the player hasn't used one and doesn't have one in their inventory.
                    int celestialOnion = ModContent.ItemType<CelestialOnion>();
                    DropHelper.DropItemCondition(s, player, celestialOnion, !player.Calamity().extraAccessoryML && !player.InventoryHas(celestialOnion));
                    break;
                */
                #endregion

                #region Fishing Crates
                /*
                case ItemID.WoodenCrate:
                case ItemID.WoodenCrateHard:
                    DropHelper.DropItemChance(s, player, ModContent.ItemType<WulfrumMetalScrap>(), 0.25f, 3, 5);
                    break;

                case ItemID.IronCrate:
                case ItemID.IronCrateHard:
                    DropHelper.DropItemChance(s, player, ModContent.ItemType<WulfrumMetalScrap>(), 0.25f, 5, 8);
                    DropHelper.DropItemChance(s, player, ModContent.ItemType<AncientBoneDust>(), 0.25f, 5, 8);
                    break;

                case ItemID.GoldenCrate:
                    DropHelper.DropItemChance(s, player, ItemID.FlareGun, 0.1f, 1, 1);
                    DropHelper.DropItemChance(s, player, ItemID.ShoeSpikes, 0.1f, 1, 1);
                    DropHelper.DropItemChance(s, player, ItemID.BandofRegeneration, 0.1f, 1, 1);
                    break;

                case ItemID.GoldenCrateHard:
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<AuricOre>(), DownedBossSystem.downedYharon, 0.15f, 30, 40);
                    break;

                case ItemID.CorruptFishingCrate:
                case ItemID.CrimsonFishingCrate:
                case ItemID.CorruptFishingCrateHard:
                case ItemID.CrimsonFishingCrateHard:
                    DropHelper.DropItemChance(s, player, ModContent.ItemType<BlightedGel>(), 0.15f, 5, 8);
                    break;

                case ItemID.HallowedFishingCrate: // WHY
                case ItemID.HallowedFishingCrateHard:
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<UnholyEssence>(), DownedBossSystem.downedProvidence, 0.15f, 5, 10);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<ProfanedRagePotion>(), DownedBossSystem.downedProvidence, 0.15f, 1, 2);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<HolyWrathPotion>(), DownedBossSystem.downedProvidence, 0.15f, 1, 2);
                    break;

                case ItemID.DungeonFishingCrate:
                case ItemID.DungeonFishingCrateHard:
                    DropHelper.DropItemCondition(s, player, ItemID.Ectoplasm, NPC.downedPlantBoss, 0.1f, 1, 5);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<Phantoplasm>(), DownedBossSystem.downedPolterghast, 0.1f, 1, 5);
                    break;

                case ItemID.JungleFishingCrate:
                case ItemID.JungleFishingCrateHard:
                    DropHelper.DropItemChance(s, player, ModContent.ItemType<MurkyPaste>(), 0.2f, 1, 3);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<BeetleJuice>(), Main.hardMode, 0.2f, 1, 3);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<TrapperBulb>(), Main.hardMode, 0.2f, 1, 3);
                    DropHelper.DropItemCondition(s, player, ItemID.ChlorophyteOre, DownedBossSystem.downedCalamitas || NPC.downedPlantBoss, 0.2f, 16, 28);
                    DropHelper.DropItemCondition(s, player, ItemID.ChlorophyteBar, DownedBossSystem.downedCalamitas || NPC.downedPlantBoss, 0.15f, 4, 7);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<PerennialOre>(), NPC.downedPlantBoss, 0.2f, 16, 28);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<PerennialBar>(), NPC.downedPlantBoss, 0.15f, 4, 7);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<PlagueCellCanister>(), NPC.downedGolemBoss, 0.2f, 3, 6);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<UelibloomOre>(), DownedBossSystem.downedProvidence, 0.2f, 16, 28);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<UelibloomBar>(), DownedBossSystem.downedProvidence, 0.15f, 4, 7);
                    break;

                case ItemID.FloatingIslandFishingCrate:
                case ItemID.FloatingIslandFishingCrateHard:
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<AerialiteOre>(), DownedBossSystem.downedHiveMind || DownedBossSystem.downedPerforator, 0.2f, 16, 28);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<AerialiteBar>(), DownedBossSystem.downedHiveMind || DownedBossSystem.downedPerforator, 0.15f, 4, 7);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<EssenceofSunlight>(), Main.hardMode, 0.2f, 2, 4);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<GalacticaSingularity>(), NPC.downedMoonlord, 0.1f, 1, 3);
                    break;

                case ItemID.FrozenCrate:
                case ItemID.FrozenCrateHard:
                    int numMechsDown = NPC.downedMechBoss1.ToInt() + NPC.downedMechBoss2.ToInt() + NPC.downedMechBoss3.ToInt();
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<CryonicOre>(), DownedBossSystem.downedCryogen && numMechsDown >= 2, 0.2f, 16, 28);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<CryonicBar>(), DownedBossSystem.downedCryogen && numMechsDown >= 2, 0.15f, 4, 7);
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<EssenceofEleum>(), Main.hardMode, 0.2f, 2, 4);
                    break;

                case ItemID.LavaCrate:
                case ItemID.LavaCrateHard:
                    DropHelper.DropItemCondition(s, player, ModContent.ItemType<EssenceofChaos>(), Main.hardMode, 0.2f, 2, 4);
                    break;

                // Calamity does not touch Oasis Crates yet
                case ItemID.OasisCrate:
                case ItemID.OasisCrateHard:
                    break;

                // Calamity does not touch Ocean Crates yet
                case ItemID.OceanCrate:
                case ItemID.OceanCrateHard:
                    break;
                */
                #endregion

                #region Miscellaneous
                // Bat Hook is now acquired from Vampires.
                case ItemID.GoodieBag:
                    RemoveBatHookFromGoodieBag(rules);
                    break;
                #endregion
            }
        }
        #endregion

        #region Goodie Bag Bat Hook
        private static void RemoveBatHookFromGoodieBag(IList<IItemDropRule> rules)
        {
            SequentialRulesNotScalingWithLuckRule rule1 = null;
            foreach (IItemDropRule rule in rules)
                if (rule is SequentialRulesNotScalingWithLuckRule s)
                    rule1 = s;
            if (rule1 is null)
                return;
            foreach (IItemDropRule rule in rule1.rules)
                if (rule is CommonDropNotScalingWithLuck rule2 && rule2.itemId == ItemID.BatHook)
                {
                    rule2.chanceNumerator = 0;
                    rule2.chanceDenominator = 1;
                }
        }
        #endregion
    }
}
