using System.Collections;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// 
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private Rigidbody2D Body;

        /// <summary>
        /// 
        /// </summary>
        public float Speed;

        /// <summary>
        /// 
        /// </summary>
        public float FuseDuration;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Body = GetComponent<Rigidbody2D>();

            if (FuseDuration > 0)
            {
                StartCoroutine(OnFuseLit());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            var velocity = new Vector3(
                Speed * transform.localScale.x,
                Body.velocity.y
            );

            Body.velocity = velocity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter2D(Collider2D other)
        {
            OnHit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator OnFuseLit()
        {
            yield return new WaitForSeconds(FuseDuration);

            OnHit();
        }

        /// <summary>
        /// 
        /// </summary>
        void OnHit()
        {
            Destroy(gameObject);
        }
    }
}