using UnityEngine;

namespace Objects.Obstacles
{
    /// <summary>
    /// 
    /// </summary>
    public class Switch : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private Animator Animator;
        
        /// <summary>
        /// 
        /// </summary>
        private int CollisionCount;
        
        /// <summary>
        /// 
        /// </summary>
        private bool Pressed;
        
        /// <summary>
        /// 
        /// </summary>
        public GameObject Target;
        
        /// <summary>
        /// 
        /// </summary>
        public bool Sticky;

        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            Animator = GetComponent<Animator>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        void OnTriggerEnter2D(Collider2D other)
        {
            // if (!other.isTrigger)
            // {
                CollisionCount++;
            
                if (!Pressed)
                {
                    OnPress();
                }
            // }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        void OnTriggerExit2D(Collider2D other)
        {
            if (Sticky)
            {
                return;
            }

            // if (!other.isTrigger)
            // {
                CollisionCount--;
                
                if (Pressed && CollisionCount <= 0)
                {
                    OnDepress();
                }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnPress()
        {
            Pressed = true;
            Animator.SetBool("Pressed", true);
            Target.SendMessage("OnSwitchPressed", this, SendMessageOptions.RequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnDepress()
        {
            Pressed = false;
            Animator.SetBool("Pressed", false);
            Target.SendMessage("OnSwitchDepressed", this, SendMessageOptions.RequireReceiver);
        }
    }
}