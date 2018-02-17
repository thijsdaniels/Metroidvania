using Traits;
using UnityEngine;
using Utility;

namespace Objects.Platforms
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Tracer))]
    public class TracerCog : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public float DegreesPerSecond = 90f;

        /// <summary>
        /// 
        /// </summary>
        private float Rotation;

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            transform.Rotate(0, 0, Rotation * Time.deltaTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        public void OnTracerContinue(Path.Directions direction)
        {
            Rotation = DegreesPerSecond * (direction.Equals(Path.Directions.Forward) ? 1 : -1);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnTracerPause()
        {
            Rotation = 0f;
        }
    }
}