using System;
using Character;
using Physics;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Body))] 
    public class Jumper : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        Body Body;
        
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
            Body = GetComponent<Body>();
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
            if (Body.State.IsClimbing())
            {
                return false;
            }
            
            if (JumpTimeout > 0)
            {
                return false;
            }

            switch (Body.VolumeParameters.JumpMode)
            {
                case VolumeParameters.JumpModes.Anywhere:
                {
                    return true;
                }
                case VolumeParameters.JumpModes.Ground:
                {
                    return (
                        Body.State.IsGrounded() ||
                        CurrentAirJump < AirJumps ||
                        WallJumpEnabled && Body.State.IsCollidingHorizontally()
                    );
                }
                case VolumeParameters.JumpModes.None:
                {
                    return false;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(VolumeParameters.JumpMode), Body.VolumeParameters.JumpMode.ToString(), "Invalid JumpMode.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Jump()
        {
            if (Body.State.IsGrounded())
            {
                SendMessage("OnJump", SendMessageOptions.DontRequireReceiver);

                Body.SetVerticalVelocity(JumpVelocity * Body.VolumeParameters.JumpModifier);
            }
            else
            {
                if (!Body.State.IsSwimming() && Body.State.IsCollidingHorizontally())
                {
                    SendMessage("OnWallJump", SendMessageOptions.DontRequireReceiver);

                    int direction = Body.State.CollisionLeft ? 1 : -1;
            
                    Body.SetVelocity(new Vector2(
                        WallJumpVelocity.x * Body.VolumeParameters.JumpModifier * direction,
                        WallJumpVelocity.y * Body.VolumeParameters.JumpModifier
                    ));
                }
                else
                {
                    if (Body.VolumeParameters.JumpMode == VolumeParameters.JumpModes.Ground)
                    {
                        CurrentAirJump++;
                    }

                    SendMessage("OnAirJump", SendMessageOptions.DontRequireReceiver);
                    
                    Body.SetVerticalVelocity(JumpVelocity * Body.VolumeParameters.JumpModifier);
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
            
            if (CanJump())
            {
                player.ControllerInput.A.OncePressed(Jump);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnLand(Platform platform)
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