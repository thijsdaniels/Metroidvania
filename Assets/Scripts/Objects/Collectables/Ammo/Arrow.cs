using UnityEngine;
using System.Collections;

namespace Objects.Collectables.Ammo
{
    [RequireComponent(typeof(Collectable))]

    public class Arrow : MonoBehaviour
    {
        public int value;

        public void OnCollect(Collector collector)
        {
            collector.arrows += value;
            Destroy(gameObject);
        }
    }
}