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
            Interactor interactor = other.GetComponent<Interactor>();

            if (interactor)
            {
                interactor.Interactable = this;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerExit2D(Collider2D other)
        {
            var interactor = other.GetComponent<Interactor>();

            if (interactor)
            {
                if (interactor.Interactable == this)
                {
                    interactor.Interactable = null;
                }
            }
        }
    }
}