using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	public int value;

	public void OnCollect(Collector collector) {
		collector.coins += value;
	}

}
