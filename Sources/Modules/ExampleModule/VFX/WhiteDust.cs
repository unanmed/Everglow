﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Core.VFX.Base;
using Everglow.Sources.Commons.Core.VFX.Pipelines;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.ExampleModule.VFX
{
    [Pipeline(typeof(WCSPipeline), typeof(BloomPipeline))]
    internal class WhiteDust : Particle
    {

        public override void Draw()
        {
            VFXManager.spriteBatch.BindTexture(TextureAssets.MagicPixel.Value).Draw(position, new Rectangle(0, 0, 4, 4), Color.White);
        }
    }
}
