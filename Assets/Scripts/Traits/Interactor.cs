using Character;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    public class Interactor : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public Interactable Interactable;
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanInteract()
        {
            return (bool) Interactable;
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
            
            if (CanInteract())
            {
                player.ControllerInput.B.OncePressed(Interact);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Interact()
        {
            Interactable.SendMessage("OnInteraction", this, SendMessageOptions.RequireReceiver);
        }
    }
}