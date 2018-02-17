using Character;
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
        /// <param name="other"></param>
        void OnCharacterControllerStay2D(CharacterController2D other)
        {
            other.AddExternalHorizontalVelocity(Velocity);
        }
    }
}