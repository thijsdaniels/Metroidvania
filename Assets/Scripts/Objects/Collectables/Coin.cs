using UnityEngine;
using System.Collections;

namespace Objects.Collectables
{
    [RequireComponent(typeof(Collectable))]

    public class Coin : MonoBehaviour
    {
        public int value;

        public void OnCollect(Collector collector)
        {
            collector.coins += value;
            Destroy(gameObject);
        }
    }
}