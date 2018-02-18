using Character;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(Animator))]
    public class Roller : MonoBehaviour
    {
        /// <summary>
        /// Rolling
        /// </summary>
        public float RollSpeed = 12f;
        
        /// <summary>
        /// 
        /// </summary>
        protected CharacterController2D Controller;

        /// <summary>
        /// 
        /// </summary>
        protected Interactor Interactor;

        /// <summary>
        /// 
        /// </summary>
        protected Animator Animator;

        /// <summary>
        /// 
        /// </summary>
        protected Damagable Damagable;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Controller = GetComponent<CharacterController2D>();
            Interactor = GetComponent<Interactor>();
            Animator = GetComponent<Animator>();
            Damagable = GetComponent<Damagable>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanRoll()
        {
            // TODO: Maybe just let Mecanim decide when it's OK to roll?
            return (
                !(Interactor && Interactor.CanInteract()) &&
                !Controller.State.IsRolling() &&
                Controller.State.IsGrounded() &&
                !Controller.State.IsClimbing() &&
                !Controller.State.IsSwimming()
            );
        }

        /// <summary>
        /// 
        /// </summary>
        public void Roll()
        {
            Animator.SetTrigger("Roll");
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnRollStart()
        {
            Controller.State.Rolling = true;

            if (Damagable)
            {
                Damagable.Dodging = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnRollEnd()
        {
            Controller.State.Rolling = false;

            if (Damagable)
            {
                Damagable.Dodging = false;
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
            
            if (player.ControllerInput.B.Pressed && CanRoll())
            {
                Roll();
            }

            if (Controller.State.MovementMode.Equals(CharacterState2D.MovementModes.Rolling))
            {
                Move();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void Move()
        {
            MoveHorizontally();
            MoveVertically();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveHorizontally()
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
        private void MoveVertically()
        {
            // No vertical movement.
        }
    }
}