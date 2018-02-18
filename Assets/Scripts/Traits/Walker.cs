using Character;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(CharacterController2D))]
    public class Walker : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public float RunSpeed = 8f;

        /// <summary>
        /// 
        /// </summary>
        public AudioClip LeftFootSound;
        
        /// <summary>
        /// 
        /// </summary>
        public AudioClip RightFootSound;
        
        /// <summary>
        /// 
        /// </summary>
        protected CharacterController2D Controller;

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

            if (Controller.State.MovementMode.Equals(CharacterState2D.MovementModes.Walking))
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
            float walkAcceleration = RunSpeed * Controller.PlatformParameters.Traction; // * Time.deltaTime;

            if (movement.x > 0f && Controller.Velocity.x < RunSpeed || movement.x < 0f && Controller.Velocity.x > -RunSpeed)
            {
                Controller.AddHorizontalVelocity(movement.x * walkAcceleration);
            }
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
    }
}