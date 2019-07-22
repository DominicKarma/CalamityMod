using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Capture;

namespace CalamityMod.Walls
{
	public class AstralMonolithWall : ModWall
    {
        public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = mod.DustType("Sparkle");
			drop = mod.ItemType("AstralMonolithWall");
			AddMapEntry(new Color(5, 5, 5));
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, mod.DustType("AstralBasic"), 0f, 0f, 1, new Color(255, 255, 255), 1f);
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D sprite = mod.GetTexture("Walls/AstralMonolithWall");
            Color lightColor = Lighting.GetColor(i, j);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            zero -= new Vector2(8, 8);
            Vector2 drawOffset = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + zero;
            int[] sheetOffset = CreatePattern(i, j);
            spriteBatch.Draw
                (
                    sprite,
                    drawOffset,
                    new Rectangle(sheetOffset[0] + Main.tile[i, j].wallFrameX(), sheetOffset[1] + Main.tile[i, j].wallFrameY(), 32, 32),
                    lightColor,
                    0,
                    new Vector2(0f, 0f),
                    1,
                    SpriteEffects.None,
                    0f
                );
            return false;
        }

        private int[] CreatePattern(int i, int j)
        {
            int[] sheetOffset = new int[2] { i % 4, j % 4 };
            sheetOffset[0] = sheetOffset[0] * 468;
            sheetOffset[1] = sheetOffset[1] * 180;
            return (sheetOffset);
        }
    }
}