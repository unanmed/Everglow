namespace Everglow.Myth.TheFirefly.Buffs;

public class ShadowPotionBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
	}
	public override void Update(Player player, ref int buffIndex)
	{
		if (!Main.dayTime && (player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight)) //TODO: use the darkness mechanic
		{
			player.nightVision = true;
			player.statDefense += 15;
			player.allDamage += 0.2f;
		}
		else
		{
			player.lifeRegen -= (int)(player.statLifeMax2 * 0.01f);
			player.lifeRegenTime = 0;
		}
	}
}