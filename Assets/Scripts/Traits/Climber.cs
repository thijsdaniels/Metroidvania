using Character;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(CharacterController2D))]
    public class Climber : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public float ClimbSpeed = 4f;

        /// <summary>
        /// Climbing
        /// </summary>
        protected float ClimbingThreshold = 0.5f;
        
        /// <summary>
        /// 
        /// </summary>
        protected int Climbables;
        
        /// <summary>
        /// 
        /// </summary>
        protected Climbable Climbable;

        /// <summary>
        /// 
        /// </summary>
        protected CharacterController2D Controller;
        
        /// <summary>
        /// 
        /// </summary>
        public AudioClip LeftClimbSound;
        public AudioClip RightClimbSound;

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

            if (Climbables == 0 && Controller.State.IsClimbing())
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
        /// <returns></returns>
        public bool IsClimbing()
        {
            return Controller.State.IsClimbing();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartClimbing()
        {
            Controller.State.Climbing = true;

            Controller.SetHorizontalVelocity(0);

            transform.position = new Vector3(
                Climbable.transform.position.x + Climbable.Offset,
                transform.position.y,
                transform.position.z
            );
            
            SendMessage("OnStartClimbing", this, SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopClimbing()
        {
            Controller.State.Climbing = false;

            Controller.SetVerticalVelocity(0);
            
            SendMessage("OnStopClimbing", this, SendMessageOptions.DontRequireReceiver);
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
            
            if (Mathf.Abs(player.ControllerInput.Movement.y) > ClimbingThreshold && CanClimb())
            {
                StartClimbing();
            }

            if (Controller.State.MovementMode.Equals(CharacterState2D.MovementModes.Climbing))
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
            Controller.SetVerticalVelocity(movement.y * ClimbSpeed);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnLand()
        {
            if (Controller.State.IsClimbing())
            {
                StopClimbing();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnStartSwimming()
        {
            if (IsClimbing())
            {
                StopClimbing();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void OnLeftClimb()
        {
            if (LeftClimbSound)
            {
                AudioSource.PlayClipAtPoint(LeftClimbSound, transform.position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnRightClimb()
        {
            if (RightClimbSound)
            {
                AudioSource.PlayClipAtPoint(RightClimbSound, transform.position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animator"></param>
        public void OnAnimate(Animator animator)
        {
            // set climbing animation parameter
            animator.SetBool("Climbing", Controller.State.IsClimbing());
        }
    }
}