using Traits;
using UnityEngine;

namespace Physics
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Volume : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public VolumeParameters Parameters;

        /// <summary>
        /// 
        /// </summary>
        public bool IsLiquid;

        /// <summary>
        /// 
        /// </summary>
        /// TODO: Doesn't Density make IsLiquid obsolete?
        public float Density = 1f;

        /// <summary>
        /// 
        /// </summary>
        public Fleeting Splash;
        public AudioClip SplashSound;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter2D(Collider2D other)
        {
            TriggerSplash(other.gameObject);
            
            Body body = other.gameObject.GetComponent<Body>();

            if (body)
            {
                body.Volume = this;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerExit2D(Collider2D other)
        {
            TriggerSplash(other.gameObject);
            
            Body body = other.gameObject.GetComponent<Body>();
            
            if (body)
            {
                body.Volume = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cause"></param>
        protected void TriggerSplash(GameObject cause)
        {
            if (Splash)
            {
                Instantiate(Splash, new Vector3(
                    cause.transform.position.x,
                    cause.transform.position.y - cause.transform.localScale.y / 4,
                    cause.transform.position.z
                ), Quaternion.identity);
            }

            if (SplashSound)
            {
                AudioSource.PlayClipAtPoint(SplashSound, transform.position);
            }
        }
    }
}