using UnityEngine;

[RequireComponent(typeof(Animation))]

public class Fleeting : MonoBehaviour
{
	public void OnAnimationEnd()
	{
		Destroy(gameObject);
	}
}
