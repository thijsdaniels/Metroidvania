using UnityEngine;
using BoomerangProjectile = Objects.Projectiles.Boomerang;

namespace Objects.Collectables.Items
{
    /**
     * 
     */
    public class Sword : Item
    {
        public float coolDownDuration = 0.25f;

        public Fleeting projectile;
        public Vector2 projectileOffset;

        /**
	     * 
	     */
        public override bool CanBeUsed()
        {
            if (!base.CanBeUsed())
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

            Swing();
        }

        /**
	     * 
	     */
        protected void Swing()
        {
            SetCoolDown(coolDownDuration);

            Animator animator = owner.GetComponent<Animator>();
            animator.SetTrigger("Downward Slash");
        }
    }
}