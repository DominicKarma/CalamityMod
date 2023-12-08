﻿using System;
using System.Collections.Generic;
using CalamityMod.Enums;
using CalamityMod.Events;
using Terraria.ModLoader;

namespace CalamityMod.Systems
{
    public class BossRushDialogueSystem : ModSystem
    {
        public static BossRushDialoguePhase Phase = BossRushDialoguePhase.None;
        private static BossRushDialogueEvent[] currentSequence = null;
        private static int currentSequenceIndex = 0;

        public static int CurrentDialogueDelay = 0;

        internal struct BossRushDialogueEvent
        {
            private const int DefaultFrameDelay = 180;

            internal int FrameDelay;
            internal string LocalizationKey;
            internal Func<bool> skipCondition;

            public BossRushDialogueEvent() { }
            public BossRushDialogueEvent(string key)
            {
                LocalizationKey = key;
                FrameDelay = DefaultFrameDelay;
                skipCondition = null;
            }
            public BossRushDialogueEvent(string key, int delay = DefaultFrameDelay, Func<bool> skipFunc = null)
            {
                LocalizationKey = key;
                FrameDelay = delay;
                skipCondition = skipFunc;
            }

            public readonly bool ShouldDisplay()
            {
                if (skipCondition is null)
                    return true;
                return !skipCondition.Invoke();
            }
        }

        internal static Dictionary<BossRushDialoguePhase, BossRushDialogueEvent[]> BossRushDialogue;

        public override void Load()
        {
            // Dialogue that occurs the first time you start Boss Rush
            BossRushDialogueEvent[] startDialogues = new BossRushDialogueEvent[]
            {
                new("Mods.CalamityMod.Events.BossRushStartText_1"),
                new("Mods.CalamityMod.Events.BossRushStartText_2"),
                new("Mods.CalamityMod.Events.BossRushStartText_3"),
                new("Mods.CalamityMod.Events.BossRushStartText_4"),
                new("Mods.CalamityMod.Events.BossRushStartText_5"),
                new("Mods.CalamityMod.Events.BossRushStartText_6"),
                new("Mods.CalamityMod.Events.BossRushStartText_7"),
                new("Mods.CalamityMod.Events.BossRushStartText_8"),
                new("Mods.CalamityMod.Events.BossRushStartText_9"),
                new("Mods.CalamityMod.Events.BossRushStartText_DoG", skipFunc: () => !DownedBossSystem.downedDoG),
                new("Mods.CalamityMod.Events.BossRushStartText_Yharon", skipFunc: () => !DownedBossSystem.downedYharon),
                new("Mods.CalamityMod.Events.BossRushStartText_DraedonSCal", skipFunc: () => !DownedBossSystem.downedExoMechs || !DownedBossSystem.downedCalamitas),
                new("Mods.CalamityMod.Events.BossRushStartText_10"),
                new("Mods.CalamityMod.Events.BossRushStartText_11"),
                new("Mods.CalamityMod.Events.BossRushStartText_12"),
            };

            // Dialogue that occurs when starting Boss Rush on repeat attempts
            BossRushDialogueEvent[] startDialoguesShort = new BossRushDialogueEvent[]
            {
                new("Mods.CalamityMod.Events.BossRushStartText_Repeat"),
            };

            // Dialogue that occurs when beating Tier 1 of Boss Rush
            BossRushDialogueEvent[] tierOneDialogues = new BossRushDialogueEvent[]
            {
                new("Mods.CalamityMod.Events.BossRushTierOneEndText_1"),
                new("Mods.CalamityMod.Events.BossRushTierOneEndText_2"),
            };

            // Dialogue that occurs when beating Tier 2 of Boss Rush
            BossRushDialogueEvent[] tierTwoDialogues = new BossRushDialogueEvent[]
            {
                new("Mods.CalamityMod.Events.BossRushTierTwoEndText_1"),
                new("Mods.CalamityMod.Events.BossRushTierTwoEndText_2"),
            };

            // Dialogue that occurs when beating Tier 3 of Boss Rush
            BossRushDialogueEvent[] tierThreeDialogues = new BossRushDialogueEvent[]
            {
                new("Mods.CalamityMod.Events.BossRushTierThreeEndText_1"),
                new("Mods.CalamityMod.Events.BossRushTierThreeEndText_2"),
            };

            // Dialogue that occurs when beating Tier 4 of Boss Rush
            BossRushDialogueEvent[] tierFourDialogues = new BossRushDialogueEvent[]
            {
                new("Mods.CalamityMod.Events.BossRushTierFourEndText_1"),
                new("Mods.CalamityMod.Events.BossRushTierFourEndText_2"),
                new("Mods.CalamityMod.Events.BossRushTierFourEndText_3"),
            };

            // Dialogue that occurs the first time you beat Boss Rush
            BossRushDialogueEvent[] endDialogues = new BossRushDialogueEvent[]
            {
                new("Mods.CalamityMod.Events.BossRushEndText_1"),
                new("Mods.CalamityMod.Events.BossRushEndText_2"),
                new("Mods.CalamityMod.Events.BossRushEndText_3"),
                new("Mods.CalamityMod.Events.BossRushEndText_4"),
                new("Mods.CalamityMod.Events.BossRushEndText_5"),
                new("Mods.CalamityMod.Events.BossRushEndText_6"),
                new("Mods.CalamityMod.Events.BossRushEndText_7"),
                new("Mods.CalamityMod.Events.BossRushEndText_8"),
            };

            // Dialogue that occurs the first time you beat Boss Rush
            BossRushDialogueEvent[] endDialoguesShort = new BossRushDialogueEvent[]
            {
                new("Mods.CalamityMod.Events.BossRushEndText_Repeat"),
            };

            BossRushDialogue = new Dictionary<BossRushDialoguePhase, BossRushDialogueEvent[]>()
            {
                { BossRushDialoguePhase.Start, startDialogues },
                { BossRushDialoguePhase.StartRepeat, startDialoguesShort },
                { BossRushDialoguePhase.TierOneComplete, tierOneDialogues },
                { BossRushDialoguePhase.TierTwoComplete, tierTwoDialogues },
                { BossRushDialoguePhase.TierThreeComplete, tierThreeDialogues },
                { BossRushDialoguePhase.TierFourComplete, tierFourDialogues },
                { BossRushDialoguePhase.End, endDialogues },
                { BossRushDialoguePhase.EndRepeat, endDialoguesShort },
            };
        }

        public override void Unload()
        {
            BossRushDialogue = null;
        }

        public static void StartDialogue(BossRushDialoguePhase phaseToRun)
        {
            Phase = phaseToRun;
            bool validDialogueFound = BossRushDialogue.TryGetValue(Phase, out var dialogueListToUse);
            if (validDialogueFound)
            {
                currentSequence = dialogueListToUse;
                currentSequenceIndex = 0;
            }

            CurrentDialogueDelay = 4;
        }

        internal static void Tick()
        {
            // If the phase isn't defined properly, don't do anything.
            if (Phase == BossRushDialoguePhase.None)
                return;
            
            if (currentSequenceIndex < currentSequence.Length)
            {
                // If it's time to display dialogue, do so.
                if (CurrentDialogueDelay == 0 && currentSequenceIndex < currentSequence.Length)
                {
                    BossRushDialogueEvent line = currentSequence[currentSequenceIndex];
                    CalamityUtils.DisplayLocalizedText(line.LocalizationKey, BossRushEvent.XerocTextColor);
                    CurrentDialogueDelay = line.FrameDelay;

                    // Move onto the next dialogue line.
                    ++currentSequenceIndex;
                }
                // Otherwise, decrement the existing delay.
                else
                    --CurrentDialogueDelay;

                // Ensure a boss does not attack the player while they are reading dialogue.
                // Indefinitely stall the countdown.
                if (BossRushEvent.BossRushSpawnCountdown < 180)
                    BossRushEvent.BossRushSpawnCountdown = CurrentDialogueDelay + 180;
            }

            // If the end of a sequence has been reached, stay in this state indefinitely.
            // Allow the boss spawn countdown to hit zero and the next boss to appear without showing any dialogue or causing any delays.

            // However, if Boss Rush is not occurring, reset all variables.
            if (!BossRushEvent.BossRushActive)
            {
                Phase = BossRushDialoguePhase.None;
                currentSequence = null;
                currentSequenceIndex = 0;
                CurrentDialogueDelay = 0;
            }
        }
    }
}
