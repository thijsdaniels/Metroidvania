using UnityEngine;
using System.Collections;

public class Patroller : MonoBehaviour {

	private Rigidbody2D body;
	private CharacterController2D controller;

	public float walkSpeed;
	public bool avoidCollisions;
	public bool avoidGaps;

	public Vector2 collisionSightStart;
	public Vector2 collisionSightEnd;

	public Vector2 gapSightStart;
	public Vector2 gapSightEnd;

	void Start() {
		body = GetComponent<Rigidbody2D>();
		controller = GetComponent<CharacterController2D>();
	}

	void Update() {

		// turn around if a collision is ahead
		if (avoidCollisions && CollisionAhead()) {
			TurnAround();
		}

		// turn around if a collision is ahead
		if (avoidGaps && GapAhead()) {
			TurnAround();
		}

		// move
		Move();

	}

	bool CollisionAhead() {

		// correct sight start for position and direction
		var correctedSightStart = new Vector2(
			transform.position.x + collisionSightStart.x * transform.localScale.x,
			transform.position.y + collisionSightStart.y * transform.localScale.y
		);

		// correct sight end for position and direction
		var correctedSightEnd = new Vector2(
			transform.position.x + collisionSightEnd.x * transform.localScale.x,
			transform.position.y + collisionSightEnd.y * transform.localScale.y
		);

		// visualize the line of sight
		Debug.DrawLine(correctedSightStart, correctedSightEnd, Color.green);

		// check if the line of sight collides with the gound layer
		return Physics2D.Linecast(
			correctedSightStart,
			correctedSightEnd,
			controller.groundLayerMask | 1 << LayerMask.NameToLayer("Enemies")
		);

	}

	bool GapAhead() {

		// don't scan for gaps in the air
		if (controller && !controller.State.IsGrounded()) {
			return false;
		}
		
		// correct sight start for position and direction
		var correctedSightStart = new Vector2(
			transform.position.x + gapSightStart.x * transform.localScale.x,
			transform.position.y + gapSightStart.y * transform.localScale.y
		);
		
		// correct sight end for position and direction
		var correctedSightEnd = new Vector2(
			transform.position.x + gapSightEnd.x * transform.localScale.x,
			transform.position.y + gapSightEnd.y * transform.localScale.y
		);
		
		// visualize the line of sight
		Debug.DrawLine(correctedSightStart, correctedSightEnd, Color.green);
		
		// check if the line of sight does not collide with the gound layer
		return !Physics2D.Linecast(
			correctedSightStart,
			correctedSightEnd,
			controller.groundLayerMask | controller.oneWayPlatformLayerMask
		);
		
	}

	void TurnAround() {

		transform.localScale = new Vector3(
			transform.localScale.x * -1,
			transform.localScale.y,
			transform.localScale.z
		);

	}

	void Move() {

		var velocity = new Vector2(
			walkSpeed * transform.localScale.x,
			body.velocity.y
		);
		
		body.velocity = velocity;

	}

}
