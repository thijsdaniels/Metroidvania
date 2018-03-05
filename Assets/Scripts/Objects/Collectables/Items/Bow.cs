using Objects.Projectiles;
using Traits;
using UnityEngine;

namespace Objects.Collectables.Items
{
    /// <summary>
    /// 
    /// </summary>
    public class Bow : Item
    {
        /// <summary>
        /// 
        /// </summary>
        protected float CurrentCharge;
        public float InitialCharge = 0.5f;
        public float ChargeFactor = 3f;
        public float MaxCharge = 3f;

        /// <summary>
        /// 
        /// </summary>
        protected const float Force = 50f;
        public float CoolDownDuration = 0.25f;

        /// <summary>
        /// 
        /// </summary>
        public Arrow Arrow;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            CurrentCharge = InitialCharge;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collector"></param>
        public override void OnCollect(Collector collector)
        {
            base.OnCollect(collector);

            Owner.Arrows.Upgrade(30);
            Owner.Arrows.Restore();
            Owner.Arrows.Enable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanBeUsed()
        {
            if (!base.CanBeUsed())
            {
                return false;
            }

            if (!Owner.Arrows.Available() || !Owner.Mana.Available(Arrow.RequiredMana))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnPress()
        {
            if (!IsCooledDown() || !CanBeUsed())
            {
                return;
            }

            Aimer aimer = Owner.GetComponent<Aimer>();
            
            aimer.StartAiming();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnHold()
        {
            if (!IsCooledDown() || !CanBeUsed())
            {
                return;
            }

            Charge(Time.unscaledDeltaTime * ChargeFactor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaCharge"></param>
        protected void Charge(float deltaCharge)
        {
            CurrentCharge = Mathf.Min(MaxCharge, CurrentCharge + deltaCharge);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnRelease()
        {
            if (!IsCooledDown() || !CanBeUsed())
            {
                return;
            }

            Aimer aimer = Owner.GetComponent<Aimer>();
            
            Shoot(aimer.transform.position, aimer.GetAimingDirection());

            aimer.StopAiming();

            SetCoolDown(CoolDownDuration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        protected void Shoot(Vector3 origin, Vector2 direction)
        {
            Owner.Arrows.Consume();
            Owner.Mana.Consume(Arrow.RequiredMana);

            Arrow arrowInstance = Instantiate(Arrow, origin, Quaternion.identity);

            Rigidbody2D arrowBody = arrowInstance.GetComponent<Rigidbody2D>();
            
            arrowBody.AddForce(direction * CurrentCharge * Force);

            CurrentCharge = InitialCharge;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Collector.Ammo? GetAmmo()
        {
            return Owner.Arrows;
        }
    }
}