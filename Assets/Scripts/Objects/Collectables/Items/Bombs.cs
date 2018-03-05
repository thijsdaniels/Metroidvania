using Objects.Projectiles;
using Physics;
using Traits;
using UnityEngine;

namespace Objects.Collectables.Items
{
    /// <summary>
    /// 
    /// </summary>
    public class Bombs : Item
    {
        /// <summary>
        /// 
        /// </summary>
        protected float CurrentCharge;
        public float InitialCharge = 1.5f;
        public float ChargeFactor = 3f;
        public float MaxCharge = 3f;

        /// <summary>
        /// 
        /// </summary>
        protected const float Force = 10f;
        public float CoolDownDuration = 0.25f;

        /// <summary>
        /// 
        /// </summary>
        public Bomb Bomb;
        protected Bomb BombInstance;

        /// <summary>
        /// 
        /// </summary>
        [HideInInspector] public int BombCount;
        [SerializeField] private int MaxBombCount = 3;

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

            Owner.Bombs.Upgrade(20);
            Owner.Bombs.Restore();
            Owner.Bombs.Enable();
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
            
            if (!Owner.Bombs.Available() || BombCount >= MaxBombCount)
            {
                return false;
            }

            Body body = Owner.GetComponent<Body>();
            
            if (
                body.State.IsRolling() ||
                body.State.IsSwimming() ||
                body.State.IsClimbing()
            )
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
            if (BombInstance || !IsCooledDown() || !CanBeUsed())
            {
                return;
            }

            Bomb bomb = DrawBomb();

            Carrier carrier = Owner.GetComponent<Carrier>();
            Aimer aimer = Owner.GetComponent<Aimer>();
            
            carrier.Grab(bomb);
            aimer.StartAiming();

            bomb.LightFuse();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected Bomb DrawBomb()
        {
            Owner.Bombs.Consume();

            BombInstance = Instantiate(Bomb, Owner.transform.position, Quaternion.identity) as Bomb;

            BombCount++;
            BombInstance.Origin = this;

            return BombInstance;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnHold()
        {
            if (!BombInstance)
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
            if (!BombInstance)
            {
                return;
            }

            Thrower thrower = Owner.GetComponent<Thrower>();
            Aimer aimer = Owner.GetComponent<Aimer>();
            
            thrower.Throw(aimer.GetAimingDirection() * CurrentCharge * Force);
            aimer.StopAiming();

            BombInstance = null;
            CurrentCharge = InitialCharge;
            SetCoolDown(CoolDownDuration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Collector.Ammo? GetAmmo()
        {
            return Owner.Bombs;
        }
    }
}