using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	public AudioClip collectSound;

	void OnTriggerEnter2D(Collider2D target) {

		if (target.gameObject.tag == "Player") {

			var collector = target.gameObject.GetComponent<Collector>();
			if (collector) {
				Collect(collector);
				Destroy(gameObject);
			}

		}

	}

	public void Collect(Collector collector) {

		if (collectSound) {
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
		}

		this.SendMessage("OnCollect", collector, SendMessageOptions.RequireReceiver);
	}

}
