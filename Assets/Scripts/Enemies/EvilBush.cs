using System.Collections;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// 
    /// </summary>
    public class EvilBush : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private Animator Animator;

        /// <summary>
        /// 
        /// </summary>
        public Transform Target;
        
        /// <summary>
        /// 
        /// </summary>
        public float AttackDelay;
        
        /// <summary>
        /// 
        /// </summary>
        public Projectile Projectile;
        
        /// <summary>
        /// 
        /// </summary>
        public float ProjectileSpeed;
        
        /// <summary>
        /// 
        /// </summary>
        public AudioClip ShootSound;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            // get a reference to the animator
            Animator = GetComponent<Animator>();

            // start the attack co-routine
            if (AttackDelay > 0)
            {
                StartCoroutine(OnAttack());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator OnAttack()
        {
            // wait out the attack interval
            yield return new WaitForSeconds(AttackDelay);

            // shoot a projectile
            Attack();

            // restart the attack co-routine
            StartCoroutine(OnAttack());
        }

        /// <summary>
        /// 
        /// </summary>
        private void Attack()
        {
            if (Target)
            {
                Animator.SetTrigger("Attack");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnShoot()
        {
            if (!Target || !Projectile)
            {
                return;
            }
        
            // create a projectile
            Projectile projectileInstance = Instantiate(Projectile, transform.position, Quaternion.identity);

            // determine the direction to the target
            int direction = Target.position.x > transform.position.x ? 1 : -1;

            // flip the projectile in the target's direction
            projectileInstance.transform.localScale = new Vector3(
                projectileInstance.transform.localScale.x * direction,
                projectileInstance.transform.localScale.y,
                projectileInstance.transform.localScale.z
            );

            // set the projectile's speed
            projectileInstance.Speed = ProjectileSpeed;

            // play the shoot sound
            if (ShootSound)
            {
                AudioSource.PlayClipAtPoint(ShootSound, transform.position);
            }
        }
    }
}