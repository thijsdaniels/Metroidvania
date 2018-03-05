using Character;
using Physics;
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
        /// <param name="body"></param>
        void OnBodyEnter(Body body)
        {
            body.SetVerticalVelocity(JumpVelocity);
        }
    }
}