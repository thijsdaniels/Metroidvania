using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {

	private Animator animator;
	private int collisionCount;
	private bool pressed;

	public GameObject target;
	public bool sticky;

	void Start() {
		animator = GetComponent<Animator>();
	}

	void OnTriggerEnter2D(Collider2D other) {

		//if (!other.isTrigger) {
			collisionCount++;
			if (!pressed) {
				OnPress();
			}
		//}

	}

	void OnTriggerExit2D(Collider2D other) {

		if (sticky) {
			return;
		}

		//if (!other.isTrigger) {
			collisionCount--;
			if (pressed && collisionCount <= 0) {
				OnDepress();
			}
		//}

	}

	public void OnPress() {
		pressed = true;
		animator.SetBool("Pressed", true);
		target.SendMessage("OnSwitchPressed", this, SendMessageOptions.RequireReceiver);
	}

	public void OnDepress() {
		pressed = false;
		animator.SetBool("Pressed", false);
		target.SendMessage("OnSwitchDepressed", this, SendMessageOptions.RequireReceiver);
	}

}
