using UnityEngine;
using System.Collections;

public class Damagable : MonoBehaviour
{
	public int initialHealth;
	public int currentHealth { get; protected set; }
	public int maximumHealth;

    public bool flinch;
    protected Animator animator;

	public bool destroyOnDeath = true;

	public AudioClip hitSound;
    public float hitTimeout;
    protected float currentHitTimeout;
    protected SpriteRenderer spriteRenderer;
    protected Color originalColor;

    public AudioClip deathSound;
    public Fleeting deathResidue;
    public Vector2 deathResidueOffset = Vector2.zero;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

	public void Start()
    {
		currentHealth = initialHealth;
	}

    public void Update()
    {
        if (currentHitTimeout > 0)
        {
            currentHitTimeout = Mathf.Max(0, currentHitTimeout - Time.deltaTime);

            if (currentHitTimeout <= 0 && spriteRenderer && originalColor != null)
            {
                spriteRenderer.color = originalColor;
            }
        }
    }

	public void TakeDamage(GameObject cause, int damage)
    {
        if (!CanBeHit() || Immune(cause))
        {
            return;
        }
		
		currentHealth = Mathf.Max(0, currentHealth - damage);

		if (currentHealth > 0)
        {
			Hit();
		}
        else
        {
			Die();
		}
	}

    protected bool CanBeHit()
    {
        if (currentHitTimeout > 0)
        {
            return false;
        }

        return true;
    }

    protected virtual bool Immune(GameObject cause)
    {
        return false;
    }

	public void Kill()
    {
		currentHealth = 0;
		Die();
	}

	public void Heal(int amount)
    {
		currentHealth = Mathf.Min(maximumHealth, currentHealth + amount);
	}

	public void Restore()
    {
		currentHealth = maximumHealth;
	}

	public void Revive()
    {
		currentHealth = initialHealth;
	}

	protected void Hit()
    {
		SendMessage("OnHit", null, SendMessageOptions.DontRequireReceiver);

		if (hitSound)
        {
			AudioSource.PlayClipAtPoint(hitSound, transform.position);
		}

        if (flinch)
        {
            animator.SetTrigger("Flinch");
        }

        if (hitTimeout > 0)
        {
            currentHitTimeout = hitTimeout;

            if (spriteRenderer)
            {
                originalColor = spriteRenderer.color;
                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a * 0.5f);
                spriteRenderer.color = newColor;
            }
        }
	}

    private void OnFlinchAnimationStart()
    {
        SendMessage("OnFlinch", null, SendMessageOptions.RequireReceiver);
    }

    private void OnFlinchAnimationEnd()
    {
        SendMessage("OnFlinchEnd", null, SendMessageOptions.RequireReceiver);
    }

	protected void Die()
    {
		SendMessage("OnDeath", this, SendMessageOptions.DontRequireReceiver);

		if (deathSound)
        {
			AudioSource.PlayClipAtPoint(deathSound, transform.position);
		}

        if (deathResidue)
        {
            Vector3 deathResiduePosition = new Vector3(transform.position.x + deathResidueOffset.x, transform.position.y + deathResidueOffset.y, transform.position.z);
            Instantiate(deathResidue, deathResiduePosition, Quaternion.identity);
        }

		if (destroyOnDeath)
        {
			Destroy(gameObject);
		}
	}
}
