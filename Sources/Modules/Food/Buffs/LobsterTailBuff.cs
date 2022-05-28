﻿namespace Everglow.Sources.Modules.Food.Buffs
{
    public class LobsterTailBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("LobsterTailBuff");
            Description.SetDefault("加6防御,25%挖矿速度\n“壮阳”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 6; // 加6防御
            player.pickSpeed -= 0.25f;// 加25%挖矿速度
            player.wellFed = true;
        }
    }
}

