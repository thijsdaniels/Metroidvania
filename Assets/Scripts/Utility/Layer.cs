using UnityEngine;
using System.Collections;

public class Layer : MonoBehaviour
{
    public float depth = 0f;

	public void Start()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            depth
        );
	}
}
