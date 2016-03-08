using UnityEngine;
using System.Collections;

namespace Objects.Collectables
{
    [RequireComponent(typeof(Collectable))]
    [RequireComponent(typeof(SpriteRenderer))]

    /**
     * 
     */
    abstract public class Item : MonoBehaviour
    {
        public float coolDown = 0f;
        protected Collector owner;

        /**
	     * 
	     */
        public void Update()
        {
            if (!IsCooledDown())
            {
                CoolDown(Time.deltaTime);
            }
        }

        /**
	     * 
	     */
        public bool IsCooledDown()
        {
            return coolDown <= 0f;
        }

        /**
	     * 
	     */
        protected void CoolDown(float deltaCooldown)
        {
            coolDown = Mathf.Max(0f, coolDown - deltaCooldown);
        }

        /**
	     * 
	     */
        protected void SetCoolDown(float duration)
        {
            coolDown = duration;
        }

        /**
	     * 
	     */
        public virtual void OnCollect(Collector collector)
        {
            if (collector.hasItem(this))
            {
                return;
            }

            collector.items.Add(this);

            owner = collector;

            //this.gameObject.SetActive(false); // TODO: Find a way to remove the object instance from the game world while still allowing the item to be used.
        }

        /**
	     * 
	     */
        public virtual bool CanBeUsed()
        {
            return true;
        }

        /**
	     * 
	     */
        public virtual void OnPress() { }

        /**
	     * 
	     */
        public virtual void OnHold() { }

        /**
	     * 
	     */
        public virtual void OnRelease() { }

        /**
	     * 
	     */
        public virtual Collector.Ammo? GetAmmo()
        {
            return null;
        }
    }
}