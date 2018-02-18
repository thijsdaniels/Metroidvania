using Character;
using UnityEngine;

namespace Traits
{
    [RequireComponent(typeof(CharacterController2D))]
    public class Swimmer : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public Vector2 SwimSpeed = new Vector2(4f, 4f);
        
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
        public void StartSwimming()
        {
            Controller.State.Swimming = true;
            
            SendMessage("OnStartSwimming", SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopSwimming()
        {
            Controller.State.Swimming = false;
            
            SendMessage("OnStopSwimming", SendMessageOptions.DontRequireReceiver);
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
            
            if (Controller.State.MovementMode.Equals(CharacterState2D.MovementModes.Swimming))
            {
                Move(player.ControllerInput.Movement);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="movement"></param>
        public void Move(Vector2 movement)
        {
            MoveHorizontally(movement);
            MoveVertically(movement);
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveHorizontally(Vector2 movement)
        {
            if (
                movement.x > 0f &&
                Controller.Velocity.x < SwimSpeed.x ||
                movement.x < 0f &&
                Controller.Velocity.x > -SwimSpeed.x
            )
            {
                Controller.AddHorizontalVelocity(movement.x * SwimSpeed.x);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveVertically(Vector2 movement)
        {
            if (
                movement.y > 0f &&
                Controller.Velocity.y < SwimSpeed.y ||
                movement.y < 0f &&
                Controller.Velocity.y > -SwimSpeed.y
            )
            {
                Controller.AddVerticalVelocity(movement.y * SwimSpeed.y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnLiquidVolumeEnter()
        {
            StartSwimming();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void OnLiquidVolumeExit()
        {
            StopSwimming();
        }
    }
}