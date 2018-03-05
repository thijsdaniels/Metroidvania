using System;
using UnityEngine;

namespace Physics
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PlatformParameters
    {
        /// <summary>
        /// 
        /// </summary>
        [Range(0f, 1f)] public float Traction = 0.5f;
        [Range(0f, 1f)] public float Friction = 0.3f;

        /// <summary>
        /// 
        /// </summary>
        [Range(0f, 90f)] public float SlopeLimit = 60f;
    }
}