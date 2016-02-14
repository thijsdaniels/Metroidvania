using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ParticleSystem))]

public class Checkpoint : MonoBehaviour {

	private Animator animator;
    private ParticleSystem emitter;
	public bool active { get; private set; }

    public Vector2 spawnOffset;

	public void Awake()
    {
		animator = GetComponent<Animator>();
        emitter = GetComponent<ParticleSystem>();

        Deactivate();
	}

	public void OnTriggerEnter2D(Collider2D other)
    {
        if (active)
        {
            return;
        }

		var player = other.GetComponent<Player>();

		if (player) {
			Activate(player);
		}
	}

	public void Activate(Player player)
    {
		if (player.checkpoint)
        {
			player.checkpoint.Deactivate();
		}

		player.checkpoint = this;

		active = true;
		animator.SetBool("Active", true);

        if (!emitter.isPlaying)
        {
            emitter.Play();
        }
	}

	public void Deactivate()
    {
		active = false;
		animator.SetBool("Active", false);

        if (emitter.isPlaying)
        {
            emitter.Stop();
        }
	}

	public void Respawn(Player player)
    {
		var respawnPosition = transform.position + new Vector3(
			spawnOffset.x,
			spawnOffset.y,
			0
		);
		
		player.transform.position = respawnPosition;

		player.controller.SetVelocity(Vector2.zero);
	}
}
