using System;
using Character;
using Physics;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(CharacterController2D))] 
    public class Jumper : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        CharacterController2D Controller;
        
        /// <summary>
        /// 
        /// </summary>
        [Range(0f, 20f)] public float JumpVelocity = 10f;
        
        /// <summary>
        /// 
        /// </summary>
        [Range(0f, 1f)] public float JumpCooldown = 0.25f;
        
        /// <summary>
        /// 
        /// </summary>
        private float JumpTimeout;
        
        /// <summary>
        /// 
        /// </summary>
        public AudioClip JumpSound;
        
        /// <summary>
        /// 
        /// </summary>
        public int AirJumps;
        
        /// <summary>
        /// 
        /// </summary>
        private int CurrentAirJump;
        
        /// <summary>
        /// 
        /// </summary>
        public AudioClip AirJumpSound;
        
        /// <summary>
        /// 
        /// </summary>
        public bool WallJumpEnabled;
        
        /// <summary>
        /// 
        /// </summary>
        public Vector2 WallJumpVelocity = new Vector2(10f, 10f);
        
        /// <summary>
        /// 
        /// </summary>
        public AudioClip WallJumpSound;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Controller = GetComponent<CharacterController2D>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            if (JumpTimeout > 0)
            {
                JumpTimeout = Mathf.Max(0, JumpTimeout - Time.deltaTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool CanJump()
        {
            if (Controller.State.IsClimbing())
            {
                return false;
            }
            
            if (JumpTimeout > 0)
            {
                return false;
            }

            switch (Controller.VolumeParameters.JumpMode)
            {
                case PhysicsVolumeParameters2D.JumpModes.Anywhere:
                {
                    return true;
                }
                case PhysicsVolumeParameters2D.JumpModes.Ground:
                {
                    return (
                        Controller.State.IsGrounded() ||
                        CurrentAirJump < AirJumps ||
                        WallJumpEnabled && Controller.State.IsCollidingHorizontally()
                    );
                }
                case PhysicsVolumeParameters2D.JumpModes.None:
                {
                    return false;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(PhysicsVolumeParameters2D.JumpMode), Controller.VolumeParameters.JumpMode.ToString(), "Invalid JumpMode.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Jump()
        {
            if (Controller.State.IsGrounded())
            {
                SendMessage("OnJump", SendMessageOptions.DontRequireReceiver);

                Controller.SetVerticalVelocity(JumpVelocity * Controller.VolumeParameters.JumpModifier);
            }
            else
            {
                if (!Controller.State.IsSwimming() && Controller.State.IsCollidingHorizontally())
                {
                    SendMessage("OnWallJump", SendMessageOptions.DontRequireReceiver);

                    int direction = Controller.State.CollisionLeft ? 1 : -1;
            
                    Controller.SetVelocity(new Vector2(
                        WallJumpVelocity.x * Controller.VolumeParameters.JumpModifier * direction,
                        WallJumpVelocity.y * Controller.VolumeParameters.JumpModifier
                    ));
                }
                else
                {
                    if (Controller.VolumeParameters.JumpMode == PhysicsVolumeParameters2D.JumpModes.Ground)
                    {
                        CurrentAirJump++;
                    }

                    SendMessage("OnAirJump", SendMessageOptions.DontRequireReceiver);
                    
                    Controller.SetVerticalVelocity(JumpVelocity * Controller.VolumeParameters.JumpModifier);
                }
            }

            JumpTimeout = JumpCooldown;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public void OnInput(Player player)
        {
            if (!player.IsListening())
            {
                return;
            }
            
            if (player.ControllerInput.A.Pressed && CanJump())
            {
                Jump();
            }
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

        /// <summary>
        /// 
        /// </summary>
        public void OnStartClimbing()
        {
            CurrentAirJump = 0;
        }
        
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
    }
}