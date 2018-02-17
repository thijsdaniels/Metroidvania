using Character;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Interactable))]
    public class Climbable : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public float Offset;

        /// <summary>
        /// 
        /// </summary>
        private Interactable Interactable;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Interactable = GetComponent<Interactable>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public void OnCharacterControllerEnter2D(CharacterController2D controller)
        {
            controller.AddClimbable(this);

            if (controller.State.IsClimbing())
            {
                Interactable.Action = "Drop";
            }
            else
            {
                Interactable.Action = "Climb";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public void OnCharacterControllerExit2D(CharacterController2D controller)
        {
            controller.RemoveClimbable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public void OnInteraction(Player player)
        {
            CharacterController2D controller = player.GetComponent<CharacterController2D>();

            if (!controller)
            {
                return;
            }

            if (!controller.State.IsClimbing() && controller.CanClimb())
            {
                controller.StartClimbing();
                Interactable.Action = "Drop";
            }
            else if (controller.State.IsClimbing())
            {
                controller.StopClimbing();
                Interactable.Action = "Climb";
            }
        }
    }
}