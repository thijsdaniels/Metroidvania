using UnityEngine;
using System.Collections.Generic;
using Objects.Collectables;

[RequireComponent(typeof(Player))]

public class Collector : MonoBehaviour {

	public int coins;
	public int keys;

	public List<Item> items;

    public int arrows = 0;
    public int bombs = 0;

    public int currentMana = 30;
    public int maximumMana = 30;

	public bool hasItem(Item item)
	{
		return items.Contains(item);
	}

    public void Collect(Collectable collectable)
    {
        collectable.SendMessage("OnCollect", this, SendMessageOptions.RequireReceiver);
    }

}
