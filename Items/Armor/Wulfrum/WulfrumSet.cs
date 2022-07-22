﻿using System;
using System.Collections.Generic;
using CalamityMod.Cooldowns;
using CalamityMod.Items.Accessories.Vanity;
using CalamityMod.Items.Materials;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Microsoft.Xna.Framework.Input.Keys;
using static Terraria.ModLoader.ModContent;

namespace CalamityMod.Items.Armor.Wulfrum
{
    #region Armor pieces

    //Datsuzei / Moonstone armor from starlight river was a nice jumping off point for this kind of set.

    [AutoloadEquip(EquipType.Head)]
    [LegacyName("WulfrumHelmet")]
    [LegacyName("WulfrumHeadSummon")]
    public class WulfrumHat : ModItem, IExtendedHat
    {
        #region big hat
        public string ExtensionTexture => "CalamityMod/Items/Armor/Wulfrum/WulfrumHat_HeadExtension";
        public Vector2 ExtensionSpriteOffset(PlayerDrawSet drawInfo) => -Vector2.UnitY * 2f;
        public string EquipSlotName(Player drawPlayer) => drawPlayer.Male ? Name : "WulfrumHatFemale";
        #endregion

        public static readonly SoundStyle SetActivationSound = new("CalamityMod/Sounds/Custom/AbilitySounds/WulfrumBastionActivate");
        public static readonly SoundStyle SetBreakSound = new("CalamityMod/Sounds/Custom/AbilitySounds/WulfrumBastionBreak");
        public static readonly SoundStyle SetBreakSoundSafe = new("CalamityMod/Sounds/Custom/AbilitySounds/WulfrumBastionBreakSafely");

        public static int BastionBuildTime = (int)(0.55f * 60);
        public static int BastionTime = 30 * 60;
        public static int TimeLostPerHit = 2 * 60;
        public static int BastionCooldown = 20 * 60;


        internal static Item DummyCannon = new Item(); //Used for the attack swap. Basically we force the player to hold a fake item.

        public static bool PowerModeEngaged(Player player, out CooldownInstance cd)
        {
            cd = null;
            bool hasWulfrumBastionCD = player.Calamity().cooldowns.TryGetValue(WulfrumBastion.ID, out cd);
            return (hasWulfrumBastionCD && cd.timeLeft > BastionCooldown);
        }

        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                EquipLoader.AddEquipTexture(Mod, "CalamityMod/Items/Armor/Wulfrum/WulfrumHat_FemaleHead", EquipType.Head, name : "WulfrumHatFemale");
            }

