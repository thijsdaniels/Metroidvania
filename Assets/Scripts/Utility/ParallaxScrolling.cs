using UnityEngine;
using System.Collections;

public class ParallaxScrolling : MonoBehaviour
{
	public Transform[] backgrounds;
	public float scale;
	public float depth;

	private Vector3 lastPosition;

	public void Awake()
    {
		lastPosition = transform.position;
	}

	public void Update()
    {
		var parallax = (lastPosition - transform.position) * -scale;

		for (var i = 0; i < backgrounds.Length; i++) {

			var currentPosition = backgrounds[i].position;
			var targetPosition = currentPosition + parallax * (i * depth + 1);

			backgrounds[i].position = Vector3.Lerp(
				currentPosition,
				targetPosition,
				Time.deltaTime
			);

		}

		lastPosition = transform.position;
	}
}
