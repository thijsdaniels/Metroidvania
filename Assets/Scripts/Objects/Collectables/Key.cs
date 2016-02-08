using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

	public void OnCollect(Collector collector) {
		collector.keys++;
	}

}
