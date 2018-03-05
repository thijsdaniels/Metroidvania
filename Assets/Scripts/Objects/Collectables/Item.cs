using Physics;
using UnityEngine;
using Traits;

namespace Objects.Collectables
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Collectable))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Item : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        protected float CoolDown;
        
        /// <summary>
        /// 
        /// </summary>
        protected Collector Owner;

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            if (!IsCooledDown())
            {
                CoolDownBy(Time.deltaTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsCooledDown()
        {
            return CoolDown <= 0f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaCooldown"></param>
        protected void CoolDownBy(float deltaCooldown)
        {
            CoolDown = Mathf.Max(0f, CoolDown - deltaCooldown);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration"></param>
        protected void SetCoolDown(float duration)
        {
            CoolDown = duration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collector"></param>
        public virtual void OnCollect(Collector collector)
        {
            if (collector.HasItem(this))
            {
                return;
            }

            collector.Items.Add(this);

            Owner = collector;

            /*
             * TODO: Find a way to remove the object instance from the game world while still allowing the item to be
             */
            // this.gameObject.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CanBeUsed()
        {
            if (!Owner)
            {
                return false;
            }
            
            Body body = Owner.GetComponent<Body>();
            
            if (body)
            {
                if (
                    body.State.IsRolling() ||
                    body.State.IsSwimming() ||
                    body.State.IsClimbing() ||
                    body.State.IsSliding())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnPress()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnHold()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnRelease()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Collector.Ammo? GetAmmo()
        {
            return null;
        }
    }
}