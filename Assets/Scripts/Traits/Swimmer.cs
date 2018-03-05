using Character;
using Physics;
using UnityEngine;

namespace Traits
{
    [RequireComponent(typeof(Body))]
    public class Swimmer : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public Vector2 SwimSpeed = new Vector2(4f, 4f);
        
        /// <summary>
        /// 
        /// </summary>
        protected Body Body;

        /// <summary>
        /// 
        /// </summary>
        protected int Liquids;

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
        public void StartSwimming()
        {
            Body.State.Swimming = true;
            
            SendMessage("OnStartSwimming", SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopSwimming()
        {
            Body.State.Swimming = false;
            
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
            
            if (Body.State.MovementMode.Equals(State.MovementModes.Swimming))
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
                Body.Velocity.x < SwimSpeed.x ||
                movement.x < 0f &&
                Body.Velocity.x > -SwimSpeed.x
            )
            {
                Body.AddHorizontalVelocity(movement.x * SwimSpeed.x);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveVertically(Vector2 movement)
        {
            if (
                movement.y > 0f &&
                Body.Velocity.y < SwimSpeed.y ||
                movement.y < 0f &&
                Body.Velocity.y > -SwimSpeed.y
            )
            {
                Body.AddVerticalVelocity(movement.y * SwimSpeed.y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnVolumeEnter(Volume volume)
        {
            if (volume.IsLiquid)
            {
                Liquids++;
            }

            if (Liquids > 0 && !Body.State.IsSwimming())
            {
                StartSwimming();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void OnVolumeExit(Volume volume)
        {
            if (volume.IsLiquid && Liquids > 0)
            {
                Liquids--;
            }
            
            if (Liquids <= 0 && Body.State.IsSwimming())
            {
                StopSwimming();
            }
        }
    }
}