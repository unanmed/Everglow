﻿using System.ComponentModel;
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
        public static bool DebugMode { get; set; }
        public override void OnLoaded()
        {
            DebugMode = debugMode;
        }
        public override void OnChanged()
        {
            DebugMode = debugMode;
        }
    }
}
