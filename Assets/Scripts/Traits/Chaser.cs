using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    public class Chaser : MonoBehaviour
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
        public Transform Target;
        public Vector2 TargetOffset;
        public bool StartOnTarget;
        public bool FaceTarget;

        /// <summary>
        /// 
        /// </summary>
        public bool ChaseXAxis;
        public bool ChaseYAxis;
        public MoveModes MoveMode = MoveModes.Linear;

        /// <summary>
        /// 
        /// </summary>
        public bool Asleep;
        public float WakeDistance;
        private float WakeDistanceSquared;

        /// <summary>
        /// 
        /// </summary>
        private bool Stopped;

        /// <summary>
        /// 
        /// </summary>
        [Range(0, 30)] public float Speed;

        /// <summary>
        /// 
        /// </summary>
        void Awake()
        {
            WakeDistanceSquared = WakeDistance * WakeDistance;
            
            if (Target && StartOnTarget)
            {
                transform.position = GetTargetPosition();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void Update()
        {
            if (!Target || Stopped)
            {
                return;
            }

            if (!Asleep)
            {
                Move();
            }
            else
            {
                Sleep();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void Sleep()
        {
            var distanceSquared = (transform.position - Target.position).sqrMagnitude;

            if (distanceSquared < WakeDistanceSquared)
            {
                WakeUp();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void WakeUp()
        {
            Asleep = false;

            gameObject.SendMessage("OnWakeUp", null, SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void FallAsleep()
        {
            Asleep = true;

            gameObject.SendMessage("OnFallAsleep", null, SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Move()
        {
            if (this.FaceTarget)
            {
                transform.localScale = new Vector3(
                    Mathf.Abs(transform.localScale.x) * GetTargetPosition().x > transform.position.x ? 1 : -1,
                    transform.localScale.y,
                    transform.localScale.z
                );
            }


            if (MoveMode == MoveModes.Linear)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    GetTargetPosition(),
                    Speed * Time.deltaTime
                );
            }
            else if (MoveMode == MoveModes.Interpolated)
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    GetTargetPosition(),
                    Speed * Time.deltaTime
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected Vector3 GetTargetPosition()
        {
            return new Vector3(
                ChaseXAxis ? Target.position.x + TargetOffset.x : transform.position.x,
                ChaseYAxis ? Target.position.y + TargetOffset.y : transform.position.y,
                transform.position.z
            );
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnDrawGizmos()
        {
            if (Asleep)
            {
                Gizmos.DrawWireSphere(transform.position, WakeDistance);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnFlinch()
        {
            Stopped = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnFlinchEnd()
        {
            Stopped = false;
        }
    }
}