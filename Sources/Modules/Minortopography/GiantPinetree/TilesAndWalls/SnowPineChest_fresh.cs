using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;
using Everglow.Commons.Utilities;
namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls;


public class SnowPineChest_fresh : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileSpelunker[Type] = true;
		Main.tileContainer[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileOreFinderPriority[Type] = 500;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.BasicChest[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;

		DustType = DustID.BorealWood;
		AdjTiles = new int[] { TileID.Containers };
		ItemDrop = ModContent.ItemType<Items.SnowPineChest_fresh>();

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
		TileObjectData.newTile.Origin = new Point16(0, 1);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
		TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
		TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
		TileObjectData.newTile.AnchorInvalidTiles = new int[] { TileID.MagicalIceBlock };
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
		TileObjectData.addTile(Type);
	}

	public override ushort GetMapOption(int i, int j)
	{
		return (ushort)(Main.tile[i, j].TileFrameX / 36);
	}

	public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
	{
		return true;
	}

	public static string MapChestName(string name, int i, int j)
	{
		int left = i;
		int top = j;
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX % 36 != 0)
			left--;

		if (tile.TileFrameY != 0)
			top--;

		int chest = Chest.FindChest(left, top);
		if (chest < 0)
			return Language.GetTextValue("LegacyChestType.0");

		if (Main.chest[chest].name == "")
			return name;

		return name + ": " + Main.chest[chest].name;
	}

	public override void KillMultiTile(int i, int j, int frameX, int frameY)
	{
		Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ItemDrop);
		Chest.DestroyChest(i, j);
	}

	public override bool RightClick(int i, int j)
	{
		return FurnitureUtils.ChestRightClick(i, j);
	}
}