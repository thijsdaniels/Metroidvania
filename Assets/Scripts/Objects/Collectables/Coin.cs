using Traits;
using UnityEngine;

namespace Objects.Collectables
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Collectable))]
    public class Coin : MonoBehaviour
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
            collector.Coins += Value;
            
            Destroy(gameObject);
        }
    }
}