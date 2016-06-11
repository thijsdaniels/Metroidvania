﻿using UnityEngine;

namespace Objects.Collectables.Items
{
    /**
     * 
     */
    public class Bombs : Item
    {
        protected float charge;
        public float initialCharge = 1.5f;
        public float chargeFactor = 3f;
        public float maxCharge = 3f;

        protected float force = 25f;
        public float coolDownDuration = 0.25f;

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

            owner.ammo.bombs.Upgrade(20);
            owner.ammo.bombs.Restore();
            owner.ammo.bombs.Enable();
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

            if (!owner.ammo.bombs.Available() || bombCount >= maxBombCount)
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
            owner.ammo.bombs.Consume();

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
        public override Collector.Ammo? GetAmmo()
        {
            return owner.ammo.bombs;
        }
    }
}