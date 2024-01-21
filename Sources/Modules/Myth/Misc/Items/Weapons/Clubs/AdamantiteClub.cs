namespace Everglow.Myth.Misc.Items.Weapons.Clubs;

public class AdamantiteClub : ClubItem
{
	public override void SetDef()
	{
		Item.damage = 74;
		Item.value = 3690;
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.AdamantiteClub>();
		ProjTypeSmash = ModContent.ProjectileType<Projectiles.Weapon.Melee.Clubs.AdamantiteClub_smash>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.AdamantiteBar, 18)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
