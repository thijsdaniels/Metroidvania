using UnityEngine;
using System.Collections;

namespace Objects.Collectables.Ammo
{
    [RequireComponent(typeof(Collectable))]

    public class Bomb : MonoBehaviour
    {
        public int value;

        public void OnCollect(Collector collector)
        {
            collector.ammo.bombs.Add(value);
            Destroy(gameObject);
        }
    }
}