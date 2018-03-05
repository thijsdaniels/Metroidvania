using Character;
using Physics;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Body))]
    public class Patroller : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private Body Body;

        /// <summary>
        /// 
        /// </summary>
        public float WalkSpeed;
        public bool AvoidCollisions;
        public bool AvoidGaps;

        /// <summary>
        /// 
        /// </summary>
        public Vector2 CollisionSightStart;
        public Vector2 CollisionSightEnd;

        /// <summary>
        /// 
        /// </summary>
        public Vector2 GapSightStart;
        public Vector2 GapSightEnd;

        /// <summary>
        /// 
        /// </summary>
        private bool Stopped;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Body = GetComponent<Body>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            // turn around if a collision is ahead
            if (AvoidCollisions && CollisionAhead())
            {
                TurnAround();
            }

            // turn around if a collision is ahead
            if (AvoidGaps && GapAhead())
            {
                TurnAround();
            }

            // move
            Move();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected bool CollisionAhead()
        {
            // correct sight start for position and direction
            var correctedSightStart = new Vector2(
                transform.position.x + CollisionSightStart.x * transform.localScale.x,
                transform.position.y + CollisionSightStart.y * transform.localScale.y
            );

            // correct sight end for position and direction
            var correctedSightEnd = new Vector2(
                transform.position.x + CollisionSightEnd.x * transform.localScale.x,
                transform.position.y + CollisionSightEnd.y * transform.localScale.y
            );

            // visualize the line of sight
            Debug.DrawLine(correctedSightStart, correctedSightEnd, Color.green);

            // check if the line of sight collides with the gound layer
            return Physics2D.Linecast(
                correctedSightStart,
                correctedSightEnd,
                Body.PlatformLayerMask | 1 << LayerMask.NameToLayer("Enemies")
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected bool GapAhead()
        {
            // don't scan for gaps in the air
            if (Body && !Body.State.IsGrounded())
            {
                return false;
            }

            // correct sight start for position and direction
            var correctedSightStart = new Vector2(
                transform.position.x + GapSightStart.x * transform.localScale.x,
                transform.position.y + GapSightStart.y * transform.localScale.y
            );

            // correct sight end for position and direction
            var correctedSightEnd = new Vector2(
                transform.position.x + GapSightEnd.x * transform.localScale.x,
                transform.position.y + GapSightEnd.y * transform.localScale.y
            );

            // visualize the line of sight
            Debug.DrawLine(correctedSightStart, correctedSightEnd, Color.green);

            // check if the line of sight does not collide with the gound layer
            return !Physics2D.Linecast(
                correctedSightStart,
                correctedSightEnd,
                Body.PlatformLayerMask | Body.LedgeLayerMask
            );
        }

        /// <summary>
        /// 
        /// </summary>
        protected void TurnAround()
        {
            transform.localScale = new Vector3(
                transform.localScale.x * -1,
                transform.localScale.y,
                transform.localScale.z
            );
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Move()
        {
            var velocity = Vector2.zero;

            if (!Stopped)
            {
                velocity = new Vector2(
                    WalkSpeed * transform.localScale.x,
                    Body.Velocity.y
                );
            }

            Body.SetVelocity(velocity);
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