using UnityEngine;
using System.Collections;
using BoomerangItem = Objects.Collectables.Items.Boomerang;

namespace Objects.Projectiles
{
    /**
     * 
     */
    public class Boomerang : MonoBehaviour
    {
        [HideInInspector] public Collector owner;
        [HideInInspector] public BoomerangItem boomerangItem;
        protected Rigidbody2D body;
        protected float previousDistance;
        protected bool returning;

        public float returnDistanceFactor = 0.25f;
        public float returnForceFactor = 2f;

        public float rotationsPerSecond = 2f;

        /**
         * 
         */
        public void Start()
        {
            body = GetComponent<Rigidbody2D>();
        }

        /**
         * 
         */
        public void Update()
        {
            Spin();
            ReturnToOwner();
        }

        /**
         * 
         */
        protected void Spin()
        {
            transform.Rotate(new Vector3(0, 0, 360 * rotationsPerSecond * Time.deltaTime));
        }

        /**
         * 
         */
        protected void ReturnToOwner()
        {
            if (!owner)
            {
                return;
            }

            Vector3 difference = owner.transform.position - transform.position;

            if (!returning)
            {
                float distance = difference.magnitude;

                if (distance < previousDistance)
                {
                    returning = true;
                    GetComponent<Collider2D>().isTrigger = true;
                }
                else
                {
                    previousDistance = distance;
                }
            }

            Vector3 direction = difference.normalized + difference * returnDistanceFactor;

            Vector2 force = new Vector2(
                direction.x,
                direction.y
            ) * returnForceFactor;

            body.AddForce(force);
        }

        /**
         * 
         */
        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (returning && collider.gameObject.tag == "Player")
            {
                OnCaught();
            }
        }

        /**
         * 
         */
        protected void Deflect()
        {
            // TODO: Deflect velocity.
        }

        protected void OnCaught()
        {
            boomerangItem.projectileCount--;

            Destroy(gameObject);
        }
    }
}
