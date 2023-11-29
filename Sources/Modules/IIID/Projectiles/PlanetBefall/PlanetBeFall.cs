using Everglow.Commons.IIID;
using Everglow.Commons.Vertex;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent;
using Humanizer;
using Terraria.GameContent.Drawing;
using Terraria.UI;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Everglow.IIID.Projectiles.NonIIIDProj.GoldenCrack;
using Terraria.Audio;
using Mono.Cecil;
using Everglow.IIID.Projectiles.NonIIIDProj.PlanetBefallWave;
using Everglow.Commons.Utilities;
using System.Diagnostics;
using Everglow.IIID.Projectiles.NonIIIDProj.PlanetBefallArray;
using Everglow.Commons.Skeleton2D;

namespace Everglow.IIID.Projectiles.PlanetBefall
{

	public class PlanetBeFall : IIIDProj
	{
		public Vector2 target;
		public Vector2 spawnposition;
		public int Array;

		public override void SetDef()
		{
			model = ObjReader.LoadFile("Everglow/IIID/Projectiles/PlanetBefall/PlanetBefall.obj");
			IIIDTexture = ModContent.Request<Texture2D>("Everglow/IIID/Projectiles/PlanetBefall/PlanetBeFallTexture").Value;
			NormalTexture = ModContent.Request<Texture2D>("Everglow/IIID/Projectiles/PlanetBefall/PlanetBeFallTexture").Value;
			MaterialTexture = TextureAssets.MagicPixel.Value;
			EmissionTexture = ModContent.Request<Texture2D>("Everglow/IIID/Projectiles/PlanetBefall/PlanetBeFallEmission").Value;
			bloom = new BloomParams
			{
				BlurIntensity = 1.0f,
				BlurRadius = 1.0f
			};
			artParameters = new ArtParameters
			{
				EnablePixelArt = true,
				EnableOuterEdge = true,
			};
			viewProjectionParams = new ViewProjectionParams
			{
				ViewTransform = Matrix.Identity,
				FieldOfView = MathF.PI / 3f,
				AspectRatio = 1.0f,
				ZNear = 1f,
				ZFar = 1200f
			};
		}

		public override Matrix ModelMovementMatrix()
		{
			return DefaultPerspectiveMatrix();
		}
		public override void OnSpawn(IEntitySource source)
		{
			Player player = Main.player[Projectile.owner];
			/*Array = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<PlanetBefallArray>(), 0, 0, player.whoAmI);
			Main.projectile[Array].Center = Main.MouseWorld;
			Projectile.ai[0] = Main.projectile[Array].Center.X;
			Projectile.ai[1] = Main.projectile[Array].Center.Y;
			Projectile.velocity = Vector2.Normalize(Main.projectile[Array].Center - new Vector2(player.Center.X, Main.MouseWorld.Y - 1500)) / 10;

			for (int i = 0; i < 16; i++)
			{
				Vector2 v = new Vector2(0.001f, 0);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), new Vector2(Projectile.Center.X, Projectile.Center.Y), v.RotatedBy(Math.PI * i / 8).RotatedByRandom(Math.PI * i / 100), ModContent.ProjectileType<GoldenCrack>(), 10, 0);
			}

			PlanetBeFallScreenMovePlayer PlanetBeFallScreenMovePlayer = player.GetModPlayer<PlanetBeFallScreenMovePlayer>();
			PlanetBeFallScreenMovePlayer.PlanetBeFallAnimation = true;
			PlanetBeFallScreenMovePlayer.proj = Projectile;

			target = new Vector2(Projectile.ai[0], Projectile.ai[1]);




			base.OnSpawn(source);*/
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			target = new Vector2(Projectile.ai[0], Projectile.ai[1]);

			if ((Projectile.Center - target).Length() < 100)
			{
				Projectile.Kill();
			}
			if (Projectile.timeLeft < 1170)
			{
				if (Projectile.velocity.Length() < 10)
				{
					Projectile.velocity *= 1.05f;
				}
				if (s < 8500)
					s = MathUtils.Lerp(0.05f, s, 8500);
			}
			player.heldProj = Projectile.whoAmI;
		}
		public override void OnKill(int timeLeft)
		{
			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.type == ModContent.ProjectileType<PlanetBefallArray>() && proj == Main.projectile[Array])
				{
					(proj.ModProjectile as PlanetBefallArray).BloomIntensityChange = true;
				}
			}

			Player player = Main.player[Projectile.owner];
			PlanetBeFallScreenMovePlayer PlanetBeFallScreenMovePlayer = player.GetModPlayer<PlanetBeFallScreenMovePlayer>();
			PlanetBeFallScreenMovePlayer.PlanetBeFallAnimation = false;
			PlanetBeFallScreenMovePlayer.proj = null;
			PlanetBeFallScreenMovePlayer.AnimationTimer = 0;


			SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, player.Center);
			ScreenShaker Gsplayer = player.GetModPlayer<ScreenShaker>();
			Gsplayer.FlyCamPosition = new Vector2(0, 150).RotatedByRandom(6.283);
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlanetBefallWave>(), 0, 0, Projectile.owner, 4f);
			float k1 = Math.Clamp(Projectile.velocity.Length(), 1, 3);
			float k2 = Math.Clamp(Projectile.velocity.Length(), 6, 10);
			float k0 = 1f / 4 * k2;
			foreach (NPC target in Main.npc)
			{
				float Dis = (target.Center - Projectile.Center).Length();

				if (Dis < 2500)
				{
					if (!target.dontTakeDamage && !target.friendly && target.active)
					{
						player.ApplyDamageToNPC(target, Projectile.damage, Projectile.knockBack, 0, Main.rand.NextBool(22, 33));
					}
				}
			}
			base.OnKill(timeLeft);
		}

		//public ObjReader.Model model = ObjReader.LoadFile("Everglow/IIID/Projectiles/PlanetBefall/PlanetBefall.obj");

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			//overPlayers.Add(index);
		}
		float s = 5000f;

		public class TestProjModelSystem : ModSystem
		{
			public override void OnModLoad()
			{
				//PlanetBeFall.model = ObjReader.LoadFile("Everglow/IIID/Projectiles/PlanetBefall/PlanetBefall.obj");
				//PlanetBeFall.NormalMap = ModContent.Request<Texture2D>("Everglow/IIID/Projectiles/PlanetBefall/PlanetBeFallTexture");
				base.OnModLoad();
			}

		}
		public class PlanetBeFallScreenMovePlayer : ModPlayer
		{
			public int AnimationTimer = 0;
			public bool PlanetBeFallAnimation = false;
			public Projectile proj;
			const float MaxTime = 180;
			public override void ModifyScreenPosition()
			{
				Vector2 target;
				if (proj != null)
				{
					if (proj.owner == Player.whoAmI)
					{
						target = proj.Center - Main.ScreenSize.ToVector2() / 2;
						if (PlanetBeFallAnimation)
						{
							AnimationTimer += 1;
							float Value = (1 - MathF.Cos(AnimationTimer / 60f * MathF.PI)) / 2f;
							if (AnimationTimer >= 60 && AnimationTimer < 120)
							{
								Value = 1;
							}
							if (AnimationTimer >= 120)
							{
								Value = (1 + MathF.Cos((AnimationTimer - 120) / 60f * MathF.PI)) / 2f;
							}

							if (AnimationTimer >= MaxTime)
							{
								AnimationTimer = (int)MaxTime;
								PlanetBeFallAnimation = false;
							}
							Player.immune = true;
							Player.immuneTime = 1;
							Main.screenPosition = (Value).Lerp(Main.screenPosition, target);
						}
					}
				}
			}
		}
	}
}
