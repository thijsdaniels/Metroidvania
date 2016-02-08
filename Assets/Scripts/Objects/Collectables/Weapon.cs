using UnityEngine;
using System.Collections;

/**
 * 
 */
abstract public class Weapon : MonoBehaviour
{
	public float coolDown = 0f;

	/**
	 * @todo Somehow, Update isn't being called...
	 */
	public void Update()
    {
        if (!IsCooledDown()) {
			CoolDown(Time.deltaTime);
		}
	}

	/**
	 * 
	 */
	public bool IsCooledDown()
    {
        return true;
		return this.coolDown <= 0f;
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

		if (collector.hasWeapon(this)) {
			return;
		}

		collector.weapons.Add(this);

		Debug.Log("weapon added");
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
	public virtual void Press(Player player) {}

	/**
	 * 
	 */
	public virtual void Hold(Player player) {}

	/**
	 * 
	 */
	public virtual void Release(Player player) {}
}
