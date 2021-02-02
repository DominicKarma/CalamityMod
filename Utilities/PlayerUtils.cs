using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.CalPlayer;
using System.Linq;
using Terraria;
using Terraria.ID;

using static Terraria.ModLoader.ModContent;

namespace CalamityMod
{
	public static partial class CalamityUtils
	{
		// These functions factor in TML 0.11 allDamage to get the player's total damage boost which affects the specified class.
		public static float MeleeDamage(this Player player) => player.allDamage + player.meleeDamage - 1f;
		public static float RangedDamage(this Player player) => player.allDamage + player.rangedDamage - 1f;
		public static float MagicDamage(this Player player) => player.allDamage + player.magicDamage - 1f;
		public static float MinionDamage(this Player player) => player.allDamage + player.minionDamage - 1f;
		public static float ThrownDamage(this Player player) => player.allDamage + player.thrownDamage - 1f;
		public static float RogueDamage(this Player player) => player.allDamage + player.thrownDamage + player.Calamity().throwingDamage - 2f;
		public static float AverageDamage(this Player player) => player.allDamage + (player.meleeDamage + player.rangedDamage + player.magicDamage + player.minionDamage + player.Calamity().throwingDamage - 5f) / 5f;

		public static bool IsUnderwater(this Player player) => Collision.DrownCollision(player.position, player.width, player.height, player.gravDir);
		public static bool StandingStill(this Player player, float velocity = 0.05f) => player.velocity.Length() < velocity;
		public static bool InSpace(this Player player)
		{
			float x = Main.maxTilesX / 4200f;
			x *= x;
			float spaceGravityMult = (float)((player.position.Y / 16f - (60f + 10f * x)) / (Main.worldSurface / 6.0));
			return spaceGravityMult < 1f;
		}
		public static bool PillarZone(this Player player) => player.ZoneTowerStardust || player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula;
		public static bool InCalamity(this Player player) => player.Calamity().ZoneCalamity;
		public static bool InSunkenSea(this Player player) => player.Calamity().ZoneSunkenSea;
		public static bool InSulphur(this Player player) => player.Calamity().ZoneSulphur;
		public static bool InAstral(this Player player, int biome = 0) //1 is above ground, 2 is underground, 3 is desert
		{
			switch (biome)
			{
				case 1:
					return player.Calamity().ZoneAstral && (player.ZoneOverworldHeight || player.ZoneSkyHeight);

				case 2:
					return player.Calamity().ZoneAstral && (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight);

				case 3:
					return player.Calamity().ZoneAstral && player.ZoneDesert;

				default:
					return player.Calamity().ZoneAstral;
			}
		}
		public static bool InAbyss(this Player player, int layer = 0)
		{
			switch (layer)
			{
				case 1:
					return player.Calamity().ZoneAbyssLayer1;

				case 2:
					return player.Calamity().ZoneAbyssLayer2;

				case 3:
					return player.Calamity().ZoneAbyssLayer3;

				case 4:
					return player.Calamity().ZoneAbyssLayer4;

				default:
					return player.Calamity().ZoneAbyss;
			}
		}
		public static bool InventoryHas(this Player player, params int[] items)
		{
			return player.inventory.Any(item => items.Contains(item.type));
		}
		public static bool PortableStorageHas(this Player player, params int[] items)
		{
			bool hasItem = false;
			if (player.bank.item.Any(item => items.Contains(item.type)))
				hasItem = true;
			if (player.bank2.item.Any(item => items.Contains(item.type)))
				hasItem = true;
			if (player.bank3.item.Any(item => items.Contains(item.type)))
				hasItem = true;
			return hasItem;
		}

		/// <summary>
		/// Returns the damage multiplier Adrenaline Mode provides for the given player.
		/// </summary>
		/// <param name="mp"></param>
		/// <returns></returns>
		public static double GetAdrenalineDamage(this CalamityPlayer mp)
		{
			double adrenalineBoost = CalamityPlayer.AdrenalineDamageBoost;
			if (mp.adrenalineBoostOne)
				adrenalineBoost += CalamityPlayer.AdrenalineDamagePerBooster;
			if (mp.adrenalineBoostTwo)
				adrenalineBoost += CalamityPlayer.AdrenalineDamagePerBooster;
			if (mp.adrenalineBoostThree)
				adrenalineBoost += CalamityPlayer.AdrenalineDamagePerBooster;

			return adrenalineBoost;
		}

		/// <summary>
		/// Applies Rage and Adrenaline to the given damage multiplier. The values controlling the so-called "Rippers" can be found in CalamityPlayer.
		/// </summary>
		/// <param name="mp">The CalamityPlayer who may or may not be using Rage or Adrenaline.</param>
		/// <param name="damageMult">A reference to the current in-use damage multiplier. This will be increased in-place.</param>
		public static void ApplyRippersToDamage(CalamityPlayer mp, ref double damageMult)
		{
			// Rage and Adrenaline now stack additively with no special cases.
			if (mp.rageModeActive)
				damageMult += mp.RageDamageBoost;
			if (mp.adrenalineModeActive)
				damageMult += mp.GetAdrenalineDamage();
		}

		/// <summary>
		/// Inflict typical exo weapon debuffs in pvp.
		/// </summary>
		/// <param name="target">The Player attacked.</param>
		/// <param name="multiplier">Debuff time multiplier if needed.</param>
		/// <returns>Inflicts debuffs if the target isn't immune.</returns>
		public static void ExoDebuffs(this Player target, float multiplier = 1f)
		{
			target.AddBuff(BuffType<ExoFreeze>(), (int)(30 * multiplier));
			target.AddBuff(BuffType<BrimstoneFlames>(), (int)(120 * multiplier));
			target.AddBuff(BuffType<GlacialState>(), (int)(120 * multiplier));
			target.AddBuff(BuffType<Plague>(), (int)(120 * multiplier));
			target.AddBuff(BuffType<HolyFlames>(), (int)(120 * multiplier));
			target.AddBuff(BuffID.CursedInferno, (int)(120 * multiplier));
			target.AddBuff(BuffID.Frostburn, (int)(120 * multiplier));
			target.AddBuff(BuffID.OnFire, (int)(120 * multiplier));
			target.AddBuff(BuffID.Ichor, (int)(120 * multiplier));
		}
	}
}
