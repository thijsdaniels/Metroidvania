using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Objects.Collectables;

[RequireComponent(typeof(CharacterController2D))]

public class Player : MonoBehaviour {

	// components
	public CharacterController2D controller;
    protected Animator animator;
    protected Inventory inventory;

    // button labels
    public string aButtonLabel;
    public string bButtonLabel;
    public string xButtonLabel;

    // listening
    protected bool isListening = true;

    // movement
    private enum Direction {
		right,
		left
	}
	private Direction direction = Direction.right;
	private float movement;
	private float verticalMovement;

	// walking
	public float walkSpeed = 4f;
	[Range(0f, 1f)]
	public float walkAcceleration = 0.6f;

	// running
	private bool running;
	public float runSpeed = 8f;
	[Range(0f, 1f)]
	public float runAcceleration = 0.8f;

	// climbing
	public float climbSpeed = 4f;
	[Range(0f, 1f)]
	public float climbAcceleration = 1f;

	// sound effects
	public AudioClip leftFootSound;
	public AudioClip rightFootSound;
	public AudioClip jumpSound;
	public AudioClip airJumpSound;
	public AudioClip landSound;

	// aiming
	private Vector2 aimingDirection;

	// crosshair
	public Transform crosshair;
	public Vector2 crosshairOffset;
	[Range(0f, 3f)]
	public float crosshairDistance = 1.5f;

	// items
	public Item primaryItem;
    public Item secondaryItem;
    public Item tertiaryItem;

    // checkpoints
    public Checkpoint checkpoint;

	// interaction
	public Interactable interactable;

	// carrying
	private GameObject carrying;

    // equipment
    public enum ItemSlot
    {
        Primary,
        Secondary,
        Tertiary
    }

	//////////////////////
	///// GAME HOOKS /////
	//////////////////////

	public void Start() {

		// reference components
		controller = GetComponent<CharacterController2D>();
		animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();

		// determine the facing direction
		direction = transform.localScale.x > 0f ? Direction.right : Direction.left;

	}

	public void Update()
    {
        // update button labels
        UpdateButtonLabels();
        
		// handle input
		HandleInput();

		// face direction
		FaceDirection();

		// move the player
		Move();

		// carry an object
		Carry();

		// animate the player
		Animate();
	}

	/////////////////
	///// INPUT /////
	/////////////////

    /**
     * @todo Move to wherever the appropriate place is. Probably the HUD.
     */
    protected void UpdateButtonLabels()
    {
        aButtonLabel = controller.CanJump() ? "Jump" : null;
        bButtonLabel = interactable ? interactable.action : null;
        xButtonLabel = "Run";
    }

    public void StartListening()
    {
        isListening = true;
    }

    public void StopListening()
    {
        isListening = false;
    }

    public bool IsListening()
    {
        return isListening;
    }

	private void HandleInput() {

        // skip input if the player isn't listening
        if (!isListening)
        {
            return;
        }

		// horizontal movement
		movement = Input.GetAxis("Horizontal Primary");

		// running
		running = Input.GetButton("Run");

		// vertical movement
		verticalMovement = Input.GetAxis("Vertical Primary");

		// jumping
		if (Input.GetButtonDown("Jump") && controller.CanJump()) {
			controller.Jump();
		}

        // mouse aiming
        /* Disabled because I started using a controller.
		var mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		aimingDirection = new Vector2(
			mousePositionInWorld.x - transform.position.x,
			mousePositionInWorld.y - transform.position.y
		).normalized;
		*/

        Aim(new Vector2(
            (Input.GetAxis("Vertical Secondary") == 0 && Input.GetAxis("Horizontal Secondary") == 0) ? Input.GetAxis("Horizontal Primary") : Input.GetAxis("Horizontal Secondary"),
            (Input.GetAxis("Vertical Secondary") == 0 && Input.GetAxis("Horizontal Secondary") == 0) ? Input.GetAxis("Vertical Primary") : Input.GetAxis("Vertical Secondary")
        ));

		// primary item
        if (primaryItem)
        {
            if (Input.GetButtonDown("Item Primary"))
            {
                primaryItem.OnPress();
            }
            if (Input.GetButton("Item Primary"))
            {
                primaryItem.OnHold();
            }
            if (Input.GetButtonUp("Item Primary"))
            {
                primaryItem.OnRelease();
            }
        }

        // secondary item
        if (secondaryItem)
        {
            if (Input.GetButtonDown("Item Secondary"))
            {
                secondaryItem.OnPress();
            }
            if (Input.GetButton("Item Secondary"))
            {
                secondaryItem.OnHold();
            }
            if (Input.GetButtonUp("Item Secondary"))
            {
                secondaryItem.OnRelease();
            }
        }

        // secondary item
        if (tertiaryItem)
        {
            if (Input.GetButtonDown("Item Tertiary"))
            {
                tertiaryItem.OnPress();
            }
            if (Input.GetButton("Item Tertiary"))
            {
                tertiaryItem.OnHold();
            }
            if (Input.GetButtonUp("Item Tertiary"))
            {
                tertiaryItem.OnRelease();
            }
        }

        // interaction
        if (Input.GetButtonDown("Interact"))
        {
			if (interactable)
            {
				interactable.SendMessage("OnInteraction", this, SendMessageOptions.RequireReceiver);
			}
		}

	}

	//////////////////
	///// FACING /////
	//////////////////

	private void FaceDirection() {

		if (movement > 0f) {
			if (direction != Direction.right) {
				Flip();
			}
		} else if (movement < 0f) {
			if (direction != Direction.left) {
				Flip();
			}
		}

	}

	private void Flip() {

		transform.localScale = new Vector3(
			-transform.localScale.x,
			transform.localScale.y,
			transform.localScale.z
		);

		direction = transform.localScale.x > 0f ? Direction.right : Direction.left;

	}

	//////////////////
	///// MOVING /////
	//////////////////
	
	private void Move() {
		
		var speed = controller.State.IsClimbing() ? climbSpeed : running ? runSpeed : walkSpeed;
		var acceleration = running ? runAcceleration : walkAcceleration;

		acceleration *= controller.Parameters.traction;

		if ((movement > 0f && controller.velocity.x < speed) || (movement < 0f && controller.velocity.x > -speed)) {
			controller.AddHorizontalVelocity(
				movement * acceleration // * Time.deltaTime // (removed because I heavily decreased the acceleration parameter).
			);
		}
		
		if (controller.State.IsClimbing()) {
			controller.SetVerticalVelocity(verticalMovement * climbSpeed);
		}
		
	}

	//////////////////
	///// AIMING /////
	//////////////////

	private void Aim(Vector2 input) {

		// controller aiming
		aimingDirection = input.normalized;
		
		// controller aiming deadzone
		if (aimingDirection.magnitude < 0.19f) {
			aimingDirection = new Vector2(
				transform.localScale.x,
				0f
				).normalized;
		}

		PositionCrosshair();

	}

	private void PositionCrosshair() {

		crosshair.position = transform.position + new Vector3(
			crosshairOffset.x + crosshairDistance * aimingDirection.x,
			crosshairOffset.y + crosshairDistance * aimingDirection.y,
			transform.position.z
		);

	}

	public Vector2 GetAim() {
		return this.aimingDirection;
	}

	/////////////////////
	///// ANIMATION /////
	/////////////////////

	private void Animate() {
		
		// set gounded animation parameter
		animator.SetBool("Grounded", controller.State.IsGrounded());
		
		// set moving animation parameter
		animator.SetBool("Moving", Mathf.Abs(movement) > 0.1f);
		
		// set the vertical speed animation parameter
		animator.SetFloat("Vertical Speed", controller.velocity.y);
		
		// set gounded animation parameter
		animator.SetBool("Running", running);

		// set climbing animation parameter
		animator.SetBool("Climbing", controller.State.IsClimbing());
		
	}

	/////////////////////////
	///// SOUND EFFECTS /////
	/////////////////////////

	public void OnJump() {
		if (jumpSound) {
			AudioSource.PlayClipAtPoint(jumpSound, transform.position);
		}
	}

	public void OnAirJump() {
		if (airJumpSound) {
			AudioSource.PlayClipAtPoint(airJumpSound, transform.position);
		}
	}

	public void OnLand() {
		if (landSound) {
			AudioSource.PlayClipAtPoint(landSound, transform.position);
		}
	}

	public void OnLeftFootstep() {
		if (leftFootSound) {
			AudioSource.PlayClipAtPoint(leftFootSound, transform.position);
		}
	}
	
	public void OnRightFootstep() {
		if (rightFootSound) {
			AudioSource.PlayClipAtPoint(rightFootSound, transform.position);
		}
	}

	/////////////////
	///// DEATH /////
	/////////////////

	public void OnDeath(Damagable damagable) {

		if (checkpoint && checkpoint.active) {

			checkpoint.Respawn(this);

			damagable.Revive();

		} else {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

	}

	////////////////////
	///// CARRYING /////
	////////////////////

	public void Grab(GameObject carriable)
	{
		var body = carriable.GetComponent<Rigidbody2D>();
		if (body) {
			body.isKinematic = true;
		}

		carrying = carriable;
	}

	public void Carry()
	{
		if (!carrying) {
			return;
		}

		carrying.transform.position = transform.position;
	}

	public GameObject Release()
	{
		var carriable = carrying;
		carrying = null;

		var body = carriable.GetComponent<Rigidbody2D>();
		if (body) {
			body.isKinematic = false;
		}

		return carriable;
	}

	public void Drop()
	{
		var carriable = Release();

		carriable.transform.position = new Vector3(
			transform.position.x,
			transform.position.y - transform.localScale.y / 2,
			transform.position.z
		);
	}

	public void Throw(Vector2 force)
	{
		var carriable = Release();

		var body = carriable.GetComponent<Rigidbody2D>();
		if (body)
        {
			body.AddForce(force);
            return;
		}
	}

    /////////////////////
    ///// EQUIPMENT /////
    /////////////////////

    public void Equip(ItemSlot slot, Item item)
    {
        switch (slot)
        {
            case ItemSlot.Primary:
                primaryItem = item;
                break;

            case ItemSlot.Secondary:
                secondaryItem = item;
                break;

            case ItemSlot.Tertiary:
                tertiaryItem = item;
                break;
        }
    }

}
