using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collectable))]

/**
 * 
 */
abstract public class Item : MonoBehaviour
{
	public float coolDown = 0f;

	/**
	 * @todo Somehow, Update isn't being called...
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
        return true; // return this.coolDown <= 0f;
	}

	/**
	 * 
	 */
	protected void CoolDown(float deltaCooldown)
    {
        this.coolDown = Mathf.Max(0f, this.coolDown - deltaCooldown);
	}

	/**
	 * 
	 */
	protected void SetCoolDown(float duration)
    {
        this.coolDown = duration;
	}

	/**
	 * 
	 */
	public void OnCollect(Collector collector)
    {
		if (collector.hasItem(this))
        {
			return;
		}

		collector.items.Add(this);

        //Destroy(gameObject); // TODO: I can't destroy the gameObject, since it is in the inventory.
	}

	/**
	 * 
	 */
	public virtual bool CanBeUsed(Player player)
	{
		return true;
	}

	/**
	 * 
	 */
	public virtual void OnPress(Player player) {}

	/**
	 * 
	 */
	public virtual void OnHold(Player player) {}

	/**
	 * 
	 */
	public virtual void OnRelease(Player player) {}
}
