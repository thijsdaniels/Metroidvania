using UnityEngine;
using Traits;

namespace Objects.Collectables
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Collectable))]
    public class Key : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collector"></param>
        public void OnCollect(Collector collector)
        {
            collector.Keys++;
            
            Destroy(gameObject);
        }
    }
}