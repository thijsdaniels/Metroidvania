using Character;
using Physics;
using UnityEngine;

namespace Objects.Platforms
{
    /// <summary>
    /// 
    /// </summary>
    public class ConveyorPlatform : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [Range(-10, 10)] public float Velocity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        void OnBodyStay(Body body)
        {
            body.AddExternalHorizontalVelocity(Velocity);
        }
    }
}