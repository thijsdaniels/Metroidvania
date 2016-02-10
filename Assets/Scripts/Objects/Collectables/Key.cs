using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collectable))]

public class Key : MonoBehaviour {

	public void OnCollect(Collector collector)
    {
		collector.keys++;
	}

}
