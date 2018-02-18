using Objects.Collectables.Items;
using Objects.Obstacles;
using Traits;
using UnityEngine;

namespace Objects.Projectiles
{
	/// <summary>
	/// 
	/// </summary>
	public class Bomb : Carriable
	{
		/// <summary>
		/// 
		/// </summary>
		private bool FuseLit;
		public float FuseLength = 5f;

		/// <summary>
		/// 
		/// </summary>
		public Fleeting Explosion;

		/// <summary>
		/// 
		/// </summary>
		[HideInInspector] public Bombs Origin;

		/// <summary>
		/// 
		/// </summary>
		public void Update()
		{
			if (FuseLit)
			{
				FuseLength -= Time.deltaTime;

				if (FuseLength <= 0)
				{
					Explode();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void LightFuse()
		{
			FuseLit = true;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Explode()
		{
			if (Origin)
			{
				Origin.BombCount--;
			}

			Instantiate(Explosion, transform.position, Quaternion.identity);

			Destroy(gameObject);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="flame"></param>
		public void OnFlameEnter(Flame flame)
		{
			FuseLength = 0f;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="explosion"></param>
		public void OnExplosionEnter(Explosion explosion)
		{
			FuseLength = 0.15f;
		}
	}
}
