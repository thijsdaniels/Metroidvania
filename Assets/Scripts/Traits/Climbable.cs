using UnityEngine;
using System.Collections;

public class Climbable : MonoBehaviour {

	// void OnCharacterControllerEnter2D(CharacterController2D controller) {
	// 	controller.AddClimbable(this);
	// }

	// void OnCharacterControllerExit2D(CharacterController2D controller) {
	// 	controller.RemoveClimbable();
	// }

	void OnTriggerEnter2D(Collider2D collider) {

		var controller = collider.GetComponent<CharacterController2D>();

		if (controller) {
			controller.AddClimbable(this);
		}

	}

	void OnTriggerExit2D(Collider2D collider) {
		
		var controller = collider.GetComponent<CharacterController2D>();
		
		if (controller) {
			controller.RemoveClimbable();
		}
		
	}

}
