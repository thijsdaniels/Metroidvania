using UnityEngine;
using Traits;

namespace Objects.Collectables.Ammo
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Collectable))]
    public class Bomb : MonoBehaviour
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
            collector.Bombs.Add(Value);
            
            Destroy(gameObject);
        }
    }
}