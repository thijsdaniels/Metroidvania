using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public AudioClip sound;

    public void Start()
    {
        if (sound)
        {
            AudioSource.PlayClipAtPoint(sound, transform.position);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        other.SendMessage("OnExplosionEnter", this, SendMessageOptions.DontRequireReceiver);
    }
}
