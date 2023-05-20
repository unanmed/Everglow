using Everglow.Myth.TheFirefly.Dusts;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles.Furnitures;

public class GlowWoodDresser : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileSolidTop[Type] = true;
		Main.tileContainer[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileTable[Type] = true;
		Main.tileLavaDeath[Type] = false;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.BasicDresser[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;

		DustType = ModContent.DustType<BlueGlow>();
		AdjTiles = new int[] { TileID.Dressers };
				AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
		TileObjectData.newTile.Height = 2;
		TileObjectData.newTile.Origin = new Point16(0, 1);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
		TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
		TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
		TileObjectData.newTile.AnchorInvalidTiles = new int[] { TileID.MagicalIceBlock };
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
		TileObjectData.addTile(Type);

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(69, 36, 78), name);
	}

	public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

	public override void ModifySmartInteractCoords(ref int width, ref int height, ref int frameWidth, ref int frameHeight, ref int extraY)
	{
		width = 3;
		height = 1;
	}

	public override bool RightClick(int i, int j)
	{
		return FurnitureUtils.DresserRightClick();
	}

	//不确定hjson能否解决，先禁掉了
	//public override void MouseOver(int i, int j)
	//{
	//	string chestName = LocalizedText;
	//	FurnitureUtils.ChestMouseOver<Items.Furnitures.GlowWoodChest>(chestName, i, j);
	//}

	//public override void MouseOverFar(int i, int j)
	//{
	//	string chestName = ContainerName.GetDefault();
	//	FurnitureUtils.ChestMouseFar<Items.Furnitures.GlowWoodChest>(chestName, i, j);
	//}

	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = fail ? 1 : 3;
	}
}