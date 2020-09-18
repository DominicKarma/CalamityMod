using CalamityMod.Items.Materials;
using CalamityMod.TileEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalamityMod.Tiles.DraedonStructures
{
    public class DraedonLabTurret : ModTile
    {
        public const int Width = 3;
        public const int Height = 2;
        public const int OriginOffsetX = 1;
        public const int OriginOffsetY = 1;
        public const int SheetSquare = 18;

        public override void SetDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = false;

            // No need to set width, height, origin, etc. here, Style3x2 is exactly what we want.
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.LavaDeath = false;

            // When this tile is placed, it places the Draedon Lab Turret tile entity.
            ModTileEntity te = ModContent.GetInstance<TEDraedonLabTurret>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(te.Hook_AfterPlacement, -1, 0, true);

            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Lab Turret");
            AddMapEntry(new Color(67, 72, 81), name);
            soundType = SoundID.Item;
            soundStyle = 14;

            // Requires a Gold Pickaxe or better to mine. Has 500% durability.
            mineResist = 5.00f;
            minPick = 55;
        }

        public override bool CanExplode(int i, int j) => false;

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, 226);
            return false;
        }

        public override bool HasSmartInteract() => true;

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            // TODO -- Turrets have no items and can't be picked up and placed by players.
            // Instead, drop some raw Draedon materials.
            Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<DubiousPlating>(), 8);
            Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<MysteriousCircuitry>(), 8);

            Tile t = Main.tile[i, j];
            int left = i - t.frameX % (Width * SheetSquare) / SheetSquare;
            int top = j - t.frameY % (Height * SheetSquare) / SheetSquare;

            TEDraedonLabTurret te = CalamityUtils.FindTileEntity<TEDraedonLabTurret>(i, j, Width, Height, SheetSquare);
            te?.Kill(left, top);
        }

        // The turret tile draws a pulse turret on top of itself.
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile t = Main.tile[i, j];
            if (t.frameX != 36 || t.frameY != 0)
                return;

            // This is done so that the turret has priority over, say, trees when drawing.
            // TODO -- this is foul and should never happen ever, is there any way around this
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone);

            TEDraedonLabTurret te = CalamityUtils.FindTileEntity<TEDraedonLabTurret>(i, j, Width, Height, SheetSquare);
            if (te is null)
                return;
            int drawDirection = te.Direction;
            Color drawColor = Lighting.GetColor(i, j);

            Texture2D tex = ModContent.GetTexture("CalamityMod/Projectiles/DraedonsArsenal/PulseTurret");
            Vector2 screenOffset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawOffset = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + screenOffset;
            drawOffset.Y -= 2f;
            drawOffset.X += drawDirection == -1 ? -10f : 2f;

            SpriteEffects sfx = drawDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None;
            spriteBatch.Draw(tex, drawOffset, null, drawColor, te.Angle, tex.Size() * 0.5f, 1f, sfx, 0.0f);
        }
    }
}
