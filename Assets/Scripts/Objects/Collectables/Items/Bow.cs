using UnityEngine;

namespace Objects.Collectables.Items
{
    /**
     * 
     */
    public class Bow : Item
    {
        protected float charge;
        public float initialCharge = 0.5f;
        public float chargeFactor = 3f;
        public float maxCharge = 3f;

        protected float force = 50f;
        protected float coolDownDuration = 0.25f;

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
        public override void OnCollect(Collector collector)
        {
            base.OnCollect(collector);

            owner.ammo.arrows.Upgrade(30);
            owner.ammo.arrows.Restore();
            owner.ammo.arrows.Enable();
        }

        /**
	     * 
	     */
        public override bool CanBeUsed()
        {
            if (!owner || !owner.ammo.arrows.Available() || !owner.mana.Available(arrow.requiredMana))
            {
                return false;
            }

            CharacterController2D controller = owner.GetComponent<CharacterController2D>();
            if (controller.State.IsClimbing())
            {
                return false;
            }

            return true;
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

            Charge(Time.deltaTime * chargeFactor);
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
            Shoot(player.transform.position, player.GetAim());

            SetCoolDown(coolDownDuration);
        }

        /**
	     * 
	     */
        protected void Shoot(Vector3 origin, Vector2 direction)
        {
            owner.ammo.arrows.Consume();
            owner.mana.Consume(arrow.requiredMana);

            Arrow arrowInstance = Instantiate(arrow, origin, Quaternion.identity) as Arrow;

            Rigidbody2D arrowBody = arrowInstance.GetComponent<Rigidbody2D>();
            arrowBody.AddForce(direction * charge * force);

            charge = initialCharge;
        }

        /**
         *
         */
        public override Collector.Ammo? GetAmmo()
        {
            return owner.ammo.arrows;
        }
    }
}