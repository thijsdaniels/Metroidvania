using Traits;
using UnityEngine;

namespace Objects.Obstacles
{
    /// <summary>
    /// 
    /// </summary>
    public class Door : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private Animator Animator;
        
        /// <summary>
        /// 
        /// </summary>
        private Collider2D Collider;

        /// <summary>
        /// 
        /// </summary>
        public enum AccessModes
        {
            Proximity,
            Remote,
            Locked
        }

        /// <summary>
        /// 
        /// </summary>
        public AccessModes AccessMode = AccessModes.Proximity;

        /// <summary>
        /// 
        /// </summary>
        public AudioClip OpenSound;
        public AudioClip CloseSound;

        /// <summary>
        /// 
        /// </summary>
        void Awake()
        {
            Animator = GetComponent<Animator>();
            Collider = GetComponent<Collider2D>();

            if (AccessMode != AccessModes.Locked)
            {
                Animator.SetTrigger("Unlock");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Open()
        {
            Animator.SetBool("Open", true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            Animator.SetBool("Open", false);
        }

        /// <summary>
        /// 
        /// </summary>
        void OnOpenStart()
        {
            if (OpenSound)
            {
                AudioSource.PlayClipAtPoint(OpenSound, transform.position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void OnOpenEnd()
        {
            Collider.enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        void OnCloseStart()
        {
            if (CloseSound)
            {
                AudioSource.PlayClipAtPoint(CloseSound, transform.position);
            }

            Collider.enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        void OnCloseEnd()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        void OnTriggerEnter2D(Collider2D other)
        {
            if (AccessMode == AccessModes.Locked)
            {
                var collector = other.gameObject.GetComponent<Collector>();

                if (collector && collector.Keys > 0)
                {
                    collector.Keys--;
                    
                    Unlock();
                }
            }
            else if (AccessMode == AccessModes.Proximity && other.tag == "Player")
            {
                Open();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Unlock()
        {
            Animator.SetTrigger("Unlock");
            
            Open();
            
            AccessMode = AccessModes.Proximity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        void OnTriggerExit2D(Collider2D other)
        {
            if (AccessMode == AccessModes.Proximity && other.tag == "Player")
            {
                Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnSwitchPressed(Switch other)
        {
            if (AccessMode == AccessModes.Remote)
            {
                Open();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnSwitchDepressed(Switch other)
        {
            if (AccessMode == AccessModes.Remote)
            {
                Close();
            }
        }
    }
}