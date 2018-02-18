using Character;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    /// TODO: Proceduraly create a Crosshair instead of relying on an assigned one.
    [RequireComponent(typeof(CharacterController2D))]
    public class Aimer : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        protected CharacterController2D Controller;
        
        /// <summary>
        /// 
        /// </summary>
        private Vector2 AimingDirection;
        
        /// <summary>
        /// 
        /// </summary>
        private float AimingTimeScale = 0.1f;

        /// <summary>
        /// 
        /// </summary>
        public Transform Crosshair;
        
        /// <summary>
        /// 
        /// </summary>
        public Vector2 CrosshairOffset;
        
        /// <summary>
        /// 
        /// </summary>
        [Range(0f, 3f)] public float CrosshairDistance = 1.5f;

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
        /// <param name="player"></param>
        public void OnInput(Player player)
        {
            if (!player.IsListening())
            {
                return;
            }
            
            SetAimingDirection(player.ControllerInput.Aim);

            if (Controller.State.MovementMode.Equals(CharacterState2D.MovementModes.Aiming))
            {
                Move(player.ControllerInput.Movement);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void Move(Vector2 movement)
        {
            MoveHorizontally(movement);
            MoveVertically(movement);
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveHorizontally(Vector2 movement)
        {
            // No horizontal movement.
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveVertically(Vector2 movement)
        {
            // No vertical movement.
        }
        
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
        /// <param name="aim"></param>
        private void SetAimingDirection(Vector2 aim)
        {
            // controller aiming
            AimingDirection = aim.normalized;

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
    }
}