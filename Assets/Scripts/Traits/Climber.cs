using Character;
using Physics;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Body))]
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
        protected Body Body;
        
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
            Body = GetComponent<Body>();
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

            if (Climbables == 0 && Body.State.IsClimbing())
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
            return Body.State.IsClimbing();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartClimbing()
        {
            Body.State.Climbing = true;

            Body.SetHorizontalVelocity(0);

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
            Body.State.Climbing = false;

            Body.SetVerticalVelocity(0);
            
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

            if (Body.State.MovementMode.Equals(State.MovementModes.Climbing))
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
            Body.SetVerticalVelocity(movement.y * ClimbSpeed);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnLand(Platform platform)
        {
            if (Body.State.IsClimbing())
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
            animator.SetBool("Climbing", Body.State.IsClimbing());
        }
    }
}