using System.Linq;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Untouchable : Damager
    {
        /// <summary>
        /// 
        /// </summary>
        public LayerMask TargetLayers;

        /// <summary>
        /// 
        /// </summary>
        public enum DamageEvents
        {
            OnEnter,
            OnStay,
        }

        /// <summary>
        /// 
        /// </summary>
        public DamageEvents DamageEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        void OnCollisionEnter2D(Collision2D other)
        {
            if (!DamageEvent.Equals(DamageEvents.OnEnter))
            {
                return;
            }

            if (IsTarget(other.gameObject))
            {
                InflictDamage(other.gameObject);
            }   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        void OnCollisionStay2D(Collision2D other)
        {
            if (!DamageEvent.Equals(DamageEvents.OnStay))
            {
                return;
            }

            if (IsTarget(other.gameObject))
            {
                InflictDamage(other.gameObject);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!DamageEvent.Equals(DamageEvents.OnEnter))
            {
                return;
            }
            
            if (IsTarget(other.gameObject))
            {
                Debug.Log("Trigger Enter");
                InflictDamage(other.gameObject);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerStay2D(Collider2D other)
        {
            if (!DamageEvent.Equals(DamageEvents.OnStay))
            {
                return;
            }

            if (IsTarget(other.gameObject))
            {
                InflictDamage(other.gameObject);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected bool IsTarget(GameObject other)
        {
            return TargetLayers == (TargetLayers| 1 << other.layer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        protected void InflictDamage(GameObject target)
        {
            Damagable damagable = target.GetComponent<Damagable>();

            if (damagable)
            {
                damagable.TakeDamage(this, Damage);
            }
        }
    }
}