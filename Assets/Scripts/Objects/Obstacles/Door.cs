using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	private Animator animator;
	private Collider2D myCollider;

	public enum AccessMode {
		Proximity,
		Remote,
		Locked
	}

	public AccessMode accessMode = AccessMode.Proximity;

	public AudioClip openSound;
	public AudioClip closeSound;

    void Awake()
    {
		animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();

        if (accessMode != AccessMode.Locked)
        {
            animator.SetTrigger("Unlock");
        }

    }

	public void Open()
    {
		animator.SetBool("Open", true);
	}

	public void Close()
    {
		animator.SetBool("Open", false);
	}

	void OnOpenStart()
    {
		if (openSound)
        {
			AudioSource.PlayClipAtPoint(openSound, transform.position);
		}
	}

	void OnOpenEnd()
    {
        myCollider.enabled = false;
	}

	void OnCloseStart()
    {
		if (closeSound)
        {
			AudioSource.PlayClipAtPoint(closeSound, transform.position);
		}

        myCollider.enabled = true;
    }

	void OnCloseEnd()
    {
        
	}

	void OnTriggerEnter2D(Collider2D other)
    {
		if (accessMode == AccessMode.Locked)
        {
			var collector = other.gameObject.GetComponent<Collector>();

			if (collector && collector.keys > 0)
            {
                collector.keys--;
                Unlock();
			}
		}
        else if (accessMode == AccessMode.Proximity && other.tag == "Player")
        {
			Open();
		}
	}

    protected void Unlock()
    {
        animator.SetTrigger("Unlock");
        Open();
        accessMode = AccessMode.Proximity;
    }

	void OnTriggerExit2D(Collider2D other)
    {
		if (accessMode == AccessMode.Proximity && other.tag == "Player") {
			Close();
		}
	}

	public void OnSwitchPressed(Switch other)
    {
		if (accessMode == AccessMode.Remote) {
			Open();
		}
	}

	public void OnSwitchDepressed(Switch other)
    {
		if (accessMode == AccessMode.Remote) {
			Close();
		}
	}

}
