using System;
using UnityEngine;
using System.Collections;

public class Dropper : MonoBehaviour {

    [Serializable]
    public struct Drop {
		public Collectable loot;
		[Range(0f, 1f)]
        public float dropChance;
	}

	public Drop[] drops;

    public Vector2 offset = Vector2.zero;
	private Vector2 offsetFactor = Vector2.zero;

	public void OnDeath() {

		for (var i = 0; i < Mathf.Min(drops.Length); i++) {

			var drop = drops[i];
			var roll = UnityEngine.Random.value;

			if (roll < drop.dropChance && drop.loot != null) {

				var dropPosition = new Vector2(
					transform.position.x,
					transform.position.y
				) + offset + offsetFactor * (-1 + UnityEngine.Random.value * 2);

				Instantiate(drop.loot, dropPosition, Quaternion.identity);

			}

		}

	}

}
