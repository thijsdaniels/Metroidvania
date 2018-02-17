using Character;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Interactable))]
    public class Lootable : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private Animator Animator;
        
        /// <summary>
        /// 
        /// </summary>
        private Interactable Interactable;
        
        /// <summary>
        /// 
        /// </summary>
        private bool Looted;

        /// <summary>
        /// 
        /// </summary>
        public Collectable Loot;
        
        /// <summary>
        /// 
        /// </summary>
        public bool DestroyOnLoot;

        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            Animator = GetComponent<Animator>();
            Interactable = GetComponent<Interactable>();

            if (Animator && Looted)
            {
                Animator.SetTrigger("Looted");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public void OnInteraction(Player player)
        {
            var collector = player.gameObject.GetComponent<Collector>();

            if (!Looted && collector)
            {
                CollectLoot(collector);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collector"></param>
        private void CollectLoot(Collector collector)
        {
            if (Animator)
            {
                Animator.SetTrigger("Looted");
            }

            if (Loot)
            {
                collector.Collect(Instantiate(Loot));
                Loot = null;
            }

            Looted = true;

            if (DestroyOnLoot)
            {
                Destroy(gameObject);
            }
            else
            {
                Interactable.Disable();
            }
        }
    }
}