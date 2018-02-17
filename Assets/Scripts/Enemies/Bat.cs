using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// 
    /// </summary>
    public class Bat : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private Animator Animator;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Animator = GetComponent<Animator>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnWakeUp()
        {
            Animator.SetBool("Asleep", false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnFallAsleep()
        {
            Animator.SetBool("Asleep", true);
        }
    }
}
