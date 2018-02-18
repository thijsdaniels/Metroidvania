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
            
            if (player.ControllerInput.B.Pressed && CanInteract())
            {
                Interactable.SendMessage("OnInteraction", this, SendMessageOptions.RequireReceiver);
            }
        }
    }
}