using Character;
using Physics;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Body))]
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
        protected Body Body;

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
            Body = GetComponent<Body>();
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
            return (
                !(Interactor && Interactor.CanInteract()) &&
                !Body.State.IsRolling() &&
                Body.State.IsGrounded() &&
                !Body.State.IsClimbing() &&
                !Body.State.IsSwimming()
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
            Body.State.Rolling = true;

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
            Body.State.Rolling = false;

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
            
            if (CanRoll())
            {
                player.ControllerInput.B.OncePressed(Roll);
            }

            if (Body.State.MovementMode.Equals(State.MovementModes.Rolling))
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
            if (!(Body.Velocity.x < RollSpeed) || !(Body.Velocity.x > -RollSpeed))
            {
                return;
            }

            float directionCoefficient = transform.localScale.x > 0f ? 1f : -1f;
            
            Body.SetHorizontalVelocity(RollSpeed * directionCoefficient);
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