using System;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    public class Dropper : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public struct Drop
        {
            /// <summary>
            /// 
            /// </summary>
            public Collectable Loot;
            
            /// <summary>
            /// 
            /// </summary>
            [Range(0f, 1f)] public float DropChance;
        }

        /// <summary>
        /// 
        /// </summary>
        public Drop[] Drops;

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Offset = Vector2.zero;
        
        /// <summary>
        /// 
        /// </summary>
        private readonly Vector2 offsetFactor = Vector2.zero;

        /// <summary>
        /// 
        /// </summary>
        public void OnDeath()
        {
            for (var i = 0; i < Mathf.Min(Drops.Length); i++)
            {
                var drop = Drops[i];
                var roll = UnityEngine.Random.value;

                if (roll < drop.DropChance && drop.Loot != null)
                {
                    var dropPosition = new Vector2(
                        transform.position.x,
                        transform.position.y
                    ) + Offset + offsetFactor * (-1 + UnityEngine.Random.value * 2);

                    Instantiate(drop.Loot, dropPosition, Quaternion.identity);
                }
            }
        }
    }
}