using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class Path : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Transform> Points;

        /// <summary>
        /// 
        /// </summary>
        public enum LoopModes
        {
            PingPong,
            Loop
        }

        /// <summary>
        /// 
        /// </summary>
        public LoopModes LoopMode;

        /// <summary>
        /// 
        /// </summary>
        public enum Directions
        {
            Forward,
            Backward
        }

        /// <summary>
        /// 
        /// </summary>
        private Directions Direction = Directions.Forward;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Points = Points.Where(e => e != null).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Transform> PointsEnumerator()
        {
            if (Points == null || Points.Count < 1)
            {
                yield break;
            }

            Direction = Directions.Forward;
            int index = 0;

            while (true)
            {
                yield return Points[index];

                if (Points.Count == 1)
                {
                    continue;
                }

                if (LoopMode == LoopModes.PingPong)
                {
                    if (index <= 0)
                    {
                        Direction = Directions.Forward;
                    }
                    else if (index >= Points.Count - 1)
                    {
                        Direction = Directions.Backward;
                    }

                    index = index + (Direction.Equals(Directions.Forward) ? 1 : -1);
                }
                else if (LoopMode == LoopModes.Loop)
                {
                    if (index >= Points.Count - 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnDrawGizmos()
        {
            Points = Points.Where(e => e != null).ToList();

            if (Points.Count < 2)
            {
                return;
            }

            switch (LoopMode)
            {
                case LoopModes.PingPong:
                {
                    for (var i = 0; i < Points.Count - 1; i++)
                    {
                        Gizmos.DrawLine(Points[i].position, Points[i + 1].position);
                    }

                    break;
                }
                case LoopModes.Loop:
                {
                    for (var i = 0; i < Points.Count; i++)
                    {
                        Transform from = Points[i];
                        Transform to = i < Points.Count - 1 ? Points[i + 1] : Points[0];
                        
                        Gizmos.DrawLine(from.position, to.position);
                    }

                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(LoopMode), LoopMode.ToString(), "Invalid LoopMode.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Directions GetDirection()
        {
            return Direction;
        }
    }
}