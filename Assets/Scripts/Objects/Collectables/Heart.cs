using UnityEngine;
using Traits;

namespace Objects.Collectables
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Collectable))]
    public class Heart : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collector"></param>
        public void OnCollect(Collector collector)
        {
            Damagable damagable = collector.GetComponent<Damagable>();

            if (damagable)
            {
                damagable.Heal(1);
            }

            Destroy(gameObject);
        }
    }
}