using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Stalker))]

public class ZoneBound : MonoBehaviour
{
    public float margin = 0.5f;

    public void LateUpdate()
    {
        float vSize = Camera.main.orthographicSize;
        float hSize = vSize * Screen.width / Screen.height;

        Rect cameraExtents = new Rect(
            Camera.main.transform.position.x - hSize,
            Camera.main.transform.position.y - vSize,
            hSize * 2,
            vSize * 2
        );

        Rect zoneExtents = ZoneManager.GetCurrentZone().GetExtents();

        float overshootLeft = zoneExtents.x - cameraExtents.x - margin;
        float overshootRight = (cameraExtents.x + cameraExtents.width) - (zoneExtents.x + zoneExtents.width) - margin;
        float overshootTop = zoneExtents.y - cameraExtents.y - margin;
        float overshootBottom = (cameraExtents.y + cameraExtents.height) - (zoneExtents.y + zoneExtents.height) - margin;

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
