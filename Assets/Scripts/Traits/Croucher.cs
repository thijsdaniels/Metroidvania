using Character;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(CharacterController2D))] 
    public class Croucher : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        protected CharacterController2D Controller;
        
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
            Controller = GetComponent<CharacterController2D>();
            Animator = GetComponent<Animator>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCrouch()
        {
            return (
                !Controller.State.IsSwimming() &&
                !Controller.State.IsClimbing() &&
                !Controller.State.IsAttacking() &&
                !Controller.State.IsAiming()
            );
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void StartCrouching()
        {
            Controller.State.Crouching = true;

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
            Controller.State.Crouching = false;

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

            if (Controller.State.MovementMode.Equals(CharacterState2D.MovementModes.Crouching))
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