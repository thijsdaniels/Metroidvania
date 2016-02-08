using UnityEngine;
using System.Collections;

public class JumpPlatform : MonoBehaviour {

	public float jumpVelocity = 10f;

	void OnCharacterControllerEnter2D(CharacterController2D other) {
		other.SetVerticalVelocity(jumpVelocity);
	}

}
