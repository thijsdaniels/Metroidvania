using System;

namespace Character
{
	/// <summary>
	/// 
	/// </summary>
	/// TODO: Move the physics=related states to the Physics.Body class.
	/// TODO: Move the trait-related states to the respective traits.
	[Serializable]
	public class State
	{
		/// <summary>
		/// 
		/// </summary>
		public bool CollisionRight;
		public bool CollisionLeft;
		public bool CollisionAbove;
		public bool CollisionBelow;

		/// <summary>
		/// 
		/// </summary>
		public bool SlopeUp;
		public bool SlopeDown;
		public float SlopeAngle;

		/// <summary>
		/// 
		/// </summary>
		public bool Rolling;
		public bool Climbing;
		public bool Swimming;
		public bool Aiming;
		public bool Attacking;
		public bool Crouching;

		/// <summary>
		/// 
		/// </summary>
		public enum MovementModes
		{
			Walking,
			Swimming,
			Aiming,
			Crouching,
			Climbing,
			Rolling
		}
		
		/// <summary>
		/// 
		/// </summary>
		public MovementModes MovementMode
		{
			get
			{
				if (IsAiming())
				{
					return MovementModes.Aiming;
				}

				if (IsRolling())
				{
					return MovementModes.Rolling;
				}

				if (IsCrouching())
				{
					return MovementModes.Crouching;
				}

				if (IsSwimming())
				{
					return MovementModes.Swimming;
				}

				if (IsClimbing())
				{
					return MovementModes.Climbing;
				}

				return MovementModes.Walking;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsGrounded()
		{
			return (CollisionBelow || Climbing) && !Swimming;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsRolling()
		{
			return Rolling;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsClimbing()
		{
			return Climbing;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsSwimming()
		{
			return Swimming;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsAiming()
		{
			return Aiming;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsCrouching()
		{
			return Crouching;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsAttacking()
		{
			return Attacking;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsSliding()
		{
			return (CollisionLeft || CollisionRight) && !CollisionBelow;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsColliding()
		{
			return CollisionLeft || CollisionRight || CollisionAbove || CollisionBelow;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsCollidingHorizontally()
		{
			return CollisionLeft || CollisionRight;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsCollidingVertically()
		{
			return CollisionAbove || CollisionBelow;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsOnSlope()
		{
			return SlopeUp || SlopeDown;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Reset()
		{
			CollisionRight = false;
			CollisionLeft = false;
			CollisionAbove = false;
			CollisionBelow = false;

			SlopeUp = false;
			SlopeDown = false;
			SlopeAngle = 0;
		}
	}
}
