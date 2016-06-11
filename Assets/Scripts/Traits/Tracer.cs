using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tracer : MonoBehaviour
{
	public enum MoveMode
    {
		linear,
		interpolated
	}

	public MoveMode moveMode = MoveMode.linear;
	public Path path;
	public float speed = 1;
	public float proximityThreshold = 0.001f;
    public float pauseDuration = 1f;

	private IEnumerator<Transform> currentPoint;
    private bool paused = false;

	public void Start()
    {
		if (path == null)
        {
			Debug.LogError("Path cannot be null", gameObject);
		}

		currentPoint = path.PointsEnumerator();
		currentPoint.MoveNext();

		if (currentPoint.Current == null)
        {
			return;
		}

		// move to first point
		transform.position = currentPoint.Current.position;
	}

	public void Update()
    {
        if (paused)
        {
            return;
        }

		if (currentPoint == null || currentPoint.Current == null)
        {
			return;
		}

		if (moveMode == MoveMode.linear)
        {
			transform.position = Vector3.MoveTowards(
				transform.position,
				currentPoint.Current.position,
				speed * Time.deltaTime
			);
		}
        else if (moveMode == MoveMode.interpolated)
        {
			transform.position = Vector3.Lerp(
				transform.position,
				currentPoint.Current.position,
				speed * Time.deltaTime
			);
		}

		var distanceSquared = (transform.position - currentPoint.Current.position).sqrMagnitude;

		if (distanceSquared < proximityThreshold * proximityThreshold)
        {
            StartCoroutine(this.PointReached());
		}

	}

    IEnumerator PointReached()
    {
        this.paused = true;
        this.SendMessage("OnTracerPause", null, SendMessageOptions.DontRequireReceiver);

        yield return new WaitForSeconds(this.pauseDuration);
        currentPoint.MoveNext();

        this.paused = false;
        this.SendMessage("OnTracerContinue", this.path.GetDirection(), SendMessageOptions.DontRequireReceiver);
    }
}
