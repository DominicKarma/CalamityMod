﻿using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Tiles.Astral
{
    public class AstralTallPlants : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileCut[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;

            DustType = ModContent.DustType<AstralBasic>();

            HitSound = SoundID.Grass;

            AddMapEntry(new Color(127, 111, 144));

            base.SetStaticDefaults();
        }

		public override void DropCritterChance(int i, int j, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance)
		{
			if (NPC.CountNPCS(NPCID.EnchantedNightcrawler) < 5 && Main.rand.NextBool(200))
			{
				int worm = NPC.NewNPC(new EntitySource_TileBreak(i, j), i * 16 + 10, j * 16, NPCID.EnchantedNightcrawler);
				Main.npc[worm].TargetClosest();
				Main.npc[worm].velocity.Y = Main.rand.NextFloat(-5f, -2.1f);
				Main.npc[worm].velocity.X = Main.rand.NextFloat(0f, 2.6f) * (float)(-Main.npc[worm].direction);
				Main.npc[worm].direction *= -1;
				Main.npc[worm].netUpdate = true;
			}
		}
    }
}
