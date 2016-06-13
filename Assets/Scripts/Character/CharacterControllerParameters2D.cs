using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class CharacterControllerParameters2D {

	// gravity
	[Range(100f, -100f)]
	public float
		gravity = -30f;

	// movement
	public Vector2 maximumVelocity = new Vector2(20f, 20f);

	// damping
	[Range(0f, 1f)]
	public float
		damping = 0.01f;

	// friction
	[Range(0f, 1f)]
	public float
		friction = 0.3f;

	// slopes
	[Range(0f, 90f)]
	public float
		slopeLimit = 60f;

	// jumping
	public enum JumpMode {
		none,
		ground,
		anywhere
	}
	public JumpMode jumpMode = JumpMode.ground;
	[Range(0f, 20f)]
	public float
		jumpVelocity = 12f;
	[Range(0f, 1f)]
	public float
		jumpCooldown = 0.25f;

}
