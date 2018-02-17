using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    public class Collectable : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public AudioClip CollectSound;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        void OnTriggerEnter2D(Collider2D target)
        {
            if (!target.gameObject.CompareTag("Player"))
            {
                return;
            }

            var collector = target.gameObject.GetComponent<Collector>();
            
            if (!collector)
            {
                return;
            }
            
            collector.Collect(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collector"></param>
        public virtual void OnCollect(Collector collector)
        {
            if (CollectSound)
            {
                AudioSource.PlayClipAtPoint(CollectSound, collector.transform.position);
            }
        }
    }
}