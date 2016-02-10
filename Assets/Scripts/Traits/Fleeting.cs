using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Fleeting : MonoBehaviour
{
	public void OnAnimationEnd()
	{
		Destroy(gameObject);
	}
}
