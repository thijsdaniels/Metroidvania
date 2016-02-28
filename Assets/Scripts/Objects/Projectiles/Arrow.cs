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
	public void Awake()
	{
        StartCoroutine(Timeout());

        lastPosition = transform.position;
	}

	/**
	 * 
	 */
	public void Update()
	{
        PointInDirection();
	}

    /**
     *
     */
    protected void PointInDirection()
    {
        Vector3 direction = gameObject.transform.position - lastPosition;

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        lastPosition = transform.position;
    }

	/**
	 * 
	 */
	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.isTrigger || other.gameObject.tag == "Player") {
			return;
		}

		OnHit();
	}

	/**
	 * 
	 */
	protected void OnHit()
	{
		var body = GetComponent<Rigidbody2D>();
		body.isKinematic = true;

		Destroy(gameObject.GetComponent<Untouchable>());

		StartCoroutine(Despawn());
	}

	/**
	 * 
	 */
	protected IEnumerator Despawn()
	{
		yield return new WaitForSeconds(despawnDelay);

		Destroy(gameObject);
	}

    /**
	 * 
	 */
    protected IEnumerator Timeout()
    {
        yield return new WaitForSeconds(timeoutDelay);

        Destroy(gameObject);
    }
}
