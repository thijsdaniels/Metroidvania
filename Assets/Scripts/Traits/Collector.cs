using UnityEngine;
using System.Collections.Generic;

public class Collector : MonoBehaviour {

	public int coins;
	public int keys;

	public List<Weapon> weapons;

	public bool hasWeapon(Weapon weapon)
	{
		return weapons.Contains(weapon);
	}

}
