using UnityEngine;
using System.Collections;

public class EvilBush : MonoBehaviour {

	private Animator animator;

	public Transform target;
	public float attackDelay;
	public Projectile projectile;
	public float projectileSpeed;
	public AudioClip shootSound;
	
	void Start() {

		// get a reference to the animator
		animator = GetComponent<Animator>();

		// start the attack co-routine
		if (attackDelay > 0) {
			StartCoroutine(OnAttack());
		}

	}
	
	void Update() {
		
	}

	IEnumerator OnAttack() {

		// wait out the attack interval
		yield return new WaitForSeconds(attackDelay);

		// shoot a projectile
		Attack();

		// restart the attack co-routine
		StartCoroutine(OnAttack());

	}

	void Attack() {
		if (target) {
			animator.SetTrigger("Attack");
		}
	}

	void OnShoot() {

		if (target && projectile) {

			// create a projectile
			Projectile projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity) as Projectile;

			// determine the direction to the target
			var direction = target.position.x > transform.position.x ? 1 : -1;

			// flip the projectile in the target's direction
			projectileInstance.transform.localScale = new Vector3(
				projectileInstance.transform.localScale.x * direction,
				projectileInstance.transform.localScale.y,
				projectileInstance.transform.localScale.z
			);

			// set the projectile's speed
			projectileInstance.speed = projectileSpeed;

			// play the shoot sound
			if (shootSound) {
				AudioSource.PlayClipAtPoint(shootSound, transform.position);
			}

		}
	}

}
