﻿using Everglow.Sources.Modules.AssetReplaceModule;
using System.ComponentModel;

using Terraria.ModLoader.Config;

namespace Everglow.Sources.Commons.Function.FeatureFlags
{
    public class EverglowConfig : ModConfig
    {
        /// <summary>
        /// 用于整个Mod，包括各个客户端的设置
        /// </summary>
        public override ConfigScope Mode => ConfigScope.ServerSide;

        /// <summary>
        /// 是否是Debug模式，如果是那么我们可以打印很多debug信息，或者运行一些debug下才有的逻辑
        /// </summary>
        [DefaultValue(false)]
        [Label("Enable Debug Mode")]
        [Tooltip("[For developers] Enable debug mode to allow debug functions to run")]
        public bool debugMode;

        public static bool DebugMode
        {
            get
            {
                return ModContent.GetInstance<EverglowConfig>().debugMode;
            }
        }
    }

    public class EverglowClientConfig : ModConfig
    {
        /// <summary>
        /// 用于各个客户端的个性化设置
        /// </summary>
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(TextureReplaceMode.Terraria)]
        [Label("$Mods.Everglow.Config.TextureReplace.Label")] //Mods.Everglow.Config.TextureReplace.Label
        [Tooltip("$Mods.Everglow.Config.TextureReplace.Tooltip")] //Mods.Everglow.Config.TextureReplace.Tooltip
        [DrawTicks]
        public TextureReplaceMode TextureReplace;

        [DefaultValue(AudioReplaceMode.MothFighting)]
        [Label("$Mods.Everglow.Config.AudioReplace.Label")] //Mods.Everglow.Config.TextureReplace.Label
        [Tooltip("$Mods.Everglow.Config.AudioReplace.Tooltip")] //Mods.Everglow.Config.TextureReplace.Tooltip
        [DrawTicks]
        public AudioReplaceMode AudioReplace;

        public override void OnChanged() {
            if ((int)TextureReplace >= 3) {
                TextureReplace = TextureReplaceMode.Terraria;
            }
            if ((int)AudioReplace >= 3)
            {
                AudioReplace = AudioReplaceMode.MothFighting;
            }
            if (AssetReplaceModule.IsLoaded)
                AssetReplaceModule.ReplaceTextures(TextureReplace);
            base.OnChanged();
		}
	}

    public enum TextureReplaceMode
    {
        Terraria,
        [Label("Eternal Resolve")]
        EternalResolve,
        Myth
    }
    public enum AudioReplaceMode
    {
        [Label("Original")]
        MothFighting,
        [Label("Alternate")]
        AltMothFighting,
        [Label("Old")]
        OldMothFighting
    }
}
