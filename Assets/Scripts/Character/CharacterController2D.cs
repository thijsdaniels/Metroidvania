using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]

public class CharacterController2D : MonoBehaviour {

	// debugging
	public bool debugMode;

	// raycasting
	[Range(0f, 0.1f)]
	public float skinThickness = 0.05f;
	[Range(0, 20)]
	public int horizontalRays = 12;
	[Range(0, 10)]
	public int verticalRays = 6;
	private float horizontalRaySpread;
	private float verticalRaySpread;
	private Vector3 raycastOriginTopLeft;
	private Vector3 raycastOriginBottomLeft;
	private Vector3 raycastOriginBottomRight;

	// collisions
	public bool handleCollisions = true;
	private BoxCollider2D boxCollider;
	public LayerMask groundLayerMask;
	public LayerMask oneWayPlatformLayerMask;

	// platforms
	public GameObject standingOn;
	private GameObject previousStandingOn;
	private Vector3 activeGlobalPlatformPoint;
	private Vector3 activeLocalPlatformPoint;
	public Vector2 platformVelocity;

	// movement
	public Vector2 velocity;
	private Vector2 externalVelocity;
	private static readonly float slopeLimitTangent = Mathf.Tan(75f * Mathf.Deg2Rad);

	// jumping
	private float jumpTimeout;
	public int airJumps;
	private int currentAirJump;

	// climbing
	public int climbables;
	public Climbable climbable;

	// parameters
	public CharacterControllerParameters2D defaultParameters;
	private CharacterControllerParameters2D overrideParameters;
	public CharacterControllerParameters2D Parameters { get { return overrideParameters ?? defaultParameters; } }

	// state
	public CharacterControllerState2D State = new CharacterControllerState2D();

	//////////////////////
	///// GAME HOOKS /////
	//////////////////////

	public void Awake() {

		// reference components
		boxCollider = GetComponent<BoxCollider2D>();

		// calculate distance between rays
		CalculateRaySpread();

	}

	public void LateUpdate() {

		// apply gravity
		ApplyGravity();

		// apply friction
		ApplyFriction();

		// apply damping
		ApplyDamping();

		// handle movement
		var combinedVelocity = velocity + externalVelocity;
		var deltaMovement = DetermineMovement(combinedVelocity * Time.deltaTime);
		Move(deltaMovement);
		RestrictVelocity(deltaMovement, externalVelocity * Time.deltaTime);

		// determine what we are standing on
		DetermineGrounding();

		// run jump timer
		if (jumpTimeout > 0) {
			jumpTimeout = Mathf.Max(0, jumpTimeout - Time.deltaTime);
		}

	}

	/////////////////////////////
	///// VELOCITY MUTATORS /////
	/////////////////////////////
	
	public void AddVelocity(Vector2 _velocity) {
		velocity += _velocity;
	}
	
	public void AddHorizontalVelocity(float _velocity) {
		velocity.x += _velocity;
	}
	
	public void AddVerticalVelocity(float _velocity) {
		velocity.y += _velocity;
	}
	
	public void SetVelocity(Vector2 _velocity) {
		velocity = _velocity;
	}
	
	public void SetHorizontalVelocity(float _velocity) {
		velocity.x = _velocity;
	}
	
	public void SetVerticalVelocity(float _velocity) {
		velocity.y = _velocity;
	}

	public void AddExternalHorizontalVelocity(float _velocity) {
		externalVelocity.x = _velocity;
	}

	///////////////////
	///// PHYSICS /////
	///////////////////

	public void OverrideParameters(CharacterControllerParameters2D parameters) {
		overrideParameters = parameters;
	}

	public void ResetParameters() {
		overrideParameters = null;
	}

	private void ApplyGravity() {

		// bypass gravity when climbing
		if (State.IsClimbing()) {
			return;
		}

		// add gravity to the character's velocity
		AddVerticalVelocity(Parameters.gravity * Time.deltaTime);

	}

