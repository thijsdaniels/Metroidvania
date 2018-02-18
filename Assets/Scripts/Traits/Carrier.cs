using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    public class Carrier : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private Carriable Carriable;

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            Carry();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="carriable"></param>
        public void Grab(Carriable carriable)
        {
            var body = carriable.GetComponent<Rigidbody2D>();
            
            if (body)
            {
                body.isKinematic = true;
            }

            Carriable = carriable;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Carry()
        {
            if (!Carriable)
            {
                return;
            }

            Carriable.transform.position = transform.position;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Carriable Release()
        {
            Carriable carriable = Carriable;
            Carriable = null;

            var body = carriable.GetComponent<Rigidbody2D>();
            
            if (body)
            {
                body.isKinematic = false;
            }

            return carriable;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Drop()
        {
            var carriable = Release();

            carriable.transform.position = new Vector3(
                transform.position.x,
                transform.position.y - transform.localScale.y / 2,
                transform.position.z
            );
        }
    }
}