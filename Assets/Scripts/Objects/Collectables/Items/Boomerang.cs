using UnityEngine;
using BoomerangProjectile = Objects.Projectiles.Boomerang;

namespace Objects.Collectables.Items
{
    /**
     * 
     */
    public class Boomerang : Item
    {
        protected float charge;
        public float initialCharge = 2f;
        public float chargeFactor = 1f;
        public float maxCharge = 3f;

        protected float force = 100f;
        public float coolDownDuration = 0.25f;

        [SerializeField] protected BoomerangProjectile projectile;
        [HideInInspector] public int projectileCount;

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
        public override bool CanBeUsed()
        {
            if (!base.CanBeUsed())
            {
                return false;
            }

            if (projectileCount >= 1)
            {
                return false;
            }

            CharacterController2D controller = owner.GetComponent<CharacterController2D>();
            if (controller.State.IsRolling() || controller.State.IsSwimming() || controller.State.IsClimbing())
            {
                return false;
            }

            return true;
        }

        /**
	     * 
	     */
        public override void OnPress()
        {
            if (!IsCooledDown() || !CanBeUsed())
            {
                return;
            }

            Player player = owner.GetComponent<Player>();
            player.StartAiming();
        }

        /**
	     * 
	     */
        public override void OnHold()
        {
            if (!IsCooledDown() || !CanBeUsed())
            {
                return;
            }

            Charge(Time.unscaledDeltaTime * chargeFactor);
        }

        /**
	     * 
	     */
        protected void Charge(float deltaCharge)
        {
            charge = Mathf.Min(maxCharge, charge + deltaCharge);
        }

        /**
	     * 
	     */
        public override void OnRelease()
        {
            if (!IsCooledDown() || !CanBeUsed())
            {
                return;
            }

            Player player = owner.GetComponent<Player>();
            Throw(player.transform.position, player.GetAimingDirection());
            
            player.StopAiming();

            SetCoolDown(coolDownDuration);
        }

        /**
	     * 
	     */
        protected void Throw(Vector3 origin, Vector2 direction)
        {
            BoomerangProjectile projectileInstance = Instantiate(projectile, origin, Quaternion.identity) as BoomerangProjectile;

            projectileCount++;
            projectileInstance.owner = owner;
            projectileInstance.boomerangItem = this;

            Rigidbody2D projectileBody = projectileInstance.GetComponent<Rigidbody2D>();
            projectileBody.AddForce(direction * charge * force);

            charge = initialCharge;
        }
    }
}