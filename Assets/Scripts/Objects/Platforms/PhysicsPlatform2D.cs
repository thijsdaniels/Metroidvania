using UnityEngine;
using System.Collections;

public class PhysicsPlatform2D : MonoBehaviour {
	
	public CharacterControllerParameters2D parameters;
	
	public void OnCharacterControllerEnter2D(CharacterController2D controller) {
		controller.OverrideParameters(parameters);
	}
	
	public void OnCharacterControllerExit2D(CharacterController2D controller) {
		controller.ResetParameters();
	}
	
}
