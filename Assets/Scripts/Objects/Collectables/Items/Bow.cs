using UnityEngine;

namespace Objects.Collectables.Items
{
    /**
     * 
     */
    public class Bow : Objects.Collectables.Item
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

            collector.arrows = Mathf.Max(collector.arrows, 30);
        }

        /**
	     * 
	     */
        public override bool CanBeUsed()
        {
            if (!owner || owner.arrows <= 0)
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

            owner.arrows--;

            Player player = owner.GetComponent<Player>();
            Shoot(player.transform.position, player.GetAim());

            SetCoolDown(coolDownDuration);
        }

        /**
	     * 
	     */
        protected void Shoot(Vector3 origin, Vector2 direction)
        {
            Arrow arrowInstance = Instantiate(arrow, origin, Quaternion.identity) as Arrow;

            Rigidbody2D arrowBody = arrowInstance.GetComponent<Rigidbody2D>();
            arrowBody.AddForce(direction * charge * force);

            charge = initialCharge;
        }

        /**
         *
         */
        public override bool RequiresAmmo()
        {
            return true;
        }

        /**
         *
         */
        public override int GetAmmo()
        {
            return owner.arrows;
        }
    }
}