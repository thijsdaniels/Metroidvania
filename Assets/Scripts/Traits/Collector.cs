using UnityEngine;
using System.Collections.Generic;
using Objects.Collectables;

[RequireComponent(typeof(Player))]

public class Collector : MonoBehaviour {

	public int coins;
	public int keys;

	public List<Item> items;

    public int arrows;
    public int bombs;

	public bool hasItem(Item item)
	{
		return items.Contains(item);
	}

    public void Collect(Collectable collectable)
    {
        collectable.SendMessage("OnCollect", this, SendMessageOptions.RequireReceiver);
    }

}
