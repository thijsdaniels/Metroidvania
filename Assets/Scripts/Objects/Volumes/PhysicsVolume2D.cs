using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]

public class PhysicsVolume2D : MonoBehaviour {

	public CharacterControllerParameters2D parameters;

	public void OnTriggerStay2D(Collider2D other)
    {	
		var controller = other.gameObject.GetComponent<CharacterController2D>();

		if (controller)
        {
			controller.OverrideParameters(parameters);
		}
	}
	
	public void OnTriggerExit2D(Collider2D other)
    {
		var controller = other.gameObject.GetComponent<CharacterController2D>();
		if (controller)
        {
			controller.ResetParameters();
		}
		
	}
}
