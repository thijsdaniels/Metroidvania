using System;
using System.Collections;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    public class Fleeting : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public enum Modes
        {
            Animator,
            ParticleSystem,
            Collision
        }

        /// <summary>
        /// 
        /// </summary>
        public Modes Mode = Modes.Animator;
        
        /// <summary>
        /// 
        /// </summary>
        public float Delay;

        /// <summary>
        /// 
        /// </summary>
        private ParticleSystem ParticleSystem;
        
        /// <summary>
        /// 
        /// </summary>
        public LayerMask CollisionLayerMask;
        
        /// <summary>
        /// 
        /// </summary>
        public bool Active = true;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            switch (Mode)
            {
                case Modes.ParticleSystem:
                {
                    ParticleSystem = GetComponent<ParticleSystem>();

                    if (!ParticleSystem)
                    {
                        Debug.LogWarning("Fleeting mode 'ParticleSystem' requires a ParticleSystem component.");
                    }

                    break;
                }
                case Modes.Animator:
                {
                    if (!GetComponent<Animator>())
                    {
                        Debug.LogWarning("Fleeting mode 'Animator' requires a Animator component.");
                    }

                    break;
                }
                case Modes.Collision:
                {
                    if (!GetComponent<Collider2D>())
                    {
                        Debug.LogWarning("Fleeting mode 'Collision' requires a Collider component.");
                    }

                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(Mode), Mode.ToString(), "Invalid Mode.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Update()
        {
            if (!Active)
            {
                return;
            }

            switch (Mode)
            {
                case Modes.Animator:
                {
                    break;
                }
                case Modes.ParticleSystem:
                {
                    if (!ParticleSystem)
                    {
                        throw new Exception("Cannot use Modes.ParticlesSystem without a ParticleSystem.");
                    }
                    
                    if (!ParticleSystem.IsAlive())
                    {
                        SendMessage("OnFleetingDelay", null, SendMessageOptions.RequireReceiver);
                
                        ParticleSystem = null;
                    }

                    break;
                }
                case Modes.Collision:
                {
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(Mode), Mode.ToString(), "Invalid Mode.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Activate()
        {
            Active = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnAnimationEnd()
        {
            if (!Active)
            {
                return;
            }

            if (!Mode.Equals(Modes.Animator))
            {
                return;
            }

            SendMessage("OnFleetingDelay", null, SendMessageOptions.RequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collision"></param>
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (!Active)
            {
                return;
            }

            if (!Mode.Equals(Modes.Collision))
            {
                return;
            }

            if (CollisionLayerMask == (CollisionLayerMask | 1 << collision.gameObject.layer))
            {
                SendMessage("OnFleetingDelay", null, SendMessageOptions.RequireReceiver);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collider"></param>
        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (!Active)
            {
                return;
            }

            if (!Mode.Equals(Modes.Collision))
            {
                return;
            }

            if (CollisionLayerMask == (CollisionLayerMask | 1 << collider.gameObject.layer))
            {
                SendMessage("OnFleetingDelay", null, SendMessageOptions.RequireReceiver);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IEnumerator OnFleetingDelay()
        {
            yield return new WaitForSeconds(Delay);

            SendMessage("OnFleetingEnd", null, SendMessageOptions.RequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnFleetingEnd()
        {
            Destroy(gameObject);
        }
    }
}