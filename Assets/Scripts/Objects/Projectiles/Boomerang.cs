using UnityEngine;
using Traits;

namespace Objects.Projectiles
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Fleeting))]
    public class Boomerang : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [HideInInspector] public Collector Owner;
        [HideInInspector] public Collectables.Items.Boomerang BoomerangItem;

        /// <summary>
        /// 
        /// </summary>
        protected Rigidbody2D Body;
        protected float PreviousDistance;
        protected bool Returning;

        /// <summary>
        /// 
        /// </summary>
        public float ReturnDistanceFactor = 0.25f;
        public float ReturnForceFactor = 2f;

        /// <summary>
        /// 
        /// </summary>
        public float RotationsPerSecond = 2f;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Body = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void FixedUpdate()
        {
            Spin();
            ReturnToOwner();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Spin()
        {
            transform.Rotate(new Vector3(0, 0, 360 * RotationsPerSecond * Time.deltaTime));
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ReturnToOwner()
        {
            if (!Owner)
            {
                return;
            }

            Vector3 difference = Owner.transform.position - transform.position;

            if (!Returning)
            {
                float distance = difference.magnitude;

                if (distance < PreviousDistance)
                {
                    Returning = true;
                    GetComponent<Collider2D>().isTrigger = true;
                }
                else
                {
                    PreviousDistance = distance;
                }
            }

            Vector3 direction = difference.normalized + difference * ReturnDistanceFactor;

            Vector2 force = new Vector2(
                direction.x,
                direction.y
            ) * ReturnForceFactor;

            Body.AddForce(force);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collider"></param>
        public void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.CompareTag("Player"))
            {
                GetComponent<Fleeting>().Activate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void OnFleetingEnd()
        {
            BoomerangItem.ProjectileCount--;
        }
    }
}