using UnityEngine;

namespace Utility
{
	/// <summary>
	/// 
	/// </summary>
	public class ParallaxScrolling : MonoBehaviour
	{
		/// <summary>
		/// 
		/// </summary>
		public Transform[] Backgrounds;
		
		/// <summary>
		/// 
		/// </summary>
		public float Scale;
		
		/// <summary>
		/// 
		/// </summary>
		public float Depth;

		/// <summary>
		/// 
		/// </summary>
		private Vector3 LastPosition;

		/// <summary>
		/// 
		/// </summary>
		public void Awake()
		{
			LastPosition = transform.position;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Update()
		{
			var parallax = (LastPosition - transform.position) * -Scale;

			for (int i = 0; i < Backgrounds.Length; i++) {

				Vector3 currentPosition = Backgrounds[i].position;
				Vector3 targetPosition = currentPosition + parallax * (i * Depth + 1);

				Backgrounds[i].position = Vector3.Lerp(
					currentPosition,
					targetPosition,
					Time.deltaTime
				);

			}

			LastPosition = transform.position;
		}
	}
}
