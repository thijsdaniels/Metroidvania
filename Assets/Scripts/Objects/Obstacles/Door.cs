using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	private Animator animator;
	private Collider2D myCollider;

	public enum AccessMode {
		proximity,
		remote,
		locked
	}
	public AccessMode accessMode = AccessMode.proximity;
	private bool locked;

	public AudioClip openSound;
	public AudioClip closeSound;

	void Start() {
		animator = GetComponent<Animator>();
		myCollider = GetComponent<Collider2D>();
		if (accessMode == AccessMode.locked) {
			locked = true;
		}
	}

	public void Open() {
		animator.SetBool("Open", true);
	}

	public void Close() {
		animator.SetBool("Open", false);
	}

	void OnOpenStart() {
		if (openSound) {
			AudioSource.PlayClipAtPoint(openSound, transform.position);
		}
	}

	void OnOpenEnd() {
		myCollider.enabled = false;
	}

	void OnCloseStart() {
		if (closeSound) {
			AudioSource.PlayClipAtPoint(closeSound, transform.position);
		}
	}

	void OnCloseEnd() {
		myCollider.enabled = true;
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (accessMode == AccessMode.locked && locked) {

			var collector = other.gameObject.GetComponent<Collector>();
			if (collector && collector.keys > 0) {
				collector.keys--;
				locked = false;
				Open();
			}

		} else if (accessMode == AccessMode.proximity && other.tag == "Player") {
			Open();
		}

	}

	void OnTriggerExit2D(Collider2D other) {

		if (accessMode == AccessMode.proximity && other.tag == "Player") {
			Close();
		}

	}

	public void OnSwitchPressed(Switch other) {

		if (accessMode == AccessMode.remote) {
			Open();
		}

	}

	public void OnSwitchDepressed(Switch other) {
		
		if (accessMode == AccessMode.remote) {
			Close();
		}
		
	}

}
