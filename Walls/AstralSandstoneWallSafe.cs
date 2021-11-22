using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Walls
{
    public class AstralSandstoneWallSafe : ModWall
    {

        public override void SetDefaults()
        {
            // TODO -- Change this dust to be one more befitting Astral Sandstone.
            dustType = DustID.Shadowflame;
            drop = ModContent.ItemType<Items.Placeables.Walls.AstralSandstoneWall>();
            Main.wallHouse[Type] = true;

            WallID.Sets.Conversion.Sandstone[Type] = true;

            AddMapEntry(new Color(29, 38, 49));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}
