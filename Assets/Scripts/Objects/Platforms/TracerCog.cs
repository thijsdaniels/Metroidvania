using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Tracer))]

public class TracerCog : MonoBehaviour
{
    public float degreesPerSecond = 90f;

    private float rotation;

    public void Update()
    {
        transform.Rotate(0, 0, this.rotation * Time.deltaTime);
    }

    public void OnTracerContinue(Path.Direction direction)
    {
        this.rotation = degreesPerSecond * (direction.Equals(Path.Direction.Forward) ? 1 : -1);
    }

    public void OnTracerPause()
    {
        this.rotation = 0f;
    }
}
