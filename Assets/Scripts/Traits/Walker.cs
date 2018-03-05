using Character;
using Physics;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Body))]
    public class Walker : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public float RunSpeed = 6f;

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
        protected Body Body;

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
        /// <param name="player"></param>
        public void OnInput(Player player)
        {
            if (!player.IsListening())
            {
                return;
            }

            if (Body.State.MovementMode.Equals(State.MovementModes.Walking))
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
            float walkAcceleration = RunSpeed * Body.PlatformParameters.Traction; // * Time.deltaTime;

            if (movement.x > 0f && Body.Velocity.x < RunSpeed || movement.x < 0f && Body.Velocity.x > -RunSpeed)
            {
                Body.AddHorizontalVelocity(movement.x * walkAcceleration);
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