﻿using Terraria;
namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class GoldLanternLine8 : ModProjectile
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("灯笼须8");
	}
	public override void SetDefaults()
	{
		Projectile.width = 1;
		Projectile.height = 1;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = true;

		Projectile.timeLeft = 140;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Magic;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 270;
	}

	public override void AI()
	{
		Player player = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
		Projectile.velocity = Projectile.velocity.RotatedBy(-Math.PI * 0.025f);
		Vector2 v2 = Projectile.velocity * Main.rand.NextFloat(0.7f, 1.2f);
		Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Projectile.velocity * 1.5f, ModContent.ProjectileType<GoldLanternLine2>(), 2, 0, player.whoAmI, 0, 0);
		if (Projectile.timeLeft > 60)
		{
			if (sca < 1)
				sca += 0.05f;
			else
			{
				sca = 1;
			}
			if (Wid < 12f)
				Wid += 0.2f;
			else
			{
				Wid = 12;
			}
			if (Projectile.timeLeft % 10 == 0)
				Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<GoldLantern>(), 0, 0, player.whoAmI, 0, 0);
		}
		else
		{
			if (sca > 0)
				sca -= 0.02f;
			else
			{
				sca = 0;
			}
			if (Wid > 0)
				Wid -= 0.2f;
			else
			{
				Wid = 0;
			}
		}

	}
	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		Projectile.Kill();
	}
	public override void Kill(int timeLeft)
	{
		/*Player player = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
            for (int j = 0;j < 10; j++)
            {
                Vector2 v2 = Projectile.velocity.RotatedBy(j / 5f * Math.PI);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, v2, ModContent.ProjectileType<Projectiles.GoldLanternLine3>(), 0, 0, player.whoAmI, 0, 0);
            }
            Kill(timeLeft);*/
	}
	private float Wid = 0;
	private float sca = 0;
	int TrueL = 1;
	public override void PostDraw(Color lightColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var bars = new List<Vertex2D>();
		float width = 12;
		if (Projectile.timeLeft < 60)
			width = Projectile.timeLeft / 5f;
		TrueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			TrueL++;
		}
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

			var factor = i / (float)TrueL;
			var w = MathHelper.Lerp(1f, 0.05f, factor);

			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(0.5f, 0.5f) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 1, w)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(0.5f, 0.5f) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 0, w)));
		}
		var Vx = new List<Vertex2D>();
		if (bars.Count > 2)
		{
			Vx.Add(bars[0]);
			var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(254, 254, 254, 0), new Vector3(0, 0.5f, 1));
			Vx.Add(bars[1]);
			Vx.Add(vertex);
			for (int i = 0; i < bars.Count - 2; i += 2)
			{
				Vx.Add(bars[i]);
				Vx.Add(bars[i + 2]);
				Vx.Add(bars[i + 1]);

				Vx.Add(bars[i + 1]);
				Vx.Add(bars[i + 2]);
				Vx.Add(bars[i + 3]);
			}
		}
		Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/heatmapLanternLine").Value;
		Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
	}
}
