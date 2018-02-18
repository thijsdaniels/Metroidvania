using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Carrier))]
    public class Thrower : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private Carrier Carrier;

        /// <summary>
        /// 
        /// </summary>
        public void Awake()
        {
            Carrier = GetComponent<Carrier>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="force"></param>
        public void Throw(Vector2 force)
        {
            Carriable carriable = Carrier.Release();

            Rigidbody2D body = carriable.GetComponent<Rigidbody2D>();
            
            if (body)
            {
                body.AddForce(force);
            }
        }
    }
}