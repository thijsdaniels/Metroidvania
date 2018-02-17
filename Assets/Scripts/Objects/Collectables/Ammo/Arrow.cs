using UnityEngine;
using Traits;

namespace Objects.Collectables.Ammo
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Collectable))]
    public class Arrow : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public int Value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collector"></param>
        public void OnCollect(Collector collector)
        {
            collector.Arrows.Add(Value);

            Destroy(gameObject);
        }
    }
}