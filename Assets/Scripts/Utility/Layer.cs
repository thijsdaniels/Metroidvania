using UnityEngine;

namespace Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class Layer : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public float Depth;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                Depth
            );
        }
    }
}
