using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    public class Damagable : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public int InitialHealth;
        public int CurrentHealth { get; protected set; }
        public int MaximumHealth;

        /// <summary>
        /// 
        /// </summary>
        public bool Flinch;
        protected Animator Animator;

        /// <summary>
        /// 
        /// </summary>
        public bool DestroyOnDeath = true;

        /// <summary>
        /// 
        /// </summary>
        public AudioClip HitSound;
        public float HitTimeout;
        protected float CurrentHitTimeout;
        protected SpriteRenderer SpriteRenderer;
        protected Color OriginalColor;

        /// <summary>
        /// 
        /// </summary>
        public AudioClip DeathSound;
        public Fleeting DeathResidue;
        public Vector2 DeathResidueOffset = Vector2.zero;

        /// <summary>
        /// 
        /// </summary>
        [HideInInspector] public bool Dodging;
        [HideInInspector] public bool Invulnerable;

        /// <summary>
        /// 
        /// </summary>
        public void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            CurrentHealth = InitialHealth;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            if (CurrentHitTimeout > 0)
            {
                CurrentHitTimeout = Mathf.Max(0, CurrentHitTimeout - Time.deltaTime);

                if (CurrentHitTimeout <= 0 && SpriteRenderer)
                {
                    SpriteRenderer.color = OriginalColor;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cause"></param>
        /// <param name="damage"></param>
        public void TakeDamage(Damager cause, int damage)
        {
            if (!CanBeHit(cause))
            {
                return;
            }

            CurrentHealth = Mathf.Max(0, CurrentHealth - damage);

            if (CurrentHealth > 0)
            {
                Hit();
            }
            else
            {
                Die();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cause"></param>
        /// <returns></returns>
        protected bool CanBeHit(Damager cause)
        {
            if (Invulnerable)
            {
                return false;
            }

            if (Immune(cause))
            {
                return false;
            }

            if (cause.Dodgeable && Dodging)
            {
                return false;
            }

            if (CurrentHitTimeout > 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cause"></param>
        /// <returns></returns>
        protected virtual bool Immune(Damager cause)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Kill()
        {
            CurrentHealth = 0;
            
            Die();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        public void Heal(int amount)
        {
            CurrentHealth = Mathf.Min(MaximumHealth, CurrentHealth + amount);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Restore()
        {
            CurrentHealth = MaximumHealth;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Revive()
        {
            CurrentHealth = InitialHealth;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Hit()
        {
            SendMessage("OnHit", null, SendMessageOptions.DontRequireReceiver);

            if (HitSound)
            {
                AudioSource.PlayClipAtPoint(HitSound, transform.position);
            }

            if (Flinch)
            {
                Animator.SetTrigger("Flinch");
            }

            if (HitTimeout > 0)
            {
                CurrentHitTimeout = HitTimeout;

                if (SpriteRenderer)
                {
                    OriginalColor = SpriteRenderer.color;
                    Color newColor = new Color(OriginalColor.r, OriginalColor.g, OriginalColor.b, OriginalColor.a * 0.5f);
                    SpriteRenderer.color = newColor;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnFlinchAnimationStart()
        {
            SendMessage("OnFlinch", null, SendMessageOptions.RequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnFlinchAnimationEnd()
        {
            SendMessage("OnFlinchEnd", null, SendMessageOptions.RequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Die()
        {
            SendMessage("OnDeath", this, SendMessageOptions.DontRequireReceiver);

            if (DeathSound)
            {
                AudioSource.PlayClipAtPoint(DeathSound, transform.position);
            }

            if (DeathResidue)
            {
                Vector3 deathResiduePosition = new Vector3(
                    transform.position.x + transform.localScale.x / 2 + DeathResidueOffset.x,
                    transform.position.y + transform.localScale.y / 2 + DeathResidueOffset.y,
                    transform.position.z
                );
                
                Instantiate(DeathResidue, deathResiduePosition, Quaternion.identity);
            }

            if (DestroyOnDeath)
            {
                Destroy(gameObject);
            }
        }
    }
}