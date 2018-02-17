using UnityEngine;

namespace Utility
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(TrailRenderer))]
    public class TrailLayerer : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        protected TrailRenderer TrailRenderer;

        /// <summary>
        /// 
        /// </summary>
        public string SortingLayerName = "Default";
        
        /// <summary>
        /// 
        /// </summary>
        public int SortingOrder;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            TrailRenderer = GetComponent<TrailRenderer>();

            Apply();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Apply()
        {
            TrailRenderer.sortingLayerName = SortingLayerName;
            TrailRenderer.sortingOrder = SortingOrder;
        }
    }
}