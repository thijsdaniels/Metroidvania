using UnityEngine;

/**
 * 
 */
public class Bow : Weapon
{
	private float charge;
	public float initialCharge = 0.5f;
    public float chargeFactor = 5f;
    public float maxCharge = 5f;

	private float force = 50f;
	private float coolDownDuration = 0.25f;

	public Arrow arrow;

	/**
	 * 
	 */
	public void Start()
	{
		charge = initialCharge;
	}

	/**
	 * 
	 */
	public override bool CanBeUsed(Player player)
	{
		var controller = player.GetComponent<CharacterController2D>();

		return controller && !controller.IsClimbing();
	}

	/**
	 * 
	 */
	public override void Hold(Player player)
	{
		Charge(Time.deltaTime * chargeFactor);
	}

    /**
	 * 
	 */
    private void Charge(float deltaCharge)
	{
		charge = Mathf.Min(maxCharge, charge + deltaCharge);
	}

	/**
	 * 
	 */
	public override void Release(Player player)
	{
		if (!IsCooledDown() || !CanBeUsed(player)) {
			return;
		}

		Shoot(player.transform.position, player.GetAim());

		SetCoolDown(coolDownDuration);
	}

	/**
	 * 
	 */
	private void Shoot(Vector3 origin, Vector2 direction)
	{
		Arrow arrowInstance = Instantiate(arrow, origin, Quaternion.identity) as Arrow;

		var arrowBody = arrowInstance.GetComponent<Rigidbody2D>();
		arrowBody.AddForce(direction * charge * force);

		charge = initialCharge;
	}
}
