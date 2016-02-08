using UnityEngine;
using System.Collections;

public class PixelArtCamera : MonoBehaviour {

	public float pixelsPerUnit;
	public int size;

	void Awake()
    {
        float pixelPerfectSize = Mathf.Pow(2, size);
		Camera camera = GetComponent<Camera>();

		camera.orthographicSize = (1 / pixelPerfectSize) * ((Screen.height / 2f) / pixelsPerUnit);
	}

}