            On.Terraria.Player.KeyDoubleTap += ActivateSetBonus;
            On.Terraria.Main.DrawPendingMouseText += SpoofMouseItem;
        }

        public override void Unload()
        {
            On.Terraria.Player.KeyDoubleTap -= ActivateSetBonus;
            On.Terraria.Main.DrawPendingMouseText -= SpoofMouseItem;


            DummyCannon.TurnToAir();
            DummyCannon = null;
        }

        private void ActivateSetBonus(On.Terraria.Player.orig_KeyDoubleTap orig, Player player, int keyDir)
        {
            if (keyDir == 0 && HasArmorSet(player) && !player.mount.Active)
            {
                //Only activate if no cooldown & available scrap.
                if (player.Calamity().cooldowns.TryGetValue(WulfrumBastion.ID, out CooldownInstance cd))
                {
                    if (cd.timeLeft > BastionCooldown && cd.timeLeft < BastionCooldown + BastionTime - 60 * 3)
                    {
                        cd.timeLeft = BastionCooldown + 1;
                        player.Calamity().SyncCooldownDictionary(false);
                    }
                }

                else if (player.HasItem(ModContent.ItemType<WulfrumMetalScrap>()))
                {
                    player.ConsumeItem(ModContent.ItemType<WulfrumMetalScrap>());
                    //I Thiiiinnnk there's no need to add mp syncing packets sicne cooldowns get auto synced right.
                    player.AddCooldown(WulfrumBastion.ID, BastionCooldown + BastionTime);
                    //Though do i need to sync that or is the player inventory auto synced?
                    DummyCannon.SetDefaults(ItemType<WulfrumFusionCannon>());
                }
            }

            orig(player, keyDir);
        }

        //Replaces the tooltip of the armor set with the fusion cannon if the player holds shift
        private void SpoofMouseItem(On.Terraria.Main.orig_DrawPendingMouseText orig)
        {
            var player = Main.LocalPlayer;

            if (DummyCannon.IsAir && !Main.gameMenu)
                DummyCannon.SetDefaults(ItemType<WulfrumFusionCannon>());

            if (IsPartOfSet(Main.HoverItem) && HasArmorSet(player) && Main.keyState.IsKeyDown(LeftShift))
            {
                Main.HoverItem = DummyCannon.Clone();
                Main.hoverItemName = DummyCannon.Name;
            }

            orig();
        }


        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            DisplayName.SetDefault("Wulfrum Hat & Goggles");
            Tooltip.SetDefault("10% increased minion damage\n"+
                "Comes equipped with hair extensions" );
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Blue;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) =>  body.type == ItemType<WulfrumJacket>() && legs.type == ItemType<WulfrumOveralls>();
        public static bool HasArmorSet(Player player) => player.armor[0].type == ItemType<WulfrumHat>() && player.armor[1].type == ItemType<WulfrumJacket>() && player.armor[2].type == ItemType<WulfrumOveralls>();
        public bool IsPartOfSet(Item item) => item.type == ItemType<WulfrumHat>() ||
                item.type == ItemType<WulfrumJacket>() ||
                item.type == ItemType<WulfrumOveralls>();

        public override void UpdateArmorSet(Player player)
        {
            WulfrumArmorPlayer armorPlayer = player.GetModPlayer<WulfrumArmorPlayer>();
            WulfrumTransformationPlayer transformationPlayer = player.GetModPlayer<WulfrumTransformationPlayer>();

            armorPlayer.wulfrumSet = true;

            player.setBonus = "+1 max minion"; //The cooler part of the set bonus happens in modifytooltips because i can't recolor it otherwise. Madge
            player.maxMinions++;
            if (PowerModeEngaged(player, out var cd))
            {
                if (cd.timeLeft == BastionCooldown + BastionTime)
                {
                    ActivationEffects(player);
                }

                //Stats
                player.statDefense += 13;
                player.endurance += 0.05f; //10% Dr in total with the chestplate


                //Can't account for previous fullbody transformations but at this point, whatever.
                Item headItem = player.armor[10].type != 0 ? player.armor[10] : player.armor[0];
                bool hatVisible = !transformationPlayer.transformationActive && headItem.type == ItemType<WulfrumHat>();


                //Spawn the hat
                if (cd.timeLeft == BastionCooldown + BastionTime - (int)(BastionBuildTime * 0.9f) && hatVisible)
                {
                    Particle leftoverHat = new WulfrumHatParticle(player, -Vector2.UnitY.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(3f, 7f), 25);
                    GeneralParticleHandler.SpawnParticle(leftoverHat);
                }


                //Visuals
                if (cd.timeLeft < BastionCooldown + BastionTime - BastionBuildTime)
                    player.GetModPlayer<WulfrumTransformationPlayer>().transformationActive = true;
                else if (cd.timeLeft <= BastionCooldown + BastionTime - (int)(BastionBuildTime * 0.9f))
                    player.GetModPlayer<WulfrumTransformationPlayer>().forceHelmetOn = true;


                //Swapping the arm.
                if (DummyCannon.IsAir)
                    DummyCannon.SetDefaults(ItemType<WulfrumFusionCannon>());

                if (Main.myPlayer == player.whoAmI)
                {
                    //Drop the player's held item if they were holding something before
                    if (!(Main.mouseItem.type == DummyCannon.type) && !Main.mouseItem.IsAir)
                        Main.LocalPlayer.QuickSpawnClonedItem(null, Main.mouseItem, Main.mouseItem.stack);

                    Main.mouseItem = DummyCannon;
                }

                //Slot 58 is the "fake" slot thats used for the item the player is holding in their mouse.
                player.inventory[58] = DummyCannon;
                player.selectedItem = 58;
            }

            else if (Main.myPlayer == player.whoAmI)
            {
                //Clear the player's hand
                if (Main.mouseItem.type == ItemType<WulfrumFusionCannon>())
                    Main.mouseItem = new Item();

                DummyCannon.TurnToAir();
            }
        }

        public void ActivationEffects(Player player)
        {
            SoundEngine.PlaySound(SetActivationSound);

            //Do'nt do the effect ifthe player is already using the wulfrum vanity lol.
            bool transformedAlready = player.GetModPlayer<WulfrumTransformationPlayer>().transformationActive;

            if (!transformedAlready)
            {
                for (int i = 0; i < 5; i++)
                {
                    Particle part = new WulfrumBastionPartsParticle(player, i, BastionBuildTime + 2);
                    GeneralParticleHandler.SpawnParticle(part);
                }
            }

            //Do spawn the cannon always though
            Particle gun = new WulfrumBastionPartsParticle(player, 5, BastionBuildTime + 2);

            if (transformedAlready)
            {
                (gun as WulfrumBastionPartsParticle).TimeOffset = 0;
                (gun as WulfrumBastionPartsParticle).AnimationTime = BastionBuildTime + 2;
            }
            GeneralParticleHandler.SpawnParticle(gun);
        }

        public static void ModifySetTooltips(ModItem item, List<TooltipLine> tooltips)
        {
            if (HasArmorSet(Main.LocalPlayer))
            {
                int setBonusIndex = tooltips.FindIndex(x => x.Name == "SetBonus" && x.Mod == "Terraria");

                if (setBonusIndex != -1)
                {
                    TooltipLine setBonus1 = new TooltipLine(item.Mod, "CalamityMod:SetBonus1", "Wulfrum Bastion - Double tap DOWN while dismounted to equip a heavy wulfrum armor");
                    setBonus1.OverrideColor = Color.Lerp(new Color(194, 255, 67), new Color(112, 244, 244), 0.5f + 0.5f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f));
                    tooltips.Insert(setBonusIndex + 1, setBonus1);

                    TooltipLine setBonus2 = new TooltipLine(item.Mod, "CalamityMod:SetBonus2", "While the power armor is active, you can only use the integrated fusion cannon, but get increased defensive stats");
                    setBonus2.OverrideColor = new Color(110, 192, 93);
                    tooltips.Insert(setBonusIndex + 2, setBonus2);

                    TooltipLine setBonus3 = new TooltipLine(item.Mod, "CalamityMod:SetBonus3", "Calling down the armor consumes one piece of wulfrum scrap, and the armor will lose durability faster when hit");
                    setBonus3.OverrideColor = new Color(110, 192, 93);
                    tooltips.Insert(setBonusIndex + 3, setBonus3);

                    if (!Main.keyState.IsKeyDown(LeftShift))
                    {
                        TooltipLine itemDisplay = new TooltipLine(item.Mod, "CalamityMod:ArmorItemDisplay", "Hold SHIFT to see the stats of the fusion cannon");
                        itemDisplay.OverrideColor = new Color(190, 190, 190);
                        tooltips.Add(itemDisplay);
                    }
                }

            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) => ModifySetTooltips(this, tooltips);

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<SummonDamageClass>() += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WulfrumMetalScrap>(5).
                AddIngredient<EnergyCore>().
                AddTile(TileID.Anvils).
                Register();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    [LegacyName("WulfrumArmor")]
    public class WulfrumJacket : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            DisplayName.SetDefault("Wulfrum Jacket");
            Tooltip.SetDefault("5% increased damage reduction"); //Increases to 10 with the wulfrum bastion active

            if (Main.netMode != NetmodeID.Server)
            {
                var equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
                ArmorIDs.Body.Sets.HidesArms[equipSlot] = true;
                ArmorIDs.Body.Sets.HidesTopSkin[equipSlot] = true;
            }
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) => WulfrumHat.ModifySetTooltips(this, tooltips);

        public override void UpdateEquip(Player player) => player.endurance += 0.05f;

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WulfrumMetalScrap>(12).
                AddIngredient<EnergyCore>().
                AddTile(TileID.Anvils).
                Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    [LegacyName("WulfrumLeggings")]
    public class WulfrumOveralls : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            DisplayName.SetDefault("Wulfrum Overalls");
            Tooltip.SetDefault("Movement speed increased by 5%");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 25, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 1;

            if (Main.netMode != NetmodeID.Server)
            {
                var equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
                ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlot] = true;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) => WulfrumHat.ModifySetTooltips(this, tooltips);
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WulfrumMetalScrap>(8).
                AddIngredient<EnergyCore>().
                AddTile(TileID.Anvils).
                Register();
        }
    }
    #endregion

    public class WulfrumArmorPlayer : ModPlayer
    {
        public static int BastionShootDamage = 10;
        public static float BastionShootSpeed = 18f;
        public static int BastionShootTime = 10;

        public bool wulfrumSet = false;
        
        public override void ResetEffects()
        {
            wulfrumSet = false;
        }

        public override void UpdateDead()
        {
            wulfrumSet = false;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (WulfrumHat.PowerModeEngaged(Player, out var cd) && Main.netMode != NetmodeID.Server)
            {
                SetBonusEndEffect(true);
                if (!Player.GetModPlayer<WulfrumTransformationPlayer>().vanityEquipped)
                    Player.GetModPlayer<WulfrumTransformationPlayer>().transformationActive = false;
            }
        }

        public override void PostUpdate()
        {
            if (wulfrumSet && Player.Calamity().cooldowns.TryGetValue(WulfrumBastion.ID, out var cd) && cd.timeLeft == WulfrumHat.BastionCooldown)
            {
                SetBonusEndEffect(false);
            }
        }

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (WulfrumHat.PowerModeEngaged(Player, out var cd))
            {
                cd.timeLeft -= WulfrumHat.TimeLostPerHit;
                if (cd.timeLeft < WulfrumHat.BastionCooldown)
                {
                    cd.timeLeft = WulfrumHat.BastionCooldown - 1;
                    if (Main.netMode != NetmodeID.Server)
                        SetBonusEndEffect(true);
                }
            }
        }

        public void SetBonusEndEffect(bool violent)
        {
            SoundStyle breakSound = WulfrumHat.SetBreakSoundSafe;
            float goreSpeed = 3f;
            int goreCount = 4;
            int goreIncrement = 2;

            if (violent)
            {
                breakSound = WulfrumHat.SetBreakSound;
                goreSpeed = 9f;
                goreCount = 9;
                goreIncrement = 1;
            }

            SoundEngine.PlaySound(breakSound, Player.Center);
            //Only spawn the cannon gore if the player already has the vanity on.
            if (Player.GetModPlayer<WulfrumTransformationPlayer>().vanityEquipped)
            {
                Vector2 shrapnelVelocity = Main.rand.NextVector2Circular(goreSpeed, goreSpeed);
                Gore.NewGore(Player.GetSource_Death(), Player.Center, shrapnelVelocity, Mod.Find<ModGore>("WulfrumPowerSuit1").Type);
            }

            else
            {
                int j = 1;

                for (int i = 1; i < goreCount; i++)
                {
                    Vector2 shrapnelVelocity = Main.rand.NextVector2Circular(goreSpeed, goreSpeed);
                    string goreType = "WulfrumPowerSuit" + j.ToString();
                    Gore.NewGore(Player.GetSource_Death(), Player.Center, shrapnelVelocity, Mod.Find<ModGore>(goreType).Type);

                    j += Main.rand.Next(1, goreIncrement);
                }
            }
        }


        public override void PostUpdateMiscEffects()
        {
            //This is important. Prevents ppl cheat sheeting the item in from softlocking their mouse button lol
            if (Main.mouseItem.ModItem is WulfrumFusionCannon && !WulfrumHat.PowerModeEngaged(Player, out _))
            {
                Main.mouseItem.TurnToAir();
            }

            //This shouldn't ever be possible since the power mode prevents you from using or moving items around
            if (!wulfrumSet && WulfrumHat.PowerModeEngaged(Player, out var cd))
            {
                cd.timeLeft = WulfrumHat.BastionCooldown;
            }
        }

        public override void FrameEffects()
        {
            //Give the braids variant to w*men
            if (!Player.Male && Player.head == EquipLoader.GetEquipSlot(Mod, "WulfrumHat", EquipType.Head))
                Player.head = EquipLoader.GetEquipSlot(Mod, "WulfrumHatFemale", EquipType.Head);
        }
    }
}