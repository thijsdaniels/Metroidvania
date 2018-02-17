using Character;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public string Action;
        
        /// <summary>
        /// 
        /// </summary>
        public void Enable()
        {
            GetComponent<Collider2D>().enabled = true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Disable()
        {
            GetComponent<Collider2D>().enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<Player>();

            if (player)
            {
                player.Interactable = this;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerExit2D(Collider2D other)
        {
            var player = other.GetComponent<Player>();

            if (player)
            {
                if (player.Interactable == this)
                {
                    player.Interactable = null;
                }
            }
        }
    }
}