using UnityEngine;
using System.Collections;

public class Flame : MonoBehaviour
{
    private ParticleSystem[] emitters;
    public bool lit = true;

    public void Awake()
    {
        this.emitters = gameObject.GetComponentsInChildren<ParticleSystem>();

        if (this.lit)
        {
            this.Light();
        }
        else
        {
            this.Extinguish();
        }
    }

    /**
     *
     */
    public void Light()
    {
        foreach (ParticleSystem emitter in emitters)
        {
            emitter.Play();
        }
    }

    /**
     *
     */
    public void Extinguish()
    {
        foreach (ParticleSystem emitter in emitters)
        {
            emitter.Stop();
        }
    }

    /**
     *
     */
    public bool IsLit()
    {
        return this.lit;
    }

    /**
     *
     */
    public void OnTriggerEnter2D(Collider2D other)
    {
        Flame otherFlame = other.GetComponent<Flame>();

        if (otherFlame != null)
        {
            if (this.IsLit())
            {
                Debug.Log("Lighting otherFlame.");
                otherFlame.Light();
            }
        }
    }
}
