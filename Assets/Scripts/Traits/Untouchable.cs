using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]

public class Untouchable : MonoBehaviour
{
    public enum TargetTag
    {
        Player,
        Enemy,
        Untagged,
    };

    public TargetTag targetTag;

	public int damage;

    public enum DamageEvent
    {
        OnEnter,
        OnStay,
    }

    public DamageEvent damageEvent;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!damageEvent.Equals(DamageEvent.OnEnter))
        {
            return;
        }

        if (targetTag.ToString() == other.gameObject.tag)
        {
			InflictDamage(other.gameObject);
		}
	}

    void OnCollisionStay2D(Collision2D other)
    {
        if (!damageEvent.Equals(DamageEvent.OnStay))
        {
            return;
        }

        if (targetTag.ToString() == other.gameObject.tag)
        {
            InflictDamage(other.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!damageEvent.Equals(DamageEvent.OnEnter))
        {
            return;
        }

        if (targetTag.ToString() == other.gameObject.tag)
        {
            InflictDamage(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!damageEvent.Equals(DamageEvent.OnStay))
        {
            return;
        }

        if (targetTag.ToString() == other.gameObject.tag)
        {
            InflictDamage(other.gameObject);
        }
    }

	void InflictDamage(GameObject target)
    {
        var damagable = target.GetComponent<Damagable>();

		if (damagable)
        {
			damagable.TakeDamage(gameObject, damage);
		}
	}

}
