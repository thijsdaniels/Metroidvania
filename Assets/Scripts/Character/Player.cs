using System;
using Objects;
using Objects.Collectables;
using Traits;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Character
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(CharacterController2D))]
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// Components.
        /// </summary>
        public CharacterController2D Controller;
        protected Animator Animator;
        protected Inventory Inventory;

        /// <summary>
        /// Button labels.
        /// </summary>
        public string AButtonLabel { get; private set; }
        public string BButtonLabel { get; private set; }

        /// <summary>
        /// Listening.
        /// </summary>
        protected bool Listening = true;

        /// <summary>
        /// Movements.
        /// </summary>
        private enum Directions
        {
            Right,
            Left
        }
        private Directions Direction = Directions.Right;
        private float HorizontalMovement;
        private float VerticalMovement;

        /// <summary>
        /// Running
        /// </summary>
        public float RunSpeed = 8f;

        /// <summary>
        /// Rolling
        /// </summary>
        public float RollSpeed = 12f;

        /// <summary>
        /// Swimming.
        /// </summary>
        public Vector2 SwimSpeed = new Vector2(6f, 4f);

        /// <summary>
        /// Climbing
        /// </summary>
        private float ClimbingThreshold = 0.5f;
        public float ClimbSpeed = 4f;

        /// <summary>
        /// Crouching
        /// </summary>
        private float CrouchingThreshold = 0.5f;

        /// <summary>
        /// Sound effects.
        /// </summary>
        public AudioClip LeftFootSound;
        public AudioClip RightFootSound;
        public AudioClip JumpSound;
        public AudioClip AirJumpSound;
        public AudioClip WallJumpSound;
        public AudioClip LandSound;
        public AudioClip LeftClimbSound;
        public AudioClip RightClimbSound;

        /// <summary>
        /// Aiming.
        /// </summary>
        private Vector2 AimingDirection;
        private float AimingTimeScale = 0.1f;

        /// <summary>
        /// Crosshair.
        /// </summary>
        public Transform Crosshair;
        public Vector2 CrosshairOffset;
        [Range(0f, 3f)] public float CrosshairDistance = 1.5f;

        /// <summary>
        /// Items.
        /// </summary>
        public Item PrimaryItem;
        public Item SecondaryItem;
        public Item TertiaryItem;
        public Item QuaternaryItem;

        /// <summary>
        /// Checkpoints.
        /// </summary>
        public Checkpoint Checkpoint;

        /// <summary>
        /// Interaction.
        /// </summary>
        public Interactable Interactable;

        /// <summary>
        /// Carrying.
        /// </summary>
        private GameObject Carrying;
        
        /// <summary>
        /// Equipment.
        /// </summary>
        public enum ItemSlots
        {
            Primary,
            Secondary,
            Tertiary,
            Quaternary
        }

        //////////////////////
        ///// GAME HOOKS /////
        //////////////////////

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            // reference components
            Controller = GetComponent<CharacterController2D>();
            Animator = GetComponent<Animator>();
            Inventory = GetComponent<Inventory>();

            // determine the facing direction
            Direction = transform.localScale.x > 0f ? Directions.Right : Directions.Left;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            // update button labels
            UpdateButtonLabels();

            // reset input
            ResetInput();

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

        // TODO: Move to wherever the appropriate place is. Probably the HUD.
        
        /// <summary>
        /// 
        /// </summary>
        protected void UpdateButtonLabels()
        {
            AButtonLabel = Controller.CanJump() ?
                Controller.State.IsSwimming() ?
                    "Swim" :
                    "Jump" :
                null;
            
            BButtonLabel = Interactable ?
                Interactable.Action :
                Controller.CanRoll() ? 
                    "Roll" :
                    null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartListening()
        {
            Listening = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopListening()
        {
            Listening = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsListening()
        {
            return Listening;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetInput()
        {
            HorizontalMovement = VerticalMovement = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        private void HandleInput()
        {
            // skip input if the player isn't listening
            if (!Listening)
            {
                return;
            }

            // horizontal movement
            HorizontalMovement = Input.GetAxis("Horizontal Primary");

            // vertical movement
            VerticalMovement = Input.GetAxis("Vertical Primary");
            
            // crouching
            if (VerticalMovement < -CrouchingThreshold && Controller.CanCrouch())
            {
                StartCrouching();
            }
            else
            {
                StopCrouching();
            }

            // jumping
            if (Input.GetButtonDown("Jump") && Controller.CanJump())
            {
                Controller.Jump();
            }
            
            // climbing
            if (Mathf.Abs(VerticalMovement) > ClimbingThreshold && Controller.CanClimb())
            {
                Controller.StartClimbing();
            }

            // aiming
            SetAimingDirection(new Vector2(
                (Input.GetAxis("Horizontal Primary") + Input.GetAxis("Horizontal Secondary")) / 2,
                (Input.GetAxis("Vertical Primary") + Input.GetAxis("Vertical Secondary")) / 2
            ));

            // primary item
            if (PrimaryItem)
            {
                if (Input.GetButtonDown("Item Primary"))
                {
                    PrimaryItem.OnPress();
                }

                if (Input.GetButton("Item Primary"))
                {
                    PrimaryItem.OnHold();
                }

                if (Input.GetButtonUp("Item Primary"))
                {
                    PrimaryItem.OnRelease();
                }
            }

            // secondary item
            if (SecondaryItem)
            {
                if (Input.GetButtonDown("Item Secondary"))
                {
                    SecondaryItem.OnPress();
                }

                if (Input.GetButton("Item Secondary"))
                {
                    SecondaryItem.OnHold();
                }

                if (Input.GetButtonUp("Item Secondary"))
                {
                    SecondaryItem.OnRelease();
                }
            }

            // tertiary item
            if (TertiaryItem)
            {
                if (Input.GetButtonDown("Item Tertiary"))
                {
                    TertiaryItem.OnPress();
                }

                if (Input.GetButton("Item Tertiary"))
                {
                    TertiaryItem.OnHold();
                }

                if (Input.GetButtonUp("Item Tertiary"))
                {
                    TertiaryItem.OnRelease();
                }
            }

            // quaternary item
            if (QuaternaryItem)
            {
                if (Input.GetButtonDown("Item Quaternary"))
                {
                    QuaternaryItem.OnPress();
                }

                if (Input.GetButton("Item Quaternary"))
                {
                    QuaternaryItem.OnHold();
                }

                if (Input.GetButtonUp("Item Quaternary"))
                {
                    QuaternaryItem.OnRelease();
                }
            }

            // interaction
            if (Input.GetButtonDown("Interact"))
            {
                if (Interactable)
                {
                    Interactable.SendMessage("OnInteraction", this, SendMessageOptions.RequireReceiver);
                }
                else if (Controller.CanRoll())
                {
                    Controller.Roll();
                }
            }
        }

        //////////////////
        ///// FACING /////
        //////////////////

        /// <summary>
        /// 
        /// </summary>
        private void FaceDirection()
        {
            if (HorizontalMovement > 0f)
            {
                if (Direction != Directions.Right)
                {
                    Flip();
                }
            }
            else if (HorizontalMovement < 0f)
            {
                if (Direction != Directions.Left)
                {
                    Flip();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Flip()
        {
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z
            );

            Direction = transform.localScale.x > 0f ? Directions.Right : Directions.Left;
        }

        //////////////////
        ///// MOVING /////
        //////////////////

        /// <summary>
        /// 
        /// </summary>
        private void Move()
        {
            if (Controller.State.IsAiming())
            {
                MoveWhileAiming();
                return;
            }
            
            if (Controller.State.IsSwimming())
            {
                MoveWhileSwimming();
                return;
            }
            
            if (Controller.State.IsClimbing())
            {
                MoveWhileClimbing();
                return;
            }
            
            if (Controller.State.IsCrouching())
            {
                MoveWhileCrouching();
                return;
            }
            
            if (Controller.State.IsRolling())
            {
                MoveWhileRolling();
                return;
            }

            if (Controller.State.IsGrounded())
            {
                MoveWhileWalking();
                return;
            }
            
            MoveWhileAirborne();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveWhileAiming()
        {
            MoveHorizontallyWhileAiming();
            MoveVerticallyWhileAiming();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveHorizontallyWhileAiming()
        {
            // No horizontal movement.
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveVerticallyWhileAiming()
        {
            // No vertical movement.
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveWhileSwimming()
        {
            MoveHorizontallyWhileSwimming();
            MoveVerticallyWhileSwimming();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveHorizontallyWhileSwimming()
        {
            float acceleration = SwimSpeed.x; // * Controller.PlatformParameters.Traction; // * Time.deltaTime;

            if (HorizontalMovement > 0f && Controller.Velocity.x < SwimSpeed.x || HorizontalMovement < 0f && Controller.Velocity.x > -SwimSpeed.x)
            {
                Controller.AddHorizontalVelocity(HorizontalMovement * acceleration);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveVerticallyWhileSwimming()
        {
            float acceleration = SwimSpeed.y; // * Controller.PlatformParameters.Traction; // * Time.deltaTime;

            if (VerticalMovement > 0f && Controller.Velocity.y < SwimSpeed.y || VerticalMovement < 0f && Controller.Velocity.y > -SwimSpeed.y)
            {
                Controller.AddVerticalVelocity(VerticalMovement * acceleration);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveWhileClimbing()
        {
            MoveHorizontallyWhileClimbing();
            MoveVerticallyWhileClimbing();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveHorizontallyWhileClimbing()
        {
            // No horizontal movement.
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveVerticallyWhileClimbing()
        {
            Controller.SetVerticalVelocity(VerticalMovement * ClimbSpeed);
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void MoveWhileCrouching()
        {
            MoveHorizontallyWhileCrouching();
            MoveVerticallyWhileCrouching();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveHorizontallyWhileCrouching()
        {
            // No horizontal movement.
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveVerticallyWhileCrouching()
        {
            // No vertical movement. 
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveWhileRolling()
        {
            MoveHorizontallyWhileRolling();
            MoveVerticallyWhileRolling();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveHorizontallyWhileRolling()
        {
            if (!(Controller.Velocity.x < RollSpeed) || !(Controller.Velocity.x > -RollSpeed))
            {
                return;
            }

            float directionCoefficient = transform.localScale.x > 0f ? 1f : -1f;
            
            Controller.SetHorizontalVelocity(RollSpeed * directionCoefficient);
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveVerticallyWhileRolling()
        {
            // No vertical movement.
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveWhileWalking()
        {
            MoveHorizontallyWhileWalking();
            MoveVerticallyWhileWalking();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveHorizontallyWhileWalking()
        {
            float walkAcceleration = RunSpeed * Controller.PlatformParameters.Traction; // * Time.deltaTime;

            if (HorizontalMovement > 0f && Controller.Velocity.x < RunSpeed || HorizontalMovement < 0f && Controller.Velocity.x > -RunSpeed)
            {
                Controller.AddHorizontalVelocity(HorizontalMovement * walkAcceleration);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveVerticallyWhileWalking()
        {
            // No vertical movement.
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveWhileAirborne()
        {
            MoveHorizontallyWhileAirborne();
            MoveVerticallyWhileAirborne();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveHorizontallyWhileAirborne()
        {
            float flyAcceleration = RunSpeed * Controller.PlatformParameters.Traction; // * Time.deltaTime;

            if (HorizontalMovement > 0f && Controller.Velocity.x < RunSpeed || HorizontalMovement < 0f && Controller.Velocity.x > -RunSpeed)
            {
                Controller.AddHorizontalVelocity(HorizontalMovement * flyAcceleration);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveVerticallyWhileAirborne()
        {
            // No vertical movement.
        }

        //////////////////
        ///// AIMING /////
        //////////////////

        /// <summary>
        /// 
        /// </summary>
        public void StartAiming()
        {
            if (Controller.State.Aiming)
            {
                return;
            }

            Controller.State.Aiming = true;

            Crosshair.GetComponent<SpriteRenderer>().enabled = true;
            
            Time.timeScale *= AimingTimeScale;
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopAiming()
        {
            if (!Controller.State.Aiming)
            {
                return;
            }

            Controller.State.Aiming = false;

            Crosshair.GetComponent<SpriteRenderer>().enabled = false;
            
            Time.timeScale *= 1 / AimingTimeScale;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Vector2 GetAimingDirection()
        {
            return AimingDirection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        private void SetAimingDirection(Vector2 input)
        {
            // controller aiming
            AimingDirection = input.normalized;

            // controller aiming deadzone
            if (AimingDirection.magnitude < 0.19f)
            {
                AimingDirection = new Vector2(
                    transform.localScale.x,
                    0f
                ).normalized;
            }

            PositionCrosshair();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PositionCrosshair()
        {
            Crosshair.position = transform.position + new Vector3(
                CrosshairOffset.x + CrosshairDistance * AimingDirection.x,
                CrosshairOffset.y + CrosshairDistance * AimingDirection.y,
                transform.position.z
            );
        }

        /////////////////////
        ///// ANIMATION /////
        /////////////////////

        /// <summary>
        /// 
        /// </summary>
        private void Animate()
        {
            // set gounded animation parameter
            Animator.SetBool("Grounded", Controller.State.IsGrounded());

            // set moving animation parameter
            Animator.SetBool("Moving", (
                Controller.State.IsGrounded() &&
                !Controller.State.IsAiming() &&
                !Controller.State.IsCrouching() &&
                Mathf.Abs(HorizontalMovement) > 0f
            ));

            // set the movement animation parameters
            Animator.SetFloat("Horizontal Movement", HorizontalMovement);
            Animator.SetFloat("Vertical Movement", VerticalMovement);
            
            // set the velocity animation parameters
            Animator.SetFloat("Horizontal Velocity", Controller.Velocity.x);
            Animator.SetFloat("Vertical Velocity", Controller.Velocity.y);

            // set climbing animation parameter
            Animator.SetBool("Climbing", Controller.State.IsClimbing());
        }

        /////////////////////////
        ///// SOUND EFFECTS /////
        /////////////////////////

        /// <summary>
        /// 
        /// </summary>
        public void OnJump()
        {
            if (JumpSound)
            {
                AudioSource.PlayClipAtPoint(JumpSound, transform.position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnAirJump()
        {
            if (AirJumpSound)
            {
                AudioSource.PlayClipAtPoint(AirJumpSound, transform.position);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void OnWallJump()
        {
            if (WallJumpSound)
            {
                AudioSource.PlayClipAtPoint(WallJumpSound, transform.position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnLand()
        {
            if (LandSound)
            {
                AudioSource.PlayClipAtPoint(LandSound, transform.position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnLeftFootstep()
        {
            if (LeftFootSound)
            {
                AudioSource.PlayClipAtPoint(LeftFootSound, transform.position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnRightFootstep()
        {
            if (RightFootSound)
            {
                AudioSource.PlayClipAtPoint(RightFootSound, transform.position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnLeftClimb()
        {
            if (LeftClimbSound)
            {
                AudioSource.PlayClipAtPoint(LeftClimbSound, transform.position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnRightClimb()
        {
            if (RightClimbSound)
            {
                AudioSource.PlayClipAtPoint(RightClimbSound, transform.position);
            }
        }

        /////////////////
        ///// DEATH /////
        /////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <param name="damagable"></param>
        public void OnDeath(Damagable damagable)
        {
            if (Checkpoint && Checkpoint.Active)
            {
                Checkpoint.Respawn(this);

                damagable.Revive();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        ////////////////////
        ///// CARRYING /////
        ////////////////////

        // TODO: Move this to a "Carrier" trait.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carriable"></param>
        public void Grab(GameObject carriable)
        {
            var body = carriable.GetComponent<Rigidbody2D>();
            if (body)
            {
                body.isKinematic = true;
            }

            Carrying = carriable;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Carry()
        {
            if (!Carrying)
            {
                return;
            }

            Carrying.transform.position = transform.position;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GameObject Release()
        {
            var carriable = Carrying;
            Carrying = null;

            var body = carriable.GetComponent<Rigidbody2D>();
            if (body)
            {
                body.isKinematic = false;
            }

            return carriable;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Drop()
        {
            var carriable = Release();

            carriable.transform.position = new Vector3(
                transform.position.x,
                transform.position.y - transform.localScale.y / 2,
                transform.position.z
            );
        }

        ////////////////////
        ///// THROWING /////
        ////////////////////

        // TODO: Move this to a "Thrower" trait (requires Carrier trait).

        /// <summary>
        /// 
        /// </summary>
        /// <param name="force"></param>
        public void Throw(Vector2 force)
        {
            GameObject carriable = Release();

            Rigidbody2D body = carriable.GetComponent<Rigidbody2D>();
            
            if (body)
            {
                body.AddForce(force);
            }
        }

        /////////////////////
        ///// EQUIPMENT /////
        /////////////////////

        // TODO: Move this to an "Equipper" trait.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="item"></param>
        public void Equip(ItemSlots slot, Item item)
        {
            switch (slot)
            {
                case ItemSlots.Primary:
                {
                    PrimaryItem = item;
                    break;
                }
                case ItemSlots.Secondary:
                {
                    SecondaryItem = item;
                    break;
                }
                case ItemSlots.Tertiary:
                {
                    TertiaryItem = item;
                    break;
                }
                case ItemSlots.Quaternary:
                {
                    QuaternaryItem = item;
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, "Invalid slot.");
                }
            }
        }
        
        ///////////////////
        ///// DUCKING /////
        ///////////////////

        /// <summary>
        /// 
        /// </summary>
        public void StartCrouching()
        {
            Controller.State.Crouching = true;
            
            Animator.SetBool("Crouching", true);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void StopCrouching()
        {
            Controller.State.Crouching = false;
            
            Animator.SetBool("Crouching", false);
        }
    }
}