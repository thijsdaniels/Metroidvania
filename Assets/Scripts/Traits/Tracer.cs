using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    public class Tracer : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public enum MoveModes
        {
            Linear,
            Interpolated
        }

        /// <summary>
        /// 
        /// </summary>
        public MoveModes MoveMode = MoveModes.Linear;
        public Path Path;
        public float Speed = 1;
        public float ProximityThreshold = 0.001f;
        public float PauseDuration = 1f;

        /// <summary>
        /// 
        /// </summary>
        private IEnumerator<Transform> CurrentPoint;
        private bool Paused;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (Path == null)
            {
                Debug.LogError("No path set for tracer.", gameObject);
                return;
            }

            CurrentPoint = Path.PointsEnumerator();
            CurrentPoint.MoveNext();

            if (CurrentPoint.Current == null)
            {
                return;
            }

            // move to first point
            transform.position = CurrentPoint.Current.position;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            if (Paused)
            {
                return;
            }

            if (CurrentPoint == null || CurrentPoint.Current == null)
            {
                return;
            }

            if (MoveMode == MoveModes.Linear)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    CurrentPoint.Current.position,
                    Speed * Time.deltaTime
                );
            }
            else if (MoveMode == MoveModes.Interpolated)
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    CurrentPoint.Current.position,
                    Speed * Time.deltaTime
                );
            }

            float distanceSquared = (transform.position - CurrentPoint.Current.position).sqrMagnitude;

            if (distanceSquared < ProximityThreshold * ProximityThreshold)
            {
                StartCoroutine(PointReached());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator PointReached()
        {
            Paused = true;
            SendMessage("OnTracerPause", null, SendMessageOptions.DontRequireReceiver);

            yield return new WaitForSeconds(PauseDuration);
            CurrentPoint.MoveNext();

            Paused = false;
            SendMessage("OnTracerContinue", Path.GetDirection(), SendMessageOptions.DontRequireReceiver);
        }
    }
}