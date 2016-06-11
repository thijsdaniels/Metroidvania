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

        protected Fleeting swordInstance;

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

            if (swordInstance)
            {
                Destroy(swordInstance.gameObject);
            }

            Vector3 position = new Vector3(
                owner.transform.position.x + (projectileOffset.x * owner.transform.localScale.x),
                owner.transform.position.y + (projectileOffset.y * owner.transform.localScale.y),
                owner.transform.position.z
            );

            swordInstance = Instantiate(projectile, position, Quaternion.identity) as Fleeting;

            swordInstance.transform.localScale = owner.transform.localScale;
            swordInstance.transform.SetParent(owner.transform);
        }
    }
}