using UnityEngine;
using System.Collections;

namespace Ammo
{
    [RequireComponent(typeof(Collectable))]

    public class Bomb : MonoBehaviour
    {
        public int value;

        public void OnCollect(Collector collector)
        {
            collector.bombs += value;
            Destroy(gameObject);
        }
    }
}