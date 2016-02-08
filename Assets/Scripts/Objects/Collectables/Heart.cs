using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour {
	
	public void OnCollect(Collector collector) {
		var damagable = collector.GetComponent<Damagable>();
		if (damagable) {
			damagable.Heal(1);
		}
	}

}
