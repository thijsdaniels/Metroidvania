using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	public AudioClip collectSound;

	void OnTriggerEnter2D(Collider2D target)
    {
		if (target.gameObject.tag == "Player") {

			var collector = target.gameObject.GetComponent<Collector>();
			if (collector) {
                collector.Collect(this);
			}

		}
	}

	public virtual void OnCollect(Collector collector)
    {
		if (collectSound) {
			AudioSource.PlayClipAtPoint(collectSound, collector.transform.position);
		}
	}

}
