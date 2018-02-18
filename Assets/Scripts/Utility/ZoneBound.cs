using Traits;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Chaser))]
    public class ZoneBound : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public float Margin = 0.5f;

        /// <summary>
        /// 
        /// </summary>
        public void LateUpdate()
        {
            Zone zone = ZoneManager.GetCurrentZone();

            if (zone == null)
            {
                return;
            }

            float vSize = Camera.main.orthographicSize;
            float hSize = vSize * Screen.width / Screen.height;

            Rect cameraExtents = new Rect(
                Camera.main.transform.position.x - hSize,
                Camera.main.transform.position.y - vSize,
                hSize * 2,
                vSize * 2
            );

            Rect zoneExtents = zone.GetExtents();

            float overshootLeft = zoneExtents.x - cameraExtents.x - Margin;
            float overshootRight = (cameraExtents.x + cameraExtents.width) - (zoneExtents.x + zoneExtents.width) - Margin;
            float overshootTop = zoneExtents.y - cameraExtents.y - Margin;
            float overshootBottom = (cameraExtents.y + cameraExtents.height) - (zoneExtents.y + zoneExtents.height) - Margin;

            Vector3 newPosition = Camera.main.transform.position;

            if (overshootLeft > 0)
            {
                newPosition.x += overshootLeft;
            }

            if (overshootRight > 0)
            {
                newPosition.x -= overshootRight;
            }

            if (overshootBottom > 0)
            {
                newPosition.y -= overshootBottom;
            }

            if (overshootTop > 0)
            {
                newPosition.y += overshootTop;
            }

            Camera.main.transform.position = newPosition;
        }
    }
}