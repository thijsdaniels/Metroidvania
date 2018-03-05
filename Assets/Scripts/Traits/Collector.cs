using System;
using System.Collections.Generic;
using Character;
using Objects.Collectables;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Player))]
    public class Collector : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [Serializable] 
        public struct Ammo
        {
            /// <summary>
            /// 
            /// </summary>
            public bool Enabled;
            
            /// <summary>
            /// 
            /// </summary>
            public int Current;
            
            /// <summary>
            /// 
            /// </summary>
            public int Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="current"></param>
            /// <param name="maximum"></param>
            public Ammo(int current, int maximum)
            {
                Enabled = true;
                Current = current;
                Maximum = maximum;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="current"></param>
            /// <param name="maximum"></param>
            /// <param name="enabled"></param>
            public Ammo(int current, int maximum, bool enabled)
            {
                Enabled = enabled;
                Current = current;
                Maximum = maximum;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Current.ToString();
            }

            /// <summary>
            /// 
            /// </summary>
            public void Enable()
            {
                Enabled = true;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool Available()
            {
                return Available(1);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool Available(int value)
            {
                return Current >= value;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public float Ratio()
            {
                return (float) Current / Maximum;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Consume()
            {
                Consume(1);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            public void Consume(int value)
            {
                Current = Mathf.Max(Current - value, 0);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            public void Add(int value)
            {
                Current = Mathf.Min(Current + value, Maximum);
            }

            /// <summary>
            /// 
            /// </summary>
            public void Restore()
            {
                Current = Maximum;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            public void Upgrade(int value)
            {
                Maximum += value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Coins;
    
        /// <summary>
        /// 
        /// </summary>
        public int Keys;

        /// <summary>
        /// 
        /// </summary>
        public List<Item> Items;

        /// <summary>
        /// 
        /// </summary>
        public Ammo Arrows;
            
        /// <summary>
        /// 
        /// </summary>
        public Ammo Bombs;

        /// <summary>
        /// 
        /// </summary>
        public Ammo Mana = new Ammo(30, 30);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool HasItem(Item item)
        {
            return Items.Contains(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectable"></param>
        public void Collect(Collectable collectable)
        {
            collectable.SendMessage("OnCollect", this, SendMessageOptions.RequireReceiver);
        }
    }
}