using Character;
using Physics;
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

            Body body = carriable.GetComponent<Body>();
            
            if (body)
            {
                body.AddVelocity(force);
            }
        }
    }
}