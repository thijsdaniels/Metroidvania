using UnityEngine;

namespace Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class PixelArtCamera : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public float PixelsPerUnit;
        
        /// <summary>
        /// 
        /// </summary>
        public int Size;

        /// <summary>
        /// 
        /// </summary>
        void Awake()
        {
            SetOrthographicSize();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetOrthographicSize()
        {
            float pixelPerfectSize = Mathf.Pow(2, Size);
            Camera camera = GetComponent<Camera>();

            camera.orthographicSize = (1 / pixelPerfectSize) * ((Screen.height / 2f) / PixelsPerUnit);
        }
    }
}