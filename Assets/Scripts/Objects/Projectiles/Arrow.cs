using UnityEngine;
using System.Collections;

/**
 * 
 */
public class Arrow : MonoBehaviour {

	private Vector3 lastPosition;

	public float despawnDelay = 0.25f;
    public float timeoutDelay = 30f;

	/**
	 * 
	 */
	void Awake()
	{
        StartCoroutine(Timeout());

        lastPosition = transform.position;
	}

	/**
	 * 
	 */
	void Update()
	{
		var moveDirection = gameObject.transform.position - lastPosition;

		if (moveDirection != Vector3.zero) {
			float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}

		lastPosition = transform.position;
	}

	/**
	 * 
	 */
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.isTrigger || other.gameObject.tag == "Player") {
			return;
		}

		OnHit();
	}

	/**
	 * 
	 */
	void OnHit()
	{
		var body = GetComponent<Rigidbody2D>();
		body.isKinematic = true;

		Destroy(gameObject.GetComponent<Untouchable>());

		StartCoroutine(Despawn());
	}

	/**
	 * 
	 */
	IEnumerator Despawn()
	{
		yield return new WaitForSeconds(despawnDelay);

		Destroy(gameObject);
	}

    /**
	 * 
	 */
    IEnumerator Timeout()
    {
        yield return new WaitForSeconds(timeoutDelay);

        Destroy(gameObject);
    }
}
