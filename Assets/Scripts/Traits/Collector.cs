﻿using UnityEngine;
using System.Collections.Generic;

public class Collector : MonoBehaviour {

	public int coins;
	public int keys;
	public List<Item> items;

	public bool hasItem(Item item)
	{
		return items.Contains(item);
	}

    public void Collect(Collectable collectable)
    {
        collectable.SendMessage("OnCollect", this, SendMessageOptions.RequireReceiver);
    }

}
