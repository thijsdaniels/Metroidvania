using System;
using UnityEngine;
using System.Collections;

public class Dropper : MonoBehaviour {

	[Serializable]
	public struct drop {
		public Collectable loot;
		[Range(0f, 1f)]
		public float
			dropChance;
	}

	public drop[] drops;

	private float offsetFactor = 0.5f;

	public void OnDeath() {

		for (var i = 0; i < Mathf.Min(drops.Length); i++) {

			var drop = drops[i];
			var roll = UnityEngine.Random.value;

			if (roll < drop.dropChance && drop.loot != null) {

				var randomOffset = new Vector2(
					transform.position.x + offsetFactor * (-1 + UnityEngine.Random.value * 2),
					transform.position.y + offsetFactor * (-1 + UnityEngine.Random.value * 2)
				);

				Instantiate(drop.loot, randomOffset, Quaternion.identity);

			}

		}

	}

}
