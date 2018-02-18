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
        /// <param name="climber"></param>
        public void OnClimberEnter(Climber climber)
        {
            climber.AddClimbable(this);

            if (climber.IsClimbing())
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
        /// <param name="climber"></param>
        public void OnClimberExit(Climber climber)
        {
            climber.RemoveClimbable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public void OnInteraction(Player player)
        {
            Climber climber = player.GetComponent<Climber>();

            if (!climber)
            {
                return;
            }

            if (!climber.IsClimbing() && climber.CanClimb())
            {
                climber.StartClimbing();
                Interactable.Action = "Drop";
            }
            else if (climber.IsClimbing())
            {
                climber.StopClimbing();
                Interactable.Action = "Climb";
            }
        }
    }
}