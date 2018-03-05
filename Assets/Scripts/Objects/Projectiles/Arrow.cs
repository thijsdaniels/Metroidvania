using System.Collections;
using Traits;
using UnityEngine;

namespace Objects.Projectiles
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))] 
    public class Arrow : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private Vector3 LastPosition;

        /// <summary>
        /// 
        /// </summary>
        public float DespawnDelay = 1.5f;
        public float TimeoutDelay = 30f;

        /// <summary>
        /// 
        /// </summary>
        public int RequiredMana;

        /// <summary>
        /// 
        /// </summary>
        public void Awake()
        {
            StartCoroutine(Timeout());

            LastPosition = transform.position;
        }

        /// <summary>
        /// 
        /// </summary>
        public void FixedUpdate()
        {
            PointInDirection();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void PointInDirection()
        {
            Vector3 direction = gameObject.transform.position - LastPosition;

            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            LastPosition = transform.position;
        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="other"></param>
//        public void OnTriggerEnter2D(Collider2D other)
//        {
//            if (other.isTrigger || other.gameObject.CompareTag("Player"))
//            {
//                return;
//            }
//
//            OnHit();
//        }

        /// <summary>
        /// 
        /// </summary>
        protected void OnFleetingDelay()
        {
            var body = GetComponent<Rigidbody2D>();
            body.isKinematic = true;

            Destroy(gameObject.GetComponent<Untouchable>());
        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        protected IEnumerator Despawn()
//        {
//        	yield return new WaitForSeconds(despawnDelay);
//        
//        	Destroy(gameObject);
//        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IEnumerator Timeout()
        {
            yield return new WaitForSeconds(TimeoutDelay);

            Destroy(gameObject);
        }
    }
}