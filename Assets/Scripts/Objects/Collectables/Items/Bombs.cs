using UnityEngine;

/**
 * 
 */
public class Bombs : Item
{
	private float charge;
	public float initialCharge = 1.5f;
    public float chargeFactor = 3f;
    public float maxCharge = 3f;

	private float force = 25f;
	private float coolDownDuration = 0.25f;

	public Bomb bomb;
	private Bomb bombInstance;

	/**
	 * 
	 */
	public void Start()
	{
		this.charge = this.initialCharge;
	}

    /**
	 * 
	 */
    public override bool CanBeUsed(Player player)
	{
		CharacterController2D controller = player.GetComponent<CharacterController2D>();

		return controller && !controller.State.IsClimbing();
	}

	/**
	 * 
	 */
	public override void OnPress(Player player)
	{
		if (this.bombInstance || !this.IsCooledDown() || !this.CanBeUsed(player))
        {
            return;
        }

        this.Draw(player);
	}

	/**
	 * 
	 */
	public void Draw(Player player)
	{
		this.bombInstance = Instantiate(bomb, player.transform.position, Quaternion.identity) as Bomb;

		player.Grab(bombInstance.gameObject);

		this.bombInstance.LightFuse();
	}

	/**
	 * 
	 */
	public override void OnHold(Player player)
	{
        if (!this.bombInstance)
        {
            return;
        }

        this.Charge(Time.deltaTime * chargeFactor);
	}

	/**
	 * 
	 */
	private void Charge(float deltaCharge)
	{
        if (!this.bombInstance)
        {
            return;
        }

        this.charge = Mathf.Min(this.maxCharge, this.charge + deltaCharge);
	}

	/**
	 * 
	 */
	public override void OnRelease(Player player)
	{
        if (!this.bombInstance)
        {
            return;
        }

        this.Throw(player);

		this.SetCoolDown(this.coolDownDuration);
	}

	/**
	 * 
	 */
	private void Throw(Player player)
	{
		player.Throw(player.GetAim() * this.charge * this.force);

		this.bombInstance = null;

		this.charge = this.initialCharge;
	}
}
