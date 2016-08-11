using System;
using UnityEngine;
using System.Collections.Generic;
using Objects.Collectables;

[RequireComponent(typeof(Player))]

public class Collector : MonoBehaviour
{
    public struct Ammo
    {
        public bool enabled { get; private set; }
        public int current { get; private set; }
        public int maximum { get; private set; }

        public Ammo(int current, int maximum)
        {
            this.enabled = true;
            this.current = current;
            this.maximum = maximum;
        }

        public Ammo(int current, int maximum, bool enabled)
        {
            this.enabled = enabled;
            this.current = current;
            this.maximum = maximum;
        }

        public override string ToString()
        {
            return current.ToString();
        }

        public void Enable()
        {
            enabled = true;
        }

        public bool Available()
        {
            return Available(1);
        }

        public bool Available(int value)
        {
            return current >= value;
        }

        public float Ratio()
        {
            return (float) current / maximum;
        }

        public void Consume()
        {
            Consume(1);
        }

        public void Consume(int value)
        {
            current = Mathf.Max(current - value, 0);
        }

        public void Add(int value) {
            current = Mathf.Min(current + value, maximum);
        }

        public void Restore()
        {
            current = maximum;
        }

        public void Upgrade(int value)
        {
            maximum += value;
        }
    }

    public struct AmmoCollection
    {
        public Ammo arrows;
        public Ammo bombs;

        public AmmoCollection(Ammo arrows, Ammo bombs)
        {
            this.arrows = arrows;
            this.bombs = bombs;
        }
    }

    public int coins;
	public int keys;

	public List<Item> items;

    public AmmoCollection ammo = new AmmoCollection();

    public Ammo mana = new Ammo(30, 30);

	public bool hasItem(Item item)
	{
		return items.Contains(item);
	}

    public void Collect(Collectable collectable)
    {
        collectable.SendMessage("OnCollect", this, SendMessageOptions.RequireReceiver);
    }
}
