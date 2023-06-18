using Everglow.Commons.Enums;
using Everglow.TwilightForest.Common;
using Terraria.Localization;

namespace Everglow.TwilightForest.Tiles;

public class TwilightTree : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = false;
		Main.tileLavaDeath[Type] = false;
		Main.tileFrameImportant[Type] = true;
		Main.tileBlockLight[Type] = false;
		Main.tileLighted[Type] = true;
		Main.tileAxe[Type] = true;
		Main.tileNoAttach[Type] = false;

		TileID.Sets.IsATreeTrunk[Type] = true;
		var modTranslation = Language.GetText("Mods.Everglow.MapEntry.TwilightTree");
		AddMapEntry(new Color(58, 53, 50), modTranslation);
		DustType = ModContent.DustType<Dusts.TwilightTreeDust>();
		AdjTiles = new int[] { Type };

		Ins.HookManager.AddHook(CodeLayer.PostDrawTiles, DrawRopes);
	}
	private RopeManager ropeManager = new();
	private List<Rope>[] ropes = new List<Rope>[16];
	private Vector2[] basePositions = new Vector2[16];
	private Dictionary<(int x, int y), (int style, List<Rope> ropes)> hasRope = new();

	/// <summary>
	/// 记录当前每个有树枝的节点的绳子Style（即TileFrameX / 256）
	/// </summary>
	public List<(int x, int y, int style)> GetRopeStyleList()
	{
		var result = new List<(int x, int y, int style)>();
		foreach (var keyvaluepair in hasRope)
		{
			result.Add((keyvaluepair.Key.x, keyvaluepair.Key.y, keyvaluepair.Value.style));
		}
		return result;
	}

	public void InitTreeRopes(List<(int x, int y, int style)> ropesData)
	{
		hasRope.Clear();
		ropeManager.Clear();
		for (int i = 0; i < ropes.Length; i++)
		{
			ropes[i] = null;
		}

		foreach (var (x, y, style) in ropesData)
		{
			InsertOneTreeRope(x, y, style);
		}
	}

	public void InsertOneTreeRope(int xTS, int yTS, int style)
	{
		Texture2D treeTexture = ModAsset.TwilightTree.Value;


		var point = new Point(xTS, yTS);
		Vector2 tileCenterWS = point.ToWorldCoordinates(0, 0);

		if (ropes[style] is null)
		{
			if (style != 2)
			{
				var HalfSize = new Vector2(-5, 24);
				ropes[style] = ropeManager.LoadRope(new Rectangle(0, 0, 4, 2), tileCenterWS + HalfSize, () => Vector2.Zero);
				hasRope.Add((xTS, yTS), (style, ropes[style]));
			}
			else
			{
				var HalfSize = new Vector2(-60, -60);
				ropes[style] = ropeManager.LoadRope(new Rectangle(0, 0, 30, 6), tileCenterWS + HalfSize, () => Vector2.Zero);
				hasRope.Add((xTS, yTS), (style, ropes[style]));
			}
			basePositions[style] = tileCenterWS;
		}
		else if (!hasRope.ContainsKey((xTS, yTS)))
		{
			Vector2 deltaPosition = tileCenterWS - basePositions[style];
			var rs = ropes[style].Select(r => r.Clone(deltaPosition)).ToList();
			ropeManager.LoadRope(rs);
			hasRope.Add((xTS, yTS), (style, rs));
		}
	}

	public void DrawRopes()
	{
		if (!Main.gamePaused)
		{
			ropeManager.drawColor = new Color(0, 0, 0, 0);
			ropeManager.Update(1f);
		}
		ropeManager.Draw();
	}
	public override IEnumerable<Item> GetItemDrops(int i, int j)
	{
		yield return new Item(ModContent.ItemType<Items.TwilightWood>());
	}
	public override bool CanDrop(int i, int j)
	{
		for (int x = 0; x < 6; x++)
		{
			Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustType, 0, 0, 0, default, Main.rand.NextFloat(0.5f, 1f));
		}
		var tile = Main.tile[i, j];
		if (tile.TileFrameY == 2)
		{
			for (int x = 0; x < 12; x++)
			{
				Gore.NewGore(null, new Vector2(i * 16 - 32, j * 16 - 120) + new Vector2(Main.rand.Next(80), Main.rand.Next(120)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<TwilightTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
			}
			for (int x = 0; x < 64; x++)
			{
				Dust.NewDust(new Vector2(i * 16 - 40, j * 16 - 100), 96, 96, DustType, 0, 0, 0, default, Main.rand.NextFloat(0.5f, 1f));
			}
		}
		if (tile.TileFrameY > 3)
			Gore.NewGore(null, new Vector2(i * 16, j * 16) + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<TwilightTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));

		if (!hasRope.ContainsKey((i, j)))
		{
			Ins.Logger.Warn("Drop: Trying to access an non-existent TwilightTree rope" + (i, j).ToString());
			return false;
		}

		var ropes = hasRope[(i, j)].ropes;
		foreach (var r in ropes)
		{
			var acc = new Vector2(Main.rand.NextFloat(-1, 1), 0);
			foreach (var m in r.mass)
			{
				m.force += acc;
				if (Main.rand.NextBool(10))
					Gore.NewGoreDirect(null, m.position, m.velocity * 0.1f, ModContent.GoreType<TwilightTree_Vine>());
				//被砍时对mass操纵写这里
			}
		}
		ropeManager.RemoveRope(hasRope[(i, j)].ropes);
		hasRope.Remove((i, j));
		return false;
	}
	private void Shake(int i, int j, int frameY)
	{
		if (Main.rand.NextBool(7))
		{
			if (frameY == 2)
			{
				for (int x = 0; x < 12; x++)
				{
					Gore.NewGore(null, new Vector2(i * 16 - 32, j * 16 - 120) + new Vector2(Main.rand.Next(80), Main.rand.Next(120)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<TwilightTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
				}
			}
			if (frameY > 3)
				Gore.NewGore(null, new Vector2(i * 16, j * 16) + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<TwilightTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
		}

		if (!hasRope.ContainsKey((i, j)))
		{
			Ins.Logger.Warn("Shake: Trying to access an non-existent TwilightTree rope" + (i, j).ToString());
			return;
		}

		var ropes = hasRope[(i, j)].ropes;
		foreach (var r in ropes)
		{
			var acc = new Vector2(Main.rand.NextFloat(-1, 1), 0);
			foreach (var m in r.mass)
			{
				m.force += acc;
				if (Main.rand.NextBool(100))
					Gore.NewGoreDirect(null, m.position, m.velocity * 0.1f, ModContent.GoreType<TwilightTree_Vine>());
				//被砍时对mass操纵写这里
			}
		}
	}
	// TODO 144
	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		//int Dy = -1;//向上破坏的自变化Y坐标
		//if (!fail)//判定为已破碎
		//{
		//	//以下是破坏的特效,比如落叶
		//	if (Main.tile[i, j].TileFrameY < 4)
		//	{
		//		Tile tileLeft;
		//		Tile tileRight;
		//		tileLeft = Main.tile[i - 1, j];
		//		if (tileLeft.TileType == Type)
		//		{
		//			Shake(i - 1, j, tileLeft.TileFrameY);
		//			Drop(i - 1, j);
		//		}
		//		tileRight = Main.tile[i + 1, j];
		//		if (tileRight.TileType == Type)
		//		{
		//			Shake(i + 1, j, tileRight.TileFrameY);
		//			Drop(i + 1, j);
		//		}
		//		while (Main.tile[i, j + Dy].HasTile && Main.tile[i, j + Dy].TileType == Type && Dy > -100)
		//		{
		//			Shake(i, j + Dy, Main.tile[i, j + Dy].TileFrameY);
		//			Drop(i, j + Dy);

		//			tileLeft = Main.tile[i - 1, j + Dy];
		//			tileRight = Main.tile[i + 1, j + Dy];
		//			if (tileLeft.TileType == Type)
		//			{
		//				if (tileLeft.TileFrameY == 2)
		//					break;
		//				Shake(i - 1, j + Dy, tileLeft.TileFrameY);
		//				Drop(i - 1, j + Dy);
		//			}
		//			if (tileRight.TileType == Type)
		//			{
		//				if (tileRight.TileFrameY == 2)
		//					break;
		//				Shake(i + 1, j + Dy, tileRight.TileFrameY);
		//				Drop(i + 1, j + Dy);
		//			}
		//			Dy -= 1;
		//		}


		//		Dy = -1;//向上破坏的自变化Y坐标
		//		tileLeft = Main.tile[i - 1, j];
		//		if (tileLeft.TileType == Type)
		//			tileLeft.HasTile = false;
		//		tileRight = Main.tile[i + 1, j];
		//		if (tileRight.TileType == Type)
		//			tileRight.HasTile = false;
		//		while (Main.tile[i, j + Dy].TileType == Type && Dy > -100)
		//		{
		//			Tile baseTile = Main.tile[i, j + Dy];

		//			baseTile.HasTile = false;

		//			tileLeft = Main.tile[i - 1, j + Dy];
		//			tileRight = Main.tile[i + 1, j + Dy];
		//			if (tileLeft.TileType == Type)
		//				tileLeft.HasTile = false;
		//			if (tileRight.TileType == Type)
		//				tileRight.HasTile = false;
		//			Dy -= 1;
		//		}
		//	}
		//	//清除吊挂的藤条

		//	if (!hasRope.ContainsKey((i, j)))
		//	{
		//		Ins.Logger.Warn("KillTile: Trying to access an non-existent TwilightTree rope" + (i, j).ToString());
		//		return;
		//	}

		//	var ropes = hasRope[(i, j)].ropes;
		//	foreach (var r in ropes)
		//	{
		//		var acc = new Vector2(Main.rand.NextFloat(-1, 1), 0);
		//		foreach (var m in r.mass)
		//		{
		//			m.force += acc;
		//			if (Main.rand.NextBool(10))
		//				Gore.NewGoreDirect(null, m.position, m.velocity * 0.1f, ModContent.GoreType<TwilightTree_Vine>());
		//			//被砍时对mass操纵写这里
		//		}
		//	}
		//	ropeManager.RemoveRope(hasRope[(i, j)].ropes);
		//	hasRope.Remove((i, j));
		//}
		//else
		//{
		//	Tile tileLeft;
		//	Tile tileRight;
		//	while (Main.tile[i, j + Dy].HasTile && Main.tile[i, j + Dy].TileType == Type && Dy > -100)
		//	{
		//		Shake(i, j + Dy, Main.tile[i, j + Dy].TileFrameY);
		//		tileLeft = Main.tile[i - 1, j + Dy];
		//		tileRight = Main.tile[i + 1, j + Dy];
		//		if (tileLeft.TileType == Type)
		//		{
		//			if (tileLeft.TileFrameY == 2)
		//				break;
		//			Shake(i - 1, j + Dy, tileLeft.TileFrameY);
		//		}
		//		if (tileRight.TileType == Type)
		//		{
		//			if (tileRight.TileFrameY == 2)
		//				break;
		//			Shake(i + 1, j + Dy, tileRight.TileFrameY);
		//		}
		//		Dy -= 1;
		//	}
		//}
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{

		Texture2D treeTexture = ModAsset.TwilightTree.Value;
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Tile tile = Main.tile[i, j];
		int Width;
		int Height = 16;
		int TexCoordY;
		int OffsetY = 20;
		int OffsetX = 8;
		float Rot = 0;
		Color color = Lighting.GetColor(i, j);
		switch (tile.TileFrameY)
		{
			default://别的表示空,最好写-1
				return false;
			case 0:  //树桩
				Width = 106;
				Height = 32;
				TexCoordY = 236;
				break;
			case 1:  //左树干
				Width = 28;
				TexCoordY = 164;
				OffsetX += -12;
				break;
			case 2:  //右树干
				Width = 28;
				TexCoordY = 182;
				OffsetX += -2;
				break;
			case 3:  //树冠
				Width = 162;
				Height = 138;
				TexCoordY = 0;
				float Wind = Main.windSpeedCurrent / 15f;
				Rot = Wind + (float)Math.Sin(j + Main.timeForVisualEffects / 30f) * Wind * 0.3f;
				OffsetY = 24;
				//对挂条的生成
				if (!hasRope.ContainsKey((i, j)))
					InsertOneTreeRope(i, j, 2);
				break;
		}
		var origin = new Vector2(Width / 2f, Height);
		spriteBatch.Draw(treeTexture, new Vector2(i * 16 + OffsetX + 8, j * 16 + OffsetY) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX * Width, TexCoordY, Width, Height), color, Rot, origin, 1, SpriteEffects.None, 0);
		spriteBatch.Draw(treeTexture, new Vector2(i * 16 + OffsetX + 8, j * 16 + OffsetY) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX * Width, TexCoordY + 274, Width, Height), new Color(1f, 1f, 1f, 0), Rot, origin, 1, SpriteEffects.None, 0);



		if (tile.TileFrameY >= 3)
		{
			var point = new Point(i, j);
			Vector2 tileCenterWS = point.ToWorldCoordinates(8f, 8f);
			if (tileCenterWS.Distance(Main.LocalPlayer.position) < 200)
			{
				var playerRect = Main.LocalPlayer.Hitbox;
				var (_, ropes) = hasRope[(i, j)];
				foreach (var rope in ropes)
				{
					foreach (var m in rope.mass)
					{
						if (playerRect.Contains(m.position.ToPoint()))
							m.force += Main.LocalPlayer.velocity / 1.5f;
					}
				}
			}
		}
		return false;
	}
}