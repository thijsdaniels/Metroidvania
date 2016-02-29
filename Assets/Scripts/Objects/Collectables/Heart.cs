using UnityEngine;
using System.Collections;

namespace Objects.Collectables
{
    [RequireComponent(typeof(Collectable))]

    public class Heart : MonoBehaviour
    {
        public void OnCollect(Collector collector)
        {
            var damagable = collector.GetComponent<Damagable>();

            if (damagable)
            {
                damagable.Heal(1);
            }

            Destroy(gameObject);
        }
    }
}