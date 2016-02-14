using UnityEngine;

public class Lootable : MonoBehaviour {

	private Animator animator;
	private bool looted = false;

	public Collectable loot;
	public bool destroyOnLoot = false;

	void Start() {

		animator = GetComponent<Animator>();

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

		looted = true;

		if (destroyOnLoot) {
			Destroy(gameObject);
		}
	}
}