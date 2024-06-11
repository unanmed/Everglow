using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.NPCs;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Utilities.Terraria.Utilities;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class RockElemental_ThrowingStone : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 1023;
		Projectile.scale = 0;
	}

	public NPC MyOwner;
	public int PolymerizationTimer;
	public Vector2 OffestSuck;
	public bool ShotAway = false;

	public override void AI()
	{
		if (MyOwner == null || !MyOwner.active || MyOwner.type != ModContent.NPCType<RockElemental>())
		{
			if (Projectile.timeLeft > 60)
			{
				Projectile.timeLeft = 60;
			}
		}
		if (Projectile.timeLeft <= 60)
		{
			Projectile.Kill();
			return;
		}

		// 丢出去之后
		if (ShotAway)
		{
			Projectile.rotation += 0.1f;
			Projectile.velocity.Y += 0.163f;
			for (int j = 0; j < 3; j++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<RockElemental_Energy_normal>());
				d.velocity = Projectile.velocity * 0.5f;
				d.scale = Main.rand.NextFloat(0.75f, 1.4f);
				d.noGravity = true;
			}
			return;
		}
		Player player = Main.player[MyOwner.target];
		if (PolymerizationTimer >= 0)
		{
			Projectile.rotation = MyOwner.rotation * 0.25f + Projectile.rotation * 0.75f;
			Projectile.Center = MyOwner.Center + new Vector2(-24, 36).RotatedBy(MyOwner.rotation);
			PolymerizationTimer++;
			if (PolymerizationTimer > 30)
			{
				Projectile.scale = (float)Utils.Lerp(Projectile.scale, 1f, 0.05f);
			}
			if (PolymerizationTimer > 120)
			{
				Projectile.scale = 1;
				PolymerizationTimer = -1;
				SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact.WithVolume(0.5f), Projectile.Center);
			}
			if (PolymerizationTimer < 80 && PolymerizationTimer > 5)
			{
				for (int g = 0; g < 3; g++)
				{
					Vector2 vel = new Vector2(0, Main.rand.NextFloat(2, 8)).RotatedByRandom(MathHelper.TwoPi);
					Vector2 addPos = new Vector2(0, Main.rand.NextFloat(132, 181)).RotatedBy(MyOwner.rotation + MathHelper.PiOver2 + Main.rand.NextFloat(-0.1f, 0.1f));
					float mulScale = Main.rand.NextFloat(2f, 8f);
					var current = new RockElemental_SuckingLine
					{
						velocity = vel,
						Active = true,
						Visible = true,
						position = Projectile.Center + addPos - vel * 15,
						maxTime = Main.rand.Next(142, 184),
						scale = mulScale,
						VFXOwner = Projectile,
						ai = new float[] { 0f, Main.rand.NextFloat(-0.05f, 0.05f) },
					};
					Ins.VFXManager.Add(current);
				}
				for (int g = 0; g < 2; g++)
				{
					Vector2 vel = new Vector2(0, Main.rand.NextFloat(2, 8)).RotatedByRandom(MathHelper.TwoPi);
					Vector2 addPos = new Vector2(0, Main.rand.NextFloat(132, 181)).RotatedBy(MyOwner.rotation + MathHelper.PiOver2 + Main.rand.NextFloat(-0.1f, 0.1f));
					Dust dust = Dust.NewDustDirect(Projectile.Center + addPos - vel * 15 - new Vector2(4), 0, 0, ModContent.DustType<RockElemental_Energy>());
					dust.customData = Projectile;
					dust.velocity = vel;
					dust.scale = 0.07f;
				}
				RockElemental rockOwner = MyOwner.ModNPC as RockElemental;
				if (rockOwner != null)
				{
					OffestSuck = rockOwner.SuckPoint - Projectile.Center + Utils.SafeNormalize(rockOwner.SuckPoint - Projectile.Center, Vector2.zeroVector) * 8;
				}
			}

			// 震动
			if (PolymerizationTimer == 88)
			{
				ShakerManager.AddShaker(Projectile.Center, new Vector2(0, -1), 20, 10, 120);
				Projectile.width = 32;
				Projectile.height = 32;
			}

			// 吸起音效
			if (PolymerizationTimer == 90)
			{
				for (int g = 0; g < 30; g++)
				{
					Vector2 vel = new Vector2(0, Main.rand.NextFloat(8, 24)).RotatedByRandom(MathHelper.TwoPi);
					Dust dust = Dust.NewDustDirect(Projectile.Center + OffestSuck, 0, 0, ModContent.DustType<RockElemental_Energy>());
					dust.velocity = vel;
					dust.customData = Projectile;
					dust.scale = Main.rand.NextFloat(0.06f, 0.1f);
				}
				SoundEngine.PlaySound(SoundID.DD2_PhantomPhoenixShot, Projectile.Center);
			}
			if (PolymerizationTimer > 90)
			{
				OffestSuck *= 0.85f;
			}

			// 吸力音效
			if (PolymerizationTimer == 1)
			{
				SoundEngine.PlaySound(new SoundStyle(ModAsset.RockElemental_Sucking_Mod).WithPitchOffset(0.5f).WithVolumeScale(0.5f), Projectile.Center);
			}
		}

		// 跟随岩石元素一起旋转
		else
		{
			Projectile.rotation = MyOwner.rotation;
			Vector2 newPos = MyOwner.Center + new Vector2(-24, 36).RotatedBy(MyOwner.rotation);
			Vector2 toPlayer = player.Center - Projectile.Center;
			Vector2 release = newPos - Projectile.Center;
			float cosTheta = Vector2.Dot(toPlayer, release) / toPlayer.Length() / release.Length();
			if (cosTheta > 0.95f && MyOwner.ai[2] > 0.2f)
			{
				ShotAway = true;
				Projectile.velocity = PredictVec(Projectile.position, player, 14f, 0.163f);
				MyOwner.velocity -= PredictVec(Projectile.position, player, 7f, 0.163f);
				if (MyOwner.ai[0] > 30)
				{
					MyOwner.ai[0] = 30;
				}
				Projectile.tileCollide = true;
			}
			if (MyOwner.ai[2] > 0.4f)
			{
				ShotAway = true;
				Projectile.velocity = PredictVec(Projectile.position, player, 28f, 0.163f);

				MyOwner.velocity -= PredictVec(Projectile.position, player, 14f, 0.163f);
				if (MyOwner.ai[0] > 30)
				{
					MyOwner.ai[0] = 30;
				}
				Projectile.tileCollide = true;
			}
			Projectile.Center = newPos;
		}
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		if (PolymerizationTimer > 0 && PolymerizationTimer < 100)
		{
			Projectile.hide = true;
			behindNPCsAndTiles.Add(index);
		}
		else
		{
			Projectile.hide = true;
			overPlayers.Add(index);
		}
	}

	public override void OnSpawn(IEntitySource source)
	{
		PolymerizationTimer = 0;
		if (MyOwner == null)
		{
			foreach (var npc in Main.npc)
			{
				if (npc.type == ModContent.NPCType<RockElemental>())
				{
					if (npc.active && npc.life >= 0)
					{
						if ((npc.Center - Projectile.Center).Length() < 500)
						{
							MyOwner = npc;
							break;
						}
					}
				}
			}
		}
		if (MyOwner == null)
		{
			Projectile.Kill();
		}
		RockElemental rockOwner = MyOwner.ModNPC as RockElemental;
		if (rockOwner == null)
		{
			Projectile.Kill();
		}
		OffestSuck = rockOwner.SuckPoint - Projectile.Center;
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
	}

	public override void OnKill(int timeLeft)
	{
		for (int x = 0; x < 16; x++)
		{
			Dust d = Dust.NewDustDirect(Projectile.position - new Vector2(4), Projectile.width, Projectile.height, ModContent.DustType<RockElemental_fragments>(), 0f, 0f, 0, default, 0.7f);
			d.velocity = new Vector2(0, Main.rand.NextFloat(7f, 16f)).RotatedByRandom(6.283);
		}
		for (int x = 0; x < 32; x++)
		{
			Dust d = Dust.NewDustDirect(Projectile.position - new Vector2(4), Projectile.width, Projectile.height, DustID.WitherLightning, 0f, 0f, 0, default, Main.rand.NextFloat(0.6f, 1.1f));
			d.velocity = new Vector2(0, Main.rand.NextFloat(2f, 11f)).RotatedByRandom(6.283);
		}
		GenerateSmog(8);
		SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
		ShakerManager.AddShaker(Projectile.Center, new Vector2(0, -1), 20, 30, 120);
	}

	public void GenerateSmog(int Frequency)
	{
		for (int g = 0; g < Frequency / 2 + 1; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new RockSmogDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(37, 45),
				scale = Main.rand.NextFloat(40f, 55f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
			};
			Ins.VFXManager.Add(somg);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D texture = ModAsset.RockElemental_ThrowingStone.Value;
		Vector2 drawCenter = Projectile.Center - Main.screenPosition;
		lightColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
		if (PolymerizationTimer < 0)
		{
			Main.EntitySpriteDraw(texture, drawCenter, null, lightColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
		}
		if (PolymerizationTimer >= 0)
		{
			Vector2 shake = new Vector2(0, Main.rand.NextFloat(5f)).RotatedByRandom(MathHelper.TwoPi);
			float silence = 1f;
			if (PolymerizationTimer > 90)
			{
				silence = (120 - PolymerizationTimer) / 30f;
			}
			shake *= silence;
			Vector2 drawCenter2 = drawCenter + OffestSuck + shake;
			for (int i = 0; i < 20 * silence; i++)
			{
				Main.EntitySpriteDraw(texture, drawCenter2 + new Vector2(0, 7).RotatedBy(i / 5d * MathHelper.TwoPi + Main.time * 0.08), null, new Color(0.7f, 0.2f, 1f, 0f), Projectile.rotation + i, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
			}
			Main.EntitySpriteDraw(texture, drawCenter2 + shake, null, lightColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
		}
		return false;
	}

	private Tuple<float, int> GetHitPoint(Vector2 pos, Vector2 Vec, Vector2 targetPos, float gravity)
	{
		int t = 1;
		while (true)
		{
			pos += Vec;
			if (Vec.Y > 0 && pos.Y > targetPos.Y)
			{
				return new Tuple<float, int>(pos.X, t);
			}
			Vec.Y += gravity;
			t++;
		}
	}
	private Tuple<Vector2, int> GetShootVec(Vector2 pos, Vector2 target, float speed, float gravity)
	{
		float L, R;

		L = 5 * MathF.PI / 4;
		R = -MathF.PI / 4;

		Tuple<Vector2, int> parabola = new Tuple<Vector2, int>(Vector2.Zero, 0);
		for (int i = 0; i < 25; i++)
		{
			float mid = (L + R) / 2;
			Vector2 Vec = Vector2.UnitX.RotatedBy(mid) * speed;
			Tuple<float, int> fakepoint = GetHitPoint(pos, Vec, target, gravity);
			if (fakepoint.Item1 < target.X)
			{
				L = mid;
			}
			else
			{
				R = mid;
			}
			parabola = new Tuple<Vector2, int>(Vec, fakepoint.Item2);
		}
		return parabola;
	}
	private Vector2 PredictVec(Vector2 pos, Player player, float speed, float gravity)
	{
		Vector2 target = player.Center - pos;
		for (int i = 0; i < 25; i++)
		{
			Tuple<Vector2, int> parabola = GetShootVec(pos, target, speed, gravity);
			Vector2 playerPos = player.Center + parabola.Item2 * player.velocity;
			if (Vector2.Distance(target, playerPos) < 0.1f)
			{
				return parabola.Item1;
			}
			target = playerPos;
		}
		return target*speed;
	}
}