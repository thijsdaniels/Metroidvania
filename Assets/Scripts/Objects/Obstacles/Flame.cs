using UnityEngine;

namespace Objects.Obstacles
{
    /// <summary>
    /// 
    /// </summary>
    public class Flame : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private ParticleSystem[] Emitters;
        
        /// <summary>
        /// 
        /// </summary>
        public bool Lit = true;

        /// <summary>
        /// 
        /// </summary>
        public void Awake()
        {
            Emitters = gameObject.GetComponentsInChildren<ParticleSystem>();

            if (Lit)
            {
                Light();
            }
            else
            {
                Extinguish();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Light()
        {
            Lit = true;

            foreach (ParticleSystem emitter in Emitters)
            {
                emitter.Play();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Extinguish()
        {
            Lit = false;

            foreach (ParticleSystem emitter in Emitters)
            {
                emitter.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsLit()
        {
            return Lit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (IsLit())
            {
                other.SendMessage("OnFlameEnter", this, SendMessageOptions.DontRequireReceiver);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnFlameEnter(Flame other)
        {
            Light();
        }
    }
}