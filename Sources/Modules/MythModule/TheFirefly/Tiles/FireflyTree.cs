
using Terraria.Localization;
using Terraria.ObjectData;
namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class FireflyTree : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileLavaDeath[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileBlockLight[Type] = false;
            Main.tileLighted[Type] = true;
            Main.tileAxe[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 8;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(51, 26, 58));
            DustType = ModContent.DustType<Bosses.CorruptMoth.Dusts.MothBlue2>();
            AdjTiles = new int[] { Type };
            //TODO Hjson
            ModTranslation modTranslation = CreateMapEntryName(null);
            modTranslation.SetDefault("Tree");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "树");
            AddMapEntry(new Color(155, 173, 183), modTranslation);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int Times = Main.rand.Next(5, 9);
            for (int d = 0; d < Times; d++)
            {
                Item.NewItem(null, i * 16 + Main.rand.Next(72), j * 16 + Main.rand.Next(64), 16, 16, ModContent.ItemType<Items.GlowWood>());
            }
            /*for (int f = 0; f < 13; f++)
            {
                Vector2 vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28d);
                Gore.NewGore(null, new Vector2(i * 16, j * 16) + vF, vF, ModContent.Find<ModGore>("MythMod/CyanVineOre" + f.ToString()).Type, 1f);
                vF = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(6.28d);
                Dust.NewDust(new Vector2(i * 16, j * 16) + vF, 0, 0, DustID.Silver, vF.X, vF.Y);
                vF = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(6.28d);
                Dust.NewDust(new Vector2(i * 16, j * 16) + vF, 0, 0, DustID.WoodFurniture, vF.X, vF.Y);
            }*/
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

        private RopeManager ropeManager = new RopeManager();
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            //if (RopPosFir.Count < 1)
            //{
            //    GetRopePos("FireflyTreeRope");
            //    InitMass_Spring();
            //}
            //Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            //if (Main.drawToScreen)
            //{
            //    zero = Vector2.Zero;
            //}
            //Tile tile = Main.tile[i, j];
            //if (tile.TileFrameY != 0 || !tile.HasTile)
            //{
            //    return false;
            //}
            //Point point = new Point(i, j);

            //Texture2D treeTexture = MythContent.QuickTexture("TheFirefly/Tiles/FireflyTree");
            //Texture2D glowTexture = MythContent.QuickTexture("TheFirefly/Tiles/FireflyTreeGlow");
            //Vector2 worldCoord = point.ToWorldCoordinates(8f, 8f);
            //Color color = Lighting.GetColor(i, j);
            //SpriteEffects effects = SpriteEffects.None;
            //const int Count = 16;
            //Vector2 HalfSize = treeTexture.Size() / 2f;
            //HalfSize.X /= Count;
            //spriteBatch.Draw(treeTexture, worldCoord - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, 0, treeTexture.Width / Count, treeTexture.Height), color, 0f, HalfSize, 1f, effects, 0f);
            //spriteBatch.Draw(glowTexture, worldCoord - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, 0, treeTexture.Width / Count, treeTexture.Height), new Color(1f, 1f, 1f, 0), 0f, HalfSize, 1f, effects, 0f);

            //Vector2 RopOffset = new Vector2(i * 16 - tile.TileFrameX - 128, j * 16 - 128) - Main.screenPosition;



            //for (int k = 0; k < RopPosFir.Count; k++)
            //{
            //    if (RopPosFir[k].X > tile.TileFrameX && RopPosFir[k].X <= tile.TileFrameX + 256)
            //    {
            //        foreach (var massJ in masses[k])
            //        {
            //            Vector2 DrawP = massJ.position + RopPosFir[k] + RopOffset + zero;
            //            massJ.force += new Vector2(0.02f + 0.02f * (float)(Math.Sin(Main.timeForVisualEffects / 72f + DrawP.X / 13d + DrawP.Y / 4d)), 0) * (Main.windSpeedCurrent + 1f) * 2f;

            //            //mass.force -= mass.velocity * 0.1f;
            //            //重力加速度（可调
            //            massJ.force += new Vector2(0, 1.0f) * massJ.mass;
            //            Texture2D t0 = MythContent.QuickTexture("TheFirefly/Tiles/Branch");
            //            int FiIdx = masses[k].FindIndex(mass => mass.position == massJ.position);
            //            float Scale = massJ.mass * 2f;
            //            if (FiIdx > 0)
            //            {
            //                Vector2 v0 = massJ.position - masses[k][FiIdx - 1].position;
            //                float Rot = (float)(Math.Atan2(v0.Y, v0.X)) - (float)(Math.PI / 2d);
            //                for (int z = 0; z < FiIdx; z++)
            //                {
            //                    spriteBatch.Draw(t0, DrawP, null, color, Rot, t0.Size() / 2f, Scale, SpriteEffects.None, 0);
            //                    spriteBatch.Draw(t0, DrawP, null, new Color(255, 255, 255, 0), Rot, t0.Size() / 2f, Scale, SpriteEffects.None, 0);
            //                }
            //            }
            //        }
            //        masses[k][0].position = Vector2.Zero;
            //        float deltaTime = 1;
            //        foreach (var spring in springs[k])
            //        {
            //            spring.ApplyForce(deltaTime);
            //        }
            //        List<Vector2> massPositions = new List<Vector2>();
            //        foreach (var massJ in masses[k])
            //        {
            //            massJ.Update(deltaTime);
            //            massPositions.Add(massJ.position);
            //        }
            //        List<Vector2> massPositionsSmooth = new List<Vector2>();
            //        massPositionsSmooth = CatmullRom.SmoothPath(massPositions);
            //        if (massPositionsSmooth.Count > 0)
            //        {
            //            //DrawRope(massPositionsSmooth, RopPosFir[k] + RopOffset, Vertices);
            //        }
            //    }
            //}


            return false;
        }
    }
}
