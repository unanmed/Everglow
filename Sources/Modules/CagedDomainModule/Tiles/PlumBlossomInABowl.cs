using Terraria.ObjectData;
namespace Everglow.Sources.Modules.CagedDomainModule.Tiles
{
    public class PlumBlossomInABowl : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 10;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16,
                16,
                16,
                16,
                16,
                16,
                16
            };
            TileObjectData.newTile.CoordinateWidth = 144;
            TileObjectData.addTile(Type);
            DustType = 1;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(90, 90, 90), modTranslation);
            HitSound = SoundID.DD2_SkeletonHurt;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.PlumBlossomInABowl>());
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            Main.tile[i, j].TileFrameX = (short)(item.placeStyle * 144);
        }
    }
}
