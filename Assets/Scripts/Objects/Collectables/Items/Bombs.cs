using UnityEngine;

namespace Objects.Collectables.Items
{
    /**
     * 
     */
    public class Bombs : Objects.Collectables.Item
    {
        protected float charge;
        public float initialCharge = 1.5f;
        public float chargeFactor = 3f;
        public float maxCharge = 3f;

        protected float force = 25f;
        protected float coolDownDuration = 0.25f;

        public Bomb bomb;
        protected Bomb bombInstance;

        [HideInInspector] public int bombCount;
        [SerializeField] private int maxBombCount = 3;

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

            collector.bombs = Mathf.Max(collector.bombs, 10);
        }

        /**
	     * 
	     */
        public override bool CanBeUsed()
        {
            if (!owner || owner.bombs <= 0 || bombCount >= maxBombCount)
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
        public override void OnPress()
        {
            if (bombInstance || !IsCooledDown() || !CanBeUsed())
            {
                return;
            }

            Draw();
        }

        /**
	     * 
	     */
        protected void Draw()
        {
            owner.bombs--;

            bombInstance = Instantiate(bomb, owner.transform.position, Quaternion.identity) as Bomb;

            bombCount++;
            bombInstance.origin = this;

            Player player = owner.GetComponent<Player>();
            player.Grab(bombInstance.gameObject);

            bombInstance.LightFuse();
        }

        /**
	     * 
	     */
        public override void OnHold()
        {
            if (!bombInstance)
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
            if (!bombInstance)
            {
                return;
            }

            charge = Mathf.Min(maxCharge, charge + deltaCharge);
        }

        /**
	     * 
	     */
        public override void OnRelease()
        {
            if (!this.bombInstance)
            {
                return;
            }

            this.Throw();

            this.SetCoolDown(this.coolDownDuration);
        }

        /**
	     * 
	     */
        protected void Throw()
        {
            Player player = owner.GetComponent<Player>();
            player.Throw(player.GetAim() * this.charge * this.force);

            this.bombInstance = null;

            this.charge = this.initialCharge;
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
            return owner.bombs;
        }
    }
}