using Character;
using UnityEngine;

namespace Objects.Collectables.Items
{
    /// <summary>
    /// 
    /// </summary>
    public class Sword : Item
    {
        /// <summary>
        /// 
        /// </summary>
        public float CoolDownDuration = 0.25f;

        /// <summary>
        /// 
        /// </summary>
        public override void OnPress()
        {
            if (!IsCooledDown() || !CanBeUsed())
            {
                return;
            }

            Swing();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Swing()
        {
            SetCoolDown(CoolDownDuration);

            Animator animator = Owner.GetComponent<Animator>();
            
            animator.SetTrigger("Downward Slash");
        }
    }
}