using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]

public class Interactable : MonoBehaviour {

    public string action;

	void OnTriggerEnter2D(Collider2D other)
    {
		var player = other.GetComponent<Player>();

		if (player)
        {
			player.interactable = this;
		}
	}

	void OnTriggerExit2D(Collider2D other)
    {
		var player = other.GetComponent<Player>();
		
		if (player)
        {
			if (player.interactable == this)
            {
				player.interactable = null;
			}
		}
	}

}
