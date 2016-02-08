using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	private Animator animator;
	public bool active { get; private set; }

	public Vector2 spawnOffset;

	public void Start() {
		animator = GetComponent<Animator>();
	}

	public void OnTriggerEnter2D(Collider2D other) {

		var player = other.GetComponent<Player>();
		if (player) {
			Activate(player);
		}

	}

	public void Activate(Player player) {
		if (player.checkpoint) {
			player.checkpoint.Deactivate();
		}
		player.checkpoint = this;
		active = true;
		animator.SetBool("Active", true);
	}

	public void Deactivate() {
		active = false;
		animator.SetBool("Active", false);
	}

	public void Respawn(Player player) {

		var respawnPosition = transform.position + new Vector3(
			spawnOffset.x,
			spawnOffset.y,
			0
		);
		
		player.transform.position = respawnPosition;

		player.controller.SetVelocity(Vector2.zero);

	}
}