	private void ApplyFriction() {

		var restrictedVelocity = velocity.x;
		if (velocity.x > 0) {
			restrictedVelocity = Mathf.Max(0, velocity.x - Parameters.friction);
		} else if (velocity.x < 0) {
			restrictedVelocity = Mathf.Min(0, velocity.x + Parameters.friction);
		}

		SetHorizontalVelocity(restrictedVelocity);
	}

	private void ApplyDamping() {
		SetVelocity(velocity * (1 - Parameters.damping));
	}

	//////////////////////
	///// RAYCASTING /////
	//////////////////////

	private void CalculateRaySpread() {
		
		// calculate horizontal spread of (vertical) rays
		var colliderWidth = boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2 * skinThickness);
		horizontalRaySpread = colliderWidth / (verticalRays - 1);
		
		// calculate vertical spread of (horizontal) rays
		var colliderHeight = boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * skinThickness);
		verticalRaySpread = colliderHeight / (horizontalRays - 1);
		
	}
	
	private void CalculateRayOrigins() {
		
		var size = new Vector2(
			boxCollider.size.x * Mathf.Abs(transform.localScale.x),
			boxCollider.size.y * Mathf.Abs(transform.localScale.y)
		);
		
		var center = new Vector2(
			boxCollider.offset.x * transform.localScale.x,
			boxCollider.offset.y * transform.localScale.y
		);
		
		raycastOriginTopLeft = transform.position + new Vector3(
			center.x - size.x / 2 + skinThickness,
			center.y + size.y / 2 - skinThickness
		);
		
		raycastOriginBottomLeft = transform.position + new Vector3(
			center.x - size.x / 2 + skinThickness,
			center.y - size.y / 2 + skinThickness
		);
		
		raycastOriginBottomRight = transform.position + new Vector3(
			center.x + size.x / 2 - skinThickness,
			center.y - size.y / 2 + skinThickness
		);
		
	}
	
	void OnDrawGizmos() {

		if (debugMode) {

			Gizmos.DrawWireSphere(new Vector3(
				raycastOriginTopLeft.x,
				raycastOriginTopLeft.y,
				0
			), skinThickness);
			
			Gizmos.DrawWireSphere(new Vector3(
				raycastOriginBottomLeft.x,
				raycastOriginBottomLeft.y,
				0
			), skinThickness);
			
			Gizmos.DrawWireSphere(new Vector3(
				raycastOriginBottomRight.x,
				raycastOriginBottomRight.y,
				0
			), skinThickness);
		
		}

	}

	///////////////////
	///// JUMPING /////
	///////////////////

	public bool CanJump() {

		if (Parameters.jumpMode == CharacterControllerParameters2D.JumpMode.anywhere) {
			return jumpTimeout <= 0;

		} else if (Parameters.jumpMode == CharacterControllerParameters2D.JumpMode.ground) {
			return State.IsGrounded() || currentAirJump < airJumps;

		} else {
			return false;
		}

	}

	public void Jump() {

		if (!State.IsGrounded()) {
			if (Parameters.jumpMode == CharacterControllerParameters2D.JumpMode.ground) {
				currentAirJump++;
			}
			SendMessage("OnAirJump", SendMessageOptions.DontRequireReceiver);
		} else {
			SendMessage("OnJump", SendMessageOptions.DontRequireReceiver);
		}

        if (State.IsClimbing())
        {
            StopClimbing();
        }

        SetVerticalVelocity(Parameters.jumpVelocity);

		jumpTimeout = Parameters.jumpCooldown;

	}

	public void OnLand() {
		currentAirJump = 0;
	}

	////////////////////
	///// CLIMBING /////
	////////////////////

	// TODO Move all climbing related code to a "Climber Trait" class?

	public void AddClimbable(Climbable climbable)
    {
		climbables++;

		this.climbable = climbable;
	}
	
	public void RemoveClimbable()
    {
		if (climbables > 0) {
			climbables--;
		}

		if (climbables == 0 && State.IsClimbing()) {
			StopClimbing();
			climbable = null;
		}
	}

	public bool CanClimb() {
		return climbables > 0;
	}

	public void StartClimbing()
    {
        State.climbing = true;

		SetHorizontalVelocity(0);

        currentAirJump = 0;

        transform.position = new Vector3(
			climbable.transform.position.x + climbable.offset,
			transform.position.y,
			transform.position.z
		);
	}

	public void StopClimbing()
    {
		State.climbing = false;

		SetVerticalVelocity(0);
	}

	//////////////////
	///// MOVING /////
	//////////////////

	// TODO Abstract parts of this method to make it more readable.
	private Vector2 DetermineMovement(Vector2 deltaMovement) {

		// remember whether the character is grounded
		var wasGrounded = State.IsGrounded();

		// then reset the state
		State.Reset();

		// handle collisions unless otherwise requested
		if (handleCollisions) {

			// handle moving platforms
			HandleMovingPlatforms();

			// calculate from where to cast rays
			CalculateRayOrigins();

			// handle slopes
			if (deltaMovement.y < 0 && wasGrounded) {
				DetermineVerticalSlopeMovement(ref deltaMovement);
			}

			// handle horizontal movement
			if (Mathf.Abs(deltaMovement.x) > 0.001f) {
				DetermineHorizontalMovement(ref deltaMovement);
			}

			// handle vertical movement
			DetermineVerticalMovement(ref deltaMovement);

			CorrectHorizontalPlacement(ref deltaMovement, true);
			CorrectHorizontalPlacement(ref deltaMovement, false);

		}

		return deltaMovement;

	}

	private void Move(Vector2 deltaMovement) {

		// move the player
		transform.Translate(deltaMovement, Space.World);

	}

	private void RestrictVelocity(Vector2 deltaMovement, Vector2 externalMovement) {

		// subtract the external movement, because we only care about the character's movement
		deltaMovement -= externalMovement;

		// update the player's velocity to whatever it was limited to
		if (Time.deltaTime != 0) {
			velocity = deltaMovement / Time.deltaTime;
		} else {
			velocity = deltaMovement; //TODO: is this correct? experiment with pausing the game
		}

		// clamp the velocity
		//TODO: Shouldn't it be clamped in both directions? That is Mathf.Max(velocity.x, -Parameters.maximumVelocity)?
		velocity.x = Mathf.Min(velocity.x, Parameters.maximumVelocity.x);
		velocity.y = Mathf.Min(velocity.y, Parameters.maximumVelocity.y);

		// correct velocity while moving up slopes
		if (State.slopeUp) {
			velocity.y = 0;
		}

		// clear the external velocity
		externalVelocity = Vector2.zero;

	}

    ///////////////////////////////
    ///// COLLISION DETECTION /////
    ///////////////////////////////

	/**
	 * Using additional inner rays, checks if a solid object
	 * is penetrating the character and if so, moves the
	 * character away from the object horizontally. This makes
	 * the player get pushed to the side by, for example,
	 * moving platforms.
	 */
	private void CorrectHorizontalPlacement(ref Vector2 deltaMovement, bool isRight) {

		var halfWidth = (boxCollider.size.x * Mathf.Abs(transform.localScale.x)) * 0.5f;
		var rayOrigin = isRight ? raycastOriginBottomRight : raycastOriginBottomLeft;

		if (isRight) {
			rayOrigin.x -= (halfWidth - skinThickness);
		} else {
			rayOrigin.x += (halfWidth - skinThickness);
		}

		var rayDirection = isRight ? Vector2.right : -Vector2.right;

		var offset = 0f;
		for (var i = 1; i < horizontalRays - 1; i++) {

			var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * verticalRaySpread));
			if (debugMode) {
				Debug.DrawRay(rayVector, rayDirection * halfWidth, isRight ? Color.cyan : Color.magenta);
			}

			var raycastHit = Physics2D.Raycast(rayVector, rayDirection, halfWidth, groundLayerMask);
			if (!raycastHit) {
				continue;
			}

			offset = isRight ? ((raycastHit.point.x - transform.position.x) - halfWidth) : (halfWidth - (transform.position.x - raycastHit.point.x));

		}

		deltaMovement.x += offset;

	}

	private void DetermineHorizontalMovement(ref Vector2 deltaMovement) {

		// can't move horizontally while climbing
		if (State.IsClimbing()) {
			deltaMovement.x = 0;
			return;
		}

		// check in which direction we're moving
		var movingRight = deltaMovement.x > 0;

		// calculate the properties of the rays
		var rayOrigin = movingRight ? raycastOriginBottomRight : raycastOriginBottomLeft;
		var rayDirection = movingRight ? Vector2.right : -Vector2.right;
		var rayDistance = skinThickness + Mathf.Abs(deltaMovement.x);

		// draw the rays
		for (var i = 0; i < horizontalRays; i++) {

			// calculate the position of the ray
			var rayVector = new Vector2(
				rayOrigin.x,
				rayOrigin.y + (i * verticalRaySpread)
			);

			// visualize the ray
			if (debugMode) {
				Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.green);
			}

			// calculate whether the ray collided with something
			var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, groundLayerMask);

			// if no collisions where found, move on to the next ray
			if (!raycastHit) {
				continue;
			}

			// ??? some stuff for slopes
			if (i == 0 && DetermineHorizontalSlopeMovement(ref deltaMovement, Vector2.Angle(raycastHit.normal, Vector2.up), movingRight)) {
				break;
			}

			// limit the deltamovement by the collision found
			deltaMovement.x = raycastHit.point.x - rayVector.x;

			// limit the ray distanceto this distance as well, since a collision further than this is irrelevant
			rayDistance = Mathf.Abs(deltaMovement.x);

			// correct for skin thickness and set collision states
			if (movingRight) {
				deltaMovement.x -= skinThickness;
				State.collisionRight = true;
			} else {
				deltaMovement.x += skinThickness;
				State.collisionLeft = true;
			}

			// if we're right next to the collision already, break out, we don't need the other rays
			if (rayDistance < skinThickness + 0.0001f) {
				break;
			}

		}

	}

	private void DetermineVerticalMovement(ref Vector2 deltaMovement) {

		var movingUp = deltaMovement.y > 0;

		var rayDistance = skinThickness + Mathf.Abs(deltaMovement.y);
		var rayDirection = movingUp ? Vector2.up : -Vector2.up;
		var rayOrigin = movingUp ? raycastOriginTopLeft : raycastOriginBottomLeft;
		
		rayOrigin.x += deltaMovement.x;
		
		var standingOnDistance = float.MaxValue;

		for (var i = 0; i < verticalRays; i++) {

			var rayVector = new Vector2(
				rayOrigin.x + (i * horizontalRaySpread),
				rayOrigin.y
			);

			if (debugMode) {
				Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);
			}

			RaycastHit2D raycastHit;
			if (movingUp || State.IsClimbing()) {
				raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, groundLayerMask);
			} else {
				raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, groundLayerMask | oneWayPlatformLayerMask);
			}

			if (!raycastHit) {
				continue;
			}

			if (!movingUp) {
				var verticalDistanceToHit = transform.position.y - raycastHit.point.y;
				if (verticalDistanceToHit < standingOnDistance) {
					standingOnDistance = verticalDistanceToHit;
					standingOn = raycastHit.collider.gameObject;
				}
			}

			deltaMovement.y = raycastHit.point.y - rayVector.y;

			rayDistance = Mathf.Abs(deltaMovement.y);

			// correct for skin thickness and set collision states
			if (movingUp) {
				deltaMovement.y -= skinThickness;
				State.collisionAbove = true;
			} else {
				deltaMovement.y += skinThickness;
				State.collisionBelow = true;
			}

			if (!movingUp && deltaMovement.y > 0.001f) {
				State.slopeUp = true;
			}

			if (rayDistance < skinThickness + 0.001f) {
				break;
			}

		}

	}

	private void DetermineGrounding() {
		
		if (standingOn != null) {
			
			activeGlobalPlatformPoint = transform.position;
			activeLocalPlatformPoint = standingOn.transform.InverseTransformPoint(transform.position);
			
			if (previousStandingOn == null) {

				SendMessage("OnLand", SendMessageOptions.DontRequireReceiver);

				if (State.IsClimbing()) {
					StopClimbing();
				}

			}
			
			if (previousStandingOn != standingOn) {

				if (previousStandingOn != null) {
					previousStandingOn.SendMessage("OnCharacterControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
				}

				standingOn.SendMessage("OnCharacterControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
				previousStandingOn = standingOn;

			} else {

				standingOn.SendMessage("OnCharacterControllerStay2D", this, SendMessageOptions.DontRequireReceiver);

			}

		} else if (previousStandingOn != null) {

			previousStandingOn.SendMessage("OnCharacterControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
			previousStandingOn = null;

		}

	}

    ////////////////////////////
    ///// COLLISION EVENTS /////
    ////////////////////////////

    public void OnTriggerEnter2D(Collider2D other)
    {
        other.SendMessage("OnCharacterControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        other.SendMessage("OnCharacterControllerStay2D", this, SendMessageOptions.DontRequireReceiver);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        other.SendMessage("OnCharacterControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
    }

    ////////////////////////////
    ///// MOVING PLATFORMS /////
    ////////////////////////////

    private void HandleMovingPlatforms() {
		
		if (standingOn != null) {
			
			var newGlobalPlatformPoint = standingOn.transform.TransformPoint(activeLocalPlatformPoint);
			var moveDistance = newGlobalPlatformPoint - activeGlobalPlatformPoint;
			
			if (moveDistance != Vector3.zero) {
				transform.Translate(moveDistance, Space.World);
			}
			
			platformVelocity = (newGlobalPlatformPoint - activeGlobalPlatformPoint) / Time.deltaTime;
			
		} else {
			platformVelocity = Vector2.zero;
		}
		
		standingOn = null;
		
	}

	//////////////////
	///// SLOPES /////
	//////////////////

	private bool DetermineHorizontalSlopeMovement(ref Vector2 deltaMovement, float angle, bool movingRight) {

		if (Mathf.RoundToInt(angle) == 90) {
			return false;
		}

		// if the slope is too steep, break out
		if (angle > Parameters.slopeLimit) {
			deltaMovement.x = 0;
			return true;
		}

		// if we're moving up (by some threshold)
		if (deltaMovement.y > 0.07f) {
			return true;
		}

		// correct for skin thickness
		deltaMovement.x += movingRight ? -skinThickness : skinThickness;

		// correct vertical movement
		deltaMovement.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * deltaMovement.x);

		// set state parameters
		State.slopeUp = true;
		State.collisionBelow = true;

		// return that we modified the movement
		return true;

	}

	private void DetermineVerticalSlopeMovement(ref Vector2 deltaMovement) {

		var center = (raycastOriginBottomLeft.x + raycastOriginBottomRight.x) / 2;
		var direction = -Vector2.up;

		var slopeDistance = slopeLimitTangent * (raycastOriginBottomRight.x - center);
		var slopeRayVector = new Vector2(center, raycastOriginBottomLeft.y);

		if (debugMode) {
			Debug.DrawRay(slopeRayVector, direction * slopeDistance, Color.yellow);
		}

		var raycastHit = Physics2D.Raycast(slopeRayVector, direction, slopeDistance, groundLayerMask);
		if (!raycastHit) {
			return;
		}

		var movingDownSlope = Mathf.Sign(raycastHit.normal.x) == Mathf.Sign(deltaMovement.x);
		if (!movingDownSlope) {
			return;
		}

		// check if we're not on something perpendicular to us (why?)
		var angle = Vector2.Angle(raycastHit.normal, Vector2.up);
		if (Mathf.Abs(angle) < 0.001f) {
			return;
		}

		State.slopeDown = true;
		State.slopeAngle = angle;

		deltaMovement.y = raycastHit.point.y - slopeRayVector.y;

	}

}
