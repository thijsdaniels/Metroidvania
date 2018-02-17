using System;
using UnityEngine;

namespace Physics
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PhysicsVolumeParameters2D
    {
        /// <summary>
        /// 
        /// </summary>
        public enum JumpModes
        {
            None,
            Ground,
            Anywhere
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Range(100f, -100f)] public float Gravity = -30f;

        /// <summary>
        /// 
        /// </summary>
        public Vector2 MaximumVelocity = new Vector2(20f, 20f);
        [Range(0f, 1f)] public float Damping = 0.01f;

        /// <summary>
        /// Jumping 
        /// </summary>
        public JumpModes JumpMode = JumpModes.Ground;
        [Range(0f, 2f)] public float JumpModifier = 1f;
    }
}