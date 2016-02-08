using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	private Rigidbody2D body;

	public float speed;
	public float fuseDuration;

	void Start() {

		body = GetComponent<Rigidbody2D>();

		if (fuseDuration > 0) {
			StartCoroutine(OnFuseLit());
		}

	}

	void Update() {

		var velocity = new Vector3(
			speed * transform.localScale.x,
			body.velocity.y
		);

		body.velocity = velocity;

	}

	void OnTriggerEnter2D(Collider2D other) {
		OnHit();
	}

	IEnumerator OnFuseLit() {

		yield return new WaitForSeconds(fuseDuration);

		OnHit();

	}

	void OnHit() {
		Destroy(gameObject);
	}
}
