using Character;
using UnityEngine;

namespace Objects.Collectables.Items
{
    /// <summary>
    /// 
    /// </summary>
    public class Boomerang : Item
    {
        /// <summary>
        /// 
        /// </summary>
        protected float CurrentCharge;
        public float InitialCharge = 2f;
        public float ChargeFactor = 1f;
        public float MaxCharge = 3f;

        /// <summary>
        /// 
        /// </summary>
        protected const float Force = 100f;
        public float CoolDownDuration = 0.25f;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] protected Projectiles.Boomerang Projectile;
        [HideInInspector] public int ProjectileCount;

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
        /// <returns></returns>
        public override bool CanBeUsed()
        {
            if (!base.CanBeUsed())
            {
                return false;
            }

            if (ProjectileCount >= 1)
            {
                return false;
            }

            CharacterController2D controller = Owner.GetComponent<CharacterController2D>();
            
            if (controller.State.IsRolling() || controller.State.IsSwimming() || controller.State.IsClimbing())
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

            Player player = Owner.GetComponent<Player>();
            
            player.StartAiming();
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

            Player player = Owner.GetComponent<Player>();
            
            Throw(player.transform.position, player.GetAimingDirection());

            player.StopAiming();

            SetCoolDown(CoolDownDuration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        protected void Throw(Vector3 origin, Vector2 direction)
        {
            Projectiles.Boomerang projectileInstance = Instantiate(Projectile, origin, Quaternion.identity);

            ProjectileCount++;
            projectileInstance.Owner = Owner;
            projectileInstance.BoomerangItem = this;

            Rigidbody2D projectileBody = projectileInstance.GetComponent<Rigidbody2D>();
            projectileBody.AddForce(direction * CurrentCharge * Force);

            CurrentCharge = InitialCharge;
        }
    }
}