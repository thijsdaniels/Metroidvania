using Character;
using Physics;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Body))] 
    public class Croucher : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        protected Body Body;
        
        /// <summary>
        /// 
        /// </summary>
        protected Animator Animator;
        
        /// <summary>
        /// 
        /// </summary>
        private float CrouchingThreshold = 0.5f;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Body = GetComponent<Body>();
            Animator = GetComponent<Animator>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCrouch()
        {
            return (
                !Body.State.IsSwimming() &&
                !Body.State.IsClimbing() &&
                !Body.State.IsAttacking() &&
                !Body.State.IsAiming()
            );
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void StartCrouching()
        {
            Body.State.Crouching = true;

            if (Animator)
            {
                Animator.SetBool("Crouching", true);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void StopCrouching()
        {
            Body.State.Crouching = false;

            if (Animator)
            {
                Animator.SetBool("Crouching", false);
            }
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
            
            if (player.ControllerInput.Movement.y < -CrouchingThreshold && CanCrouch())
            {
                StartCrouching();
            }
            else
            {
                StopCrouching();
            }

            if (Body.State.MovementMode.Equals(State.MovementModes.Crouching))
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
    }
}