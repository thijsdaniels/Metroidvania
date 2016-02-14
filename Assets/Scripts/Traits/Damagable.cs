﻿using UnityEngine;
using System.Collections;

public class Damagable : MonoBehaviour {

	public int initialHealth;
	public int currentHealth { get; protected set; }
	public int maximumHealth;

	public bool destroyOnDeath = true;

	public AudioClip hitSound;
	public AudioClip deathSound;

    public GameObject deathResidue;

	public void Start() {
		currentHealth = initialHealth;
	}

	public void TakeDamage(GameObject cause, int damage) {

        if (Immune(cause)) {
            return;
        }
		
		currentHealth = Mathf.Max(0, currentHealth - damage);

		if (currentHealth > 0) {
			Hit();
		} else {
			Die();
		}
		
	}

    protected virtual bool Immune(GameObject cause) {
        return false;
    }

	public void Kill() {
		currentHealth = 0;
		Die();
	}

	public void Heal(int amount) {
		currentHealth = Mathf.Min(maximumHealth, currentHealth + amount);
	}

	public void Restore() {
		currentHealth = maximumHealth;
	}

	public void Revive() {
		currentHealth = initialHealth;
	}

	protected void Hit() {

		SendMessage("OnHit", this, SendMessageOptions.DontRequireReceiver);

		if (hitSound) {
			AudioSource.PlayClipAtPoint(hitSound, transform.position);
		}

	}

	protected void Die() {

		SendMessage("OnDeath", this, SendMessageOptions.DontRequireReceiver);

		if (deathSound) {
			AudioSource.PlayClipAtPoint(deathSound, transform.position);
		}

        if (deathResidue)
        {
            Instantiate(deathResidue, this.transform.position, Quaternion.identity);
        }

		if (destroyOnDeath) {
			Destroy(gameObject);
		}

	}

}
