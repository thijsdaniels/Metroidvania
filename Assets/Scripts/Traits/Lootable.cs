using UnityEngine;

[RequireComponent(typeof(Interactable))]

public class Lootable : MonoBehaviour {

	private Animator animator;
    private Interactable interactable;
	private bool looted = false;

	public Collectable loot;
	public bool destroyOnLoot = false;

	void Start() {

		animator = GetComponent<Animator>();
        interactable = GetComponent<Interactable>();

		if (animator && looted) {
			animator.SetTrigger("Looted");
		}

	}

	public void OnInteraction(Player player)
	{
		var collector = player.gameObject.GetComponent<Collector>();

		if (!looted && collector) {
			Loot(collector);
		}
	}

	private void Loot(Collector collector)
	{
		if (animator) {
			animator.SetTrigger("Looted");
		}

		if (loot) {
			collector.Collect(Instantiate(loot));
			loot = null;
		}

        interactable.action = null;

		looted = true;

		if (destroyOnLoot) {
			Destroy(gameObject);
		}
	}
}