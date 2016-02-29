using UnityEngine;
using System.Collections;

namespace Objects.Collectables
{
    [RequireComponent(typeof(Collectable))]

    public class Key : MonoBehaviour
    {
        public void OnCollect(Collector collector)
        {
            collector.keys++;
            Destroy(gameObject);
        }
    }
}
