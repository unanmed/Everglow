﻿using Everglow.Myth.Bosses.Acytaea;
using ReLogic.Content;

namespace Everglow.Myth.Bosses.Acytaea.Dusts;

[Pipeline(typeof(NPPipeline), typeof(AcytaeaPipeline))]
public class CosmicFlame2 : Particle
{
	public static Asset<Texture2D> texture;

	public override void Load()
	{
		base.Load();
		texture = ModContent.Request<Texture2D>((GetType().Namespace + "." + Name).Replace('.', '/'));
	}

	public override void Update()
	{
		scale *= 0.99f;
		velocity *= 1.05f;
		if (scale <= 0.1f)
			Active = false;
	}

	public override void Draw()
	{
		Ins.Batch.BindTexture(texture.Value).Draw(position, null, Color.White, 0, texture.Value.Size() / 2, scale, SpriteEffects.None);
	}
}