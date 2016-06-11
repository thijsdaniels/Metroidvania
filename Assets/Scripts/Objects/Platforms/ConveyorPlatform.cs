using UnityEngine;
using System.Collections;

public class ConveyorPlatform : MonoBehaviour
{
	[Range(-10, 10)]
	public float
		velocity;

	void OnCharacterControllerStay2D(CharacterController2D other)
    {
		other.AddExternalHorizontalVelocity(velocity);
	}
}
