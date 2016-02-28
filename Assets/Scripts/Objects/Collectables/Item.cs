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
    protected Collector owner;

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
	public virtual void OnCollect(Collector collector)
    {
		if (collector.hasItem(this))
        {
			return;
		}

		collector.items.Add(this);

        this.owner = collector;

        //Destroy(gameObject); // TODO: I can't destroy the gameObject, since it is in the inventory.
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
	public virtual void OnPress() {}

	/**
	 * 
	 */
	public virtual void OnHold() {}

	/**
	 * 
	 */
	public virtual void OnRelease() {}

    /**
     *
     */
    public virtual bool RequiresAmmo()
    {
        return false;
    }

    /**
     *
     */
    public virtual int GetAmmo()
    {
        return 0;
    }
}
