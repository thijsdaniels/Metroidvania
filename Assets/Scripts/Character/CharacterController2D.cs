using System;
using Physics;
using Traits;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class CharacterController2D : MonoBehaviour
    {
        /// <summary>
        /// Debug
        /// </summary>
        public bool DebugMode;

        /// <summary>
        /// Raycasting
        /// </summary>
        [Range(0f, 0.1f)] public float SkinThickness = 0.05f;
        [Range(0, 20)] public int HorizontalRays = 12;
        [Range(0, 10)] public int VerticalRays = 6;
        private float HorizontalRaySpread;
        private float VerticalRaySpread;
        private Vector3 RaycastOriginTopLeft;
        private Vector3 RaycastOriginBottomLeft;
        private Vector3 RaycastOriginBottomRight;

        /// <summary>
        /// Collision Detection
        /// </summary>
        public bool HandleCollisions = true;
        private BoxCollider2D BoxCollider;
        public LayerMask GroundLayerMask;
        public LayerMask OneWayPlatformLayerMask;

        /// <summary>
        /// Platforms
        /// </summary>
        public PhysicsPlatform2D Platform { private get; set; }
        private PhysicsPlatform2D PreviousPlatform;
        private Vector3 ActiveGlobalPlatformPoint;
        private Vector3 ActiveLocalPlatformPoint;
        private Vector2 PlatformVelocity;
        public PhysicsPlatformParameters2D DefaultPlatformParameters;
        public PhysicsPlatformParameters2D PlatformParameters => Platform ? Platform.Parameters : DefaultPlatformParameters;

        /// <summary>
        /// Volumes
        /// </summary>
        public PhysicsVolume2D Volume { private get; set; }
        private PhysicsVolume2D PreviousVolume;
        public PhysicsVolumeParameters2D DefaultVolumeParameters;
        public PhysicsVolumeParameters2D VolumeParameters => Volume ? Volume.Parameters : DefaultVolumeParameters;

        /// <summary>
        /// Movement
        /// </summary>
        public Vector2 Velocity;
        private Vector2 ExternalVelocity;
        private float FallMultiplier => 2f;
        private float LowJumpMultiplier => 4f;
        private float SlopeLimitTangent = Mathf.Tan(75f * Mathf.Deg2Rad);

        /// <summary>
        /// Jumping
        /// </summary>
        [Range(0f, 20f)] public float JumpVelocity = 12f;
        [Range(0f, 1f)] public float JumpCooldown = 0.25f;
        private float JumpTimeout;
        public int AirJumps;
        private int CurrentAirJump;
        
        /// <summary>
        /// Wall Jump.
        /// </summary>
        public bool WallJumpEnabled;
        public Vector2 WallJumpVelocity = new Vector2(16f, 12f);
        public float WallFriction = 0.2f;

        /// <summary>
        /// Climbing
        /// </summary>
        public int Climbables;
        public Climbable Climbable;

        /// <summary>
        /// State
        /// </summary>
        public CharacterState2D State = new CharacterState2D();

        //////////////////////
        ///// GAME HOOKS /////
        //////////////////////

        /// <summary>
        /// 
        /// </summary>
        public void Awake()
        {
            // reference components
            BoxCollider = GetComponent<BoxCollider2D>();

            // calculate distance between rays
            CalculateRaySpread();
        }

        /// <summary>
        /// 
        /// </summary>
        public void LateUpdate()
        {
            // handle movement
            Vector2 combinedVelocity = Velocity + ExternalVelocity;
            Vector2 deltaMovement = DetermineMovement(combinedVelocity * Time.deltaTime);
            Move(deltaMovement);
            RestrictVelocity(deltaMovement, ExternalVelocity * Time.deltaTime);

            // apply gravity
            ApplyGravity();

            // apply friction
            ApplyFriction();

            // apply damping
            ApplyDamping();

            // determine what platform we're on
            DeterminePlatform();
            
            // determine what volume we're in
            DetermineVolume();

            // run jump timer
            if (JumpTimeout > 0)
            {
                JumpTimeout = Mathf.Max(0, JumpTimeout - Time.deltaTime);
            }
        }

        /////////////////////////////
        ///// VELOCITY MUTATORS /////
        /////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity"></param>
        public void AddVelocity(Vector2 velocity)
        {
            Velocity += velocity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity"></param>
        public void AddHorizontalVelocity(float velocity)
        {
            Velocity.x += velocity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity"></param>
        public void AddVerticalVelocity(float velocity)
        {
            Velocity.y += velocity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity"></param>
        public void SetVelocity(Vector2 velocity)
        {
            Velocity = velocity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity"></param>
        public void SetHorizontalVelocity(float velocity)
        {
            Velocity.x = velocity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity"></param>
        public void SetVerticalVelocity(float velocity)
        {
            Velocity.y = velocity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity"></param>
        public void AddExternalHorizontalVelocity(float velocity)
        {
            ExternalVelocity.x = velocity;
        }

        ///////////////////
        ///// PHYSICS /////
        ///////////////////

        /// <summary>
        /// 
        /// </summary>
        private void ApplyGravity()
        {
            // bypass gravity when climbing
            if (State.IsClimbing())
            {
                return;
            }

            // add gravity to the character's velocity
            AddVerticalVelocity(VolumeParameters.Gravity * GetGravityMultiplier() * Time.deltaTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private float GetGravityMultiplier()
        {
            if (Velocity.y < 0)
            {
                return FallMultiplier;
            }
            
            if (Velocity.y > 0 && !Input.GetButton("Jump"))
            {
                return LowJumpMultiplier;
            }
            
            return 1f;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ApplyFriction()
        {
            if (State.IsGrounded())
            {
                SetHorizontalVelocity(Velocity.x * (1 - PlatformParameters.Friction));
            }
            
            if (!State.IsSwimming() && State.IsCollidingHorizontally() && Velocity.y < 0)
            {
                SetVerticalVelocity(Velocity.y * (1 - WallFriction));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ApplyDamping()
        {
            SetVelocity(Velocity * (1 - VolumeParameters.Damping));
        }

        //////////////////////
        ///// RAYCASTING /////
        //////////////////////

        /// <summary>
        /// 
        /// </summary>
        private void CalculateRaySpread()
        {
            // calculate horizontal spread of (vertical) rays
            float colliderWidth = BoxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2 * SkinThickness);
            HorizontalRaySpread = colliderWidth / (VerticalRays - 1);

            // calculate vertical spread of (horizontal) rays
            float colliderHeight = BoxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinThickness);
            VerticalRaySpread = colliderHeight / (HorizontalRays - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalculateRayOrigins()
        {
            Vector2 size = new Vector2(
                BoxCollider.size.x * Mathf.Abs(transform.localScale.x),
                BoxCollider.size.y * Mathf.Abs(transform.localScale.y)
            );

            Vector2 center = new Vector2(
                BoxCollider.offset.x * transform.localScale.x,
                BoxCollider.offset.y * transform.localScale.y
            );

            RaycastOriginTopLeft = transform.position + new Vector3(
                center.x - size.x / 2 + SkinThickness,
                center.y + size.y / 2 - SkinThickness
            );

            RaycastOriginBottomLeft = transform.position + new Vector3(
                center.x - size.x / 2 + SkinThickness,
                center.y - size.y / 2 + SkinThickness
            );

            RaycastOriginBottomRight = transform.position + new Vector3(
                center.x + size.x / 2 - SkinThickness,
                center.y - size.y / 2 + SkinThickness
            );
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnDrawGizmos()
        {
            if (!DebugMode)
            {
                return;
            }

            Gizmos.DrawWireSphere(new Vector3(
                RaycastOriginTopLeft.x,
                RaycastOriginTopLeft.y,
                0
            ), SkinThickness);

            Gizmos.DrawWireSphere(new Vector3(
                RaycastOriginBottomLeft.x,
                RaycastOriginBottomLeft.y,
                0
            ), SkinThickness);

            Gizmos.DrawWireSphere(new Vector3(
                RaycastOriginBottomRight.x,
                RaycastOriginBottomRight.y,
                0
            ), SkinThickness);
        }

        ///////////////////
        ///// JUMPING /////
        ///////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool CanJump()
        {
            if (State.IsClimbing())
            {
                return false;
            }
            
            if (JumpTimeout > 0)
            {
                return false;
            }

            switch (VolumeParameters.JumpMode)
            {
                case PhysicsVolumeParameters2D.JumpModes.Anywhere:
                {
                    return true;
                }
                case PhysicsVolumeParameters2D.JumpModes.Ground:
                {
                    return (
                        State.IsGrounded() ||
                        CurrentAirJump < AirJumps ||
                        WallJumpEnabled && State.IsCollidingHorizontally()
                    );
                }
                case PhysicsVolumeParameters2D.JumpModes.None:
                {
                    return false;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(PhysicsVolumeParameters2D.JumpMode), VolumeParameters.JumpMode.ToString(), "Invalid JumpMode.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Jump()
        {
            if (State.IsClimbing())
            {
                StopClimbing();
            }

            if (State.IsGrounded())
            {
                SendMessage("OnJump", SendMessageOptions.DontRequireReceiver);

                SetVerticalVelocity(JumpVelocity * VolumeParameters.JumpModifier);
            }
            else
            {
                if (!State.IsSwimming() && State.IsCollidingHorizontally())
                {
                    SendMessage("OnWallJump", SendMessageOptions.DontRequireReceiver);

                    int direction = State.CollisionLeft ? 1 : -1;
            
                    SetVelocity(new Vector2(
                        WallJumpVelocity.x * VolumeParameters.JumpModifier * direction,
                        WallJumpVelocity.y * VolumeParameters.JumpModifier
                    ));
                }
                else
                {
                    if (VolumeParameters.JumpMode == PhysicsVolumeParameters2D.JumpModes.Ground)
                    {
                        CurrentAirJump++;
                    }

                    SendMessage("OnAirJump", SendMessageOptions.DontRequireReceiver);
                    
                    SetVerticalVelocity(JumpVelocity * VolumeParameters.JumpModifier);
                }
            }

            JumpTimeout = JumpCooldown;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void OnLand()
        {
            CurrentAirJump = 0;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void OnStartSwimming()
        {
            CurrentAirJump = 0;
        }

        ///////////////////
        ///// ROLLING /////
        ///////////////////

        // TODO: Move to the Player, or to a "Roller" trait.

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanRoll()
        {
            return (
                !State.IsRolling() &&
                State.IsGrounded() &&
                !State.IsClimbing() &&
                !State.IsSwimming()
            ); // TODO: Maybe just let Mecanim decide when it's OK to roll?
        }

        /// <summary>
        /// 
        /// </summary>
        public void Roll()
        {
            Animator animator = GetComponent<Animator>();

            if (!animator)
            {
                Debug.LogError("Cannot roll without an Animator component.");
                return;
            }

            animator.SetTrigger("Roll");
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnRollStart()
        {
            State.Rolling = true;

            Damagable damagable = GetComponent<Damagable>();

            if (damagable)
            {
                damagable.Dodging = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnRollEnd()
        {
            State.Rolling = false;

            Damagable damagable = GetComponent<Damagable>();

            if (damagable)
            {
                damagable.Dodging = false;
            }
        }

        ////////////////////
        ///// CLIMBING /////
        ////////////////////

        // TODO: Move to the Player, or to a "Climber" trait.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="climbable"></param>
        public void AddClimbable(Climbable climbable)
        {
            Climbables++;

            Climbable = climbable;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveClimbable()
        {
            if (Climbables > 0)
            {
                Climbables--;
            }

            if (Climbables == 0 && State.IsClimbing())
            {
                StopClimbing();

                Climbable = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanClimb()
        {
            return Climbables > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartClimbing()
        {
            State.Climbing = true;

            SetHorizontalVelocity(0);

            CurrentAirJump = 0;

            transform.position = new Vector3(
                Climbable.transform.position.x + Climbable.Offset,
                transform.position.y,
                transform.position.z
            );
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopClimbing()
        {
            State.Climbing = false;

            SetVerticalVelocity(0);
        }
        
        ////////////////////
        ///// SWIMMING /////
        ////////////////////

        // TODO: Move to the Player, or to a "Swimmer" trait.
        
        /// <summary>
        /// 
        /// </summary>
        public void StartSwimming()
        {
            if (State.IsClimbing())
            {
                StopClimbing();
            }
            
            State.Swimming = true;
            
            SendMessage("OnStartSwimming", SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopSwimming()
        {
            State.Swimming = false;
            
            SendMessage("OnStopSwimming", SendMessageOptions.DontRequireReceiver);
        }

        /////////////////////
        ///// CROUCHING /////
        /////////////////////
        
        // TODO: Move to the Player, or to a "Croucher" trait.
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCrouch()
        {
            return (
                !State.IsSwimming() &&
                !State.IsClimbing() &&
                !State.IsAttacking() &&
                !State.IsAiming()
            );
        }

        //////////////////
        ///// MOVING /////
        //////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaMovement"></param>
        /// <returns></returns>
        /// TODO: Abstract parts of this method to make it more readable.
        private Vector2 DetermineMovement(Vector2 deltaMovement)
        {
            // remember whether the character is grounded
            bool wasGrounded = State.IsGrounded();

            // then reset the state
            State.Reset();

            // handle collisions unless otherwise requested
            if (HandleCollisions)
            {
                // handle moving platforms
                HandleMovingPlatforms();

                // calculate from where to cast rays
                CalculateRayOrigins();

                // handle slopes
                if (deltaMovement.y < 0 && wasGrounded)
                {
                    DetermineVerticalSlopeMovement(ref deltaMovement);
                }

                // handle horizontal movement
                if (Mathf.Abs(deltaMovement.x) > 0.001f)
                {
                    DetermineHorizontalMovement(ref deltaMovement);
                }

                // handle vertical movement
                DetermineVerticalMovement(ref deltaMovement);

                // move if a collider is penetrating the skin
                CorrectHorizontalPlacement(ref deltaMovement, true);
                CorrectHorizontalPlacement(ref deltaMovement, false);
            }

            return deltaMovement;
        }

        /// <summary>
        /// Move the character.
        /// </summary>
        /// <param name="deltaMovement"></param>
        private void Move(Vector2 deltaMovement)
        {
            transform.Translate(deltaMovement, Space.World);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaMovement"></param>
        /// <param name="externalMovement"></param>
        private void RestrictVelocity(Vector2 deltaMovement, Vector2 externalMovement)
        {
            // subtract the external movement, because we only care about the character's movement
            deltaMovement -= externalMovement;

            // update the player's velocity to whatever it was limited to
            if (Time.deltaTime != 0)
            {
                Velocity = deltaMovement / Time.deltaTime;
            }
            else
            {
                Velocity = deltaMovement; //TODO: is this correct? experiment with pausing the game
            }

            // clamp the velocity to the volume's terminal velocity
            Velocity = new Vector2(
                Mathf.Min(Mathf.Max(Velocity.x, -VolumeParameters.MaximumVelocity.x), VolumeParameters.MaximumVelocity.x),
                Mathf.Min(Mathf.Max(Velocity.y, -VolumeParameters.MaximumVelocity.y), VolumeParameters.MaximumVelocity.y)
            );

            // correct velocity while moving up slopes
            // TODO: Does it make sense that this happens here? Maybe this should be moved somewhere else.
            if (State.SlopeUp)
            {
                Velocity.y = 0;
            }

            // clear the external velocity
            // TODO: Does it make sense that this happens here? Maybe this should be moved somewhere else.
            ExternalVelocity = Vector2.zero;
        }

        ///////////////////////////////
        ///// COLLISION DETECTION /////
        ///////////////////////////////

        /// <summary>
        /// Using additional inner rays, checks if a solid object is penetrating the character and if so, moves the
        /// character away from the object horizontally. This makes the player get pushed to the side by, for example,
        /// moving platforms.
        /// </summary>
        /// <param name="deltaMovement"></param>
        /// <param name="isRight"></param>
        private void CorrectHorizontalPlacement(ref Vector2 deltaMovement, bool isRight)
        {
            float halfWidth = (BoxCollider.size.x * Mathf.Abs(transform.localScale.x)) * 0.5f;
            Vector3 rayOrigin = isRight ? RaycastOriginBottomRight : RaycastOriginBottomLeft;

            if (isRight)
            {
                rayOrigin.x -= (halfWidth - SkinThickness);
            }
            else
            {
                rayOrigin.x += (halfWidth - SkinThickness);
            }

            Vector2 rayDirection = isRight ? Vector2.right : -Vector2.right;

            float offset = 0f;
            for (int i = 1; i < HorizontalRays - 1; i++)
            {
                Vector2 rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * VerticalRaySpread));
                
                if (DebugMode)
                {
                    Debug.DrawRay(rayVector, rayDirection * halfWidth, isRight ? Color.cyan : Color.magenta);
                }
                
                RaycastHit2D raycastHit = Physics2D.Raycast(rayVector, rayDirection, halfWidth, GroundLayerMask);
                
                if (!raycastHit)
                {
                    continue;
                }

                offset = isRight ? ((raycastHit.point.x - transform.position.x) - halfWidth) : (halfWidth - (transform.position.x - raycastHit.point.x));
            }

            deltaMovement.x += offset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaMovement"></param>
        private void DetermineHorizontalMovement(ref Vector2 deltaMovement)
        {
            // can't move horizontally while climbing
            if (State.IsClimbing())
            {
                deltaMovement.x = 0;
                
                return;
            }

            // check in which direction we're moving
            bool movingRight = deltaMovement.x > 0;

            // calculate the properties of the rays
            Vector3 rayOrigin = movingRight ? RaycastOriginBottomRight : RaycastOriginBottomLeft;
            Vector2 rayDirection = movingRight ? Vector2.right : -Vector2.right;
            float rayDistance = SkinThickness + Mathf.Abs(deltaMovement.x);

            // draw the rays
            for (int i = 0; i < HorizontalRays; i++)
            {
                // calculate the position of the ray
                Vector2 rayVector = new Vector2(
                    rayOrigin.x,
                    rayOrigin.y + (i * VerticalRaySpread)
                );

                // visualize the ray
                if (DebugMode)
                {
                    Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.green);
                }

                // calculate whether the ray collided with something
                RaycastHit2D raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, GroundLayerMask);

                // if no collisions where found, move on to the next ray
                if (!raycastHit)
                {
                    continue;
                }

                // ??? some stuff for slopes
                if (i == 0 && DetermineHorizontalSlopeMovement(ref deltaMovement, Vector2.Angle(raycastHit.normal, Vector2.up), movingRight))
                {
                    break;
                }

                // limit the deltamovement by the collision found
                deltaMovement.x = raycastHit.point.x - rayVector.x;

                // limit the ray distanceto this distance as well, since a collision further than this is irrelevant
                rayDistance = Mathf.Abs(deltaMovement.x);

                // correct for skin thickness and set collision states
                if (movingRight)
                {
                    deltaMovement.x -= SkinThickness;
                    State.CollisionRight = true;
                }
                else
                {
                    deltaMovement.x += SkinThickness;
                    State.CollisionLeft = true;
                }

                // if we're right next to the collision already, break out, we don't need the other rays
                if (rayDistance < SkinThickness + 0.0001f)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaMovement"></param>
        private void DetermineVerticalMovement(ref Vector2 deltaMovement)
        {
            bool movingUp = deltaMovement.y > 0;

            float rayDistance = SkinThickness + Mathf.Abs(deltaMovement.y);
            Vector2 rayDirection = movingUp ? Vector2.up : -Vector2.up;
            Vector3 rayOrigin = movingUp ? RaycastOriginTopLeft : RaycastOriginBottomLeft;

            rayOrigin.x += deltaMovement.x;

            float standingOnDistance = float.MaxValue;

            for (int i = 0; i < VerticalRays; i++)
            {
                Vector2 rayVector = new Vector2(
                    rayOrigin.x + (i * HorizontalRaySpread),
                    rayOrigin.y
                );

                if (DebugMode)
                {
                    Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);
                }

                RaycastHit2D raycastHit;
                if (movingUp || State.IsClimbing() || State.IsCrouching())
                {
                    raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, GroundLayerMask);
                }
                else
                {
                    raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, GroundLayerMask | OneWayPlatformLayerMask);
                }

                if (!raycastHit)
                {
                    continue;
                }

                if (!movingUp)
                {
                    float verticalDistanceToHit = transform.position.y - raycastHit.point.y;

                    if (verticalDistanceToHit < standingOnDistance)
                    {
                        standingOnDistance = verticalDistanceToHit;
                        Platform = raycastHit.collider.gameObject.GetComponent<PhysicsPlatform2D>();
                    }
                }

                deltaMovement.y = raycastHit.point.y - rayVector.y;

                rayDistance = Mathf.Abs(deltaMovement.y);

                // correct for skin thickness and set collision states
                if (movingUp)
                {
                    deltaMovement.y -= SkinThickness;
                    State.CollisionAbove = true;
                }
                else
                {
                    deltaMovement.y += SkinThickness;
                    State.CollisionBelow = true;
                }

                if (!movingUp && deltaMovement.y > 0.001f)
                {
                    State.SlopeUp = true;
                }

                if (rayDistance < SkinThickness + 0.001f)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DeterminePlatform()
        {
            if (Platform != null)
            {
                ActiveGlobalPlatformPoint = transform.position;
                ActiveLocalPlatformPoint = Platform.transform.InverseTransformPoint(transform.position);

                if (PreviousPlatform == null)
                {
                    SendMessage("OnLand", SendMessageOptions.DontRequireReceiver);

                    if (State.IsClimbing())
                    {
                        StopClimbing();
                    }
                }

                if (PreviousPlatform != Platform)
                {
                    if (PreviousPlatform != null)
                    {
                        PreviousPlatform.SendMessage("OnCharacterControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
                    }

                    Platform.SendMessage("OnCharacterControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
                    PreviousPlatform = Platform;
                }
                else
                {
                    Platform.SendMessage("OnCharacterControllerStay2D", this, SendMessageOptions.DontRequireReceiver);
                }
            }
            else if (PreviousPlatform != null)
            {
                PreviousPlatform.SendMessage("OnCharacterControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
                PreviousPlatform = null;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void DetermineVolume()
        {
            if (Volume != null)
            {
                if (PreviousVolume == null)
                {
                    if (Volume.IsLiquid)
                    {
                        StartSwimming();
                    }
                }

                if (PreviousVolume != Volume)
                {
                    if (PreviousVolume != null)
                    {
                        PreviousVolume.SendMessage("OnCharacterControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
                        
                        if (Volume.IsLiquid && !PreviousVolume.IsLiquid)
                        {
                            StartSwimming();
                        }
                        else if (!Volume.IsLiquid && PreviousVolume.IsLiquid)
                        {
                            StopSwimming();
                        }
                    }

                    Volume.SendMessage("OnCharacterControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
                    
                    PreviousVolume = Volume;
                }
                else
                {
                    Volume.SendMessage("OnCharacterControllerStay2D", this, SendMessageOptions.DontRequireReceiver);
                }
            }
            else if (PreviousVolume != null)
            {
                PreviousVolume.SendMessage("OnCharacterControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
                
                if (PreviousVolume.IsLiquid)
                {
                    StopSwimming();                    
                }
                
                PreviousVolume = null;
            }
        }

        ////////////////////////////
        ///// COLLISION EVENTS /////
        ////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter2D(Collider2D other)
        {
            other.SendMessage("OnCharacterControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerStay2D(Collider2D other)
        {
            other.SendMessage("OnCharacterControllerStay2D", this, SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerExit2D(Collider2D other)
        {
            other.SendMessage("OnCharacterControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
        }

        ////////////////////////////
        ///// MOVING PLATFORMS /////
        ////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        private void HandleMovingPlatforms()
        {
            if (Platform != null)
            {
                Vector3 newGlobalPlatformPoint = Platform.transform.TransformPoint(ActiveLocalPlatformPoint);
                Vector3 moveDistance = newGlobalPlatformPoint - ActiveGlobalPlatformPoint;

                if (moveDistance != Vector3.zero)
                {
                    transform.Translate(moveDistance, Space.World);
                }

                PlatformVelocity = (newGlobalPlatformPoint - ActiveGlobalPlatformPoint) / Time.deltaTime;
            }
            else
            {
                PlatformVelocity = Vector2.zero;
            }

            Platform = null;
        }

        //////////////////
        ///// SLOPES /////
        //////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaMovement"></param>
        /// <param name="angle"></param>
        /// <param name="movingRight"></param>
        /// <returns></returns>
        private bool DetermineHorizontalSlopeMovement(ref Vector2 deltaMovement, float angle, bool movingRight)
        {
            if (Mathf.RoundToInt(angle) == 90)
            {
                return false;
            }

            // if the slope is too steep, break out
            if (angle > PlatformParameters.SlopeLimit)
            {
                deltaMovement.x = 0;
                return true;
            }

            // if we're moving up (by some threshold)
            if (deltaMovement.y > 0.07f)
            {
                return true;
            }

            // correct for skin thickness
            deltaMovement.x += movingRight ? -SkinThickness : SkinThickness;

            // correct vertical movement
            deltaMovement.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * deltaMovement.x);

            // set state parameters
            State.SlopeUp = true;
            State.CollisionBelow = true;

            // return that we modified the movement
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaMovement"></param>
        private void DetermineVerticalSlopeMovement(ref Vector2 deltaMovement)
        {
            float center = (RaycastOriginBottomLeft.x + RaycastOriginBottomRight.x) / 2;
            Vector2 direction = -Vector2.up;

            float slopeDistance = SlopeLimitTangent * (RaycastOriginBottomRight.x - center);
            Vector2 slopeRayVector = new Vector2(center, RaycastOriginBottomLeft.y);

            if (DebugMode)
            {
                Debug.DrawRay(slopeRayVector, direction * slopeDistance, Color.yellow);
            }

            RaycastHit2D raycastHit = Physics2D.Raycast(slopeRayVector, direction, slopeDistance, GroundLayerMask);
            
            if (!raycastHit)
            {
                return;
            }

            bool movingDownSlope = Mathf.Sign(raycastHit.normal.x) == Mathf.Sign(deltaMovement.x);
            
            if (!movingDownSlope)
            {
                return;
            }

            // check if we're not on something perpendicular to us (why?)
            float angle = Vector2.Angle(raycastHit.normal, Vector2.up);
            
            if (Mathf.Abs(angle) < 0.001f)
            {
                return;
            }

            State.SlopeDown = true;
            State.SlopeAngle = angle;

            deltaMovement.y = raycastHit.point.y - slopeRayVector.y;
        }
    }
}