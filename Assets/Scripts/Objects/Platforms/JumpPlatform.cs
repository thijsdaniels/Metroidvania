using Character;
using UnityEngine;

namespace Objects.Platforms
{
    /// <summary>
    /// 
    /// </summary>
    public class JumpPlatform : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public float JumpVelocity = 10f;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        void OnCharacterControllerEnter2D(CharacterController2D other)
        {
            other.SetVerticalVelocity(JumpVelocity);
        }
    }
}