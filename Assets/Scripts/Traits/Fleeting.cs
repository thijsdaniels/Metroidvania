using UnityEngine;
using System.Collections;

public class Fleeting : MonoBehaviour
{
    public enum Mode
    {
        Animator,
        ParticleSystem,
        Collision
    }

    public Mode mode = Mode.Animator;
    public float delay = 0f;

    new private ParticleSystem particleSystem;
    public LayerMask collisionLayerMask;
    public bool active = true;

    /**
     * 
     */
    public void Start()
    {
        switch(mode)
        {

            case Mode.ParticleSystem:

                particleSystem = GetComponent<ParticleSystem>();

                if (!particleSystem)
                {
                    Debug.LogWarning("Fleeting mode 'ParticleSystem' requires a ParticleSystem component.");
                }

                break;

            case Mode.Animator:

                if (!GetComponent<Animator>())
                {
                    Debug.LogWarning("Fleeting mode 'Animator' requires a Animator component.");
                }

                break;

            case Mode.Collision:

                if (!GetComponent<Collider2D>())
                {
                    Debug.LogWarning("Fleeting mode 'Collision' requires a Collider component.");
                }

                break;

        }

    }

    /**
     * 
     */
    public void Update()
    {
        if (!active)
        {
            return;
        }

        if (!mode.Equals(Mode.ParticleSystem) || !particleSystem)
        {
            return;
        }

        if (!particleSystem.IsAlive())
        {
            SendMessage("OnFleetingDelay", null, SendMessageOptions.RequireReceiver);
            particleSystem = null;
        }
    }

    public void Activate()
    {
        active = true;
    }

    /**
     * 
     */
    public void OnAnimationEnd()
	{
        if (!active)
        {
            return;
        }

        if (!mode.Equals(Mode.Animator))
        {
            return;
        }

        SendMessage("OnFleetingDelay", null, SendMessageOptions.RequireReceiver);
	}
    
    /**
     *
     */
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!active)
        {
            return;
        }

        if (!mode.Equals(Mode.Collision))
        {
            return;
        }

        if (((1 << collision.gameObject.layer) & collisionLayerMask) == 0)
        {
            return;
        }

        SendMessage("OnFleetingDelay", null, SendMessageOptions.RequireReceiver);
    }

    /**
     *
     */
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!active)
        {
            return;
        }

        if (!mode.Equals(Mode.Collision))
        {
            return;
        }

        if (((1 << other.gameObject.layer) & collisionLayerMask) == 0)
        {
            return;
        }

        SendMessage("OnFleetingDelay", null, SendMessageOptions.RequireReceiver);
    }

    /**
     *
     */
    protected IEnumerator OnFleetingDelay()
    {
        yield return new WaitForSeconds(delay);

        SendMessage("OnFleetingEnd", null, SendMessageOptions.RequireReceiver);
    }

    /**
     *
     */
    public void OnFleetingEnd()
    {
        Destroy(gameObject);
    }
}
