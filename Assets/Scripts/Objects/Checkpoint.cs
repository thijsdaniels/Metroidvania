using Character;
using UnityEngine;

namespace Objects
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(ParticleSystem))]
    public class Checkpoint : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private Animator Animator;
        
        /// <summary>
        /// 
        /// </summary>
        private ParticleSystem Emitter;
        
        /// <summary>
        /// 
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 SpawnOffset;

        /// <summary>
        /// 
        /// </summary>
        public void Awake()
        {
            Animator = GetComponent<Animator>();
            Emitter = GetComponent<ParticleSystem>();

            Deactivate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (Active)
            {
                return;
            }

            var player = other.GetComponent<Player>();

            if (player)
            {
                Activate(player);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public void Activate(Player player)
        {
            if (player.Checkpoint)
            {
                player.Checkpoint.Deactivate();
            }

            player.Checkpoint = this;

            Active = true;
            Animator.SetBool("Active", true);

            if (!Emitter.isPlaying)
            {
                Emitter.Play();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Deactivate()
        {
            Active = false;
            Animator.SetBool("Active", false);

            if (Emitter.isPlaying)
            {
                Emitter.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public void Respawn(Player player)
        {
            var respawnPosition = transform.position + new Vector3(
                SpawnOffset.x,
                SpawnOffset.y,
                0
            );

            player.transform.position = respawnPosition;

            player.Controller.SetVelocity(Vector2.zero);
        }
    }
}