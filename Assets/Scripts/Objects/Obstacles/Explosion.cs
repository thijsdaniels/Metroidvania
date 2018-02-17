using UnityEngine;

namespace Objects.Obstacles
{
    /// <summary>
    /// 
    /// </summary>
    public class Explosion : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public AudioClip Sound;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (Sound)
            {
                AudioSource.PlayClipAtPoint(Sound, transform.position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter2D(Collider2D other)
        {
            other.SendMessage("OnExplosionEnter", this, SendMessageOptions.DontRequireReceiver);
        }
    }
}