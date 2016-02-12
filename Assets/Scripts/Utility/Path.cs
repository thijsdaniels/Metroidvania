using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Path : MonoBehaviour {

	public List<Transform> points;

	public enum LoopMode
    {
		pingPong,
		loop
	}
	public LoopMode loopMode;

    public enum Direction
    {
        Forward,
        Backward
    }
    private Direction direction = Direction.Forward;

	public void Start() {
		points = points.Where(e => e != null).ToList();
	}

	public IEnumerator<Transform> PointsEnumerator() {

		if (points == null || points.Count < 1) {
			yield break;
		}

		this.direction = Direction.Forward;
		var index = 0;

		while (true) {

			yield return points[index];

			if (points.Count == 1) {
				continue;
			}

			if (loopMode == LoopMode.pingPong) {

				if (index <= 0) {
                    this.direction = Direction.Forward;
                } else if (index >= points.Count - 1) {
                    this.direction = Direction.Backward;
                }

				index = index + (this.direction.Equals(Direction.Forward) ? 1 : -1);

			} else if (loopMode == LoopMode.loop) {

				if (index >= points.Count - 1) {
					index = 0;
				} else {
					index++;
				}

			}

		}

	}

	public void OnDrawGizmos() {

		points = points.Where(e => e != null).ToList();

		if (points.Count < 2) {
			return;
		}

		if (loopMode == LoopMode.pingPong) {

			for (var i = 0; i < points.Count - 1; i++) {
				Gizmos.DrawLine(points[i].position, points[i + 1].position);
			}

		} else if (loopMode == LoopMode.loop) {

			for (var i = 0; i < points.Count; i++) {
				var fromPoint = points[i];
				var toPoint = i < points.Count - 1 ? points[i + 1] : points[0];
				Gizmos.DrawLine(fromPoint.position, toPoint.position);
			}

		}

	}

    public Direction GetDirection()
    {
        return this.direction;
    }

}
