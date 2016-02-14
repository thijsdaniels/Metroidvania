using UnityEngine;
using System.Collections;

public class Stalker : MonoBehaviour {

	public enum MoveMode {
		linear,
		interpolated
	}

	public Transform target;
	public Vector2 targetOffset;
	public bool startOnTarget;
    public bool faceTarget;

	public bool followXAxis;
	public bool followYAxis;
	public MoveMode moveMode = MoveMode.linear;

	public bool asleep;
	public float wakeDistance;
	private float wakeDistanceSquared;

	[Range(0, 30)]
	public float
		speed;

    /**
     *
     */
	void Awake()
    {
		wakeDistanceSquared = wakeDistance * wakeDistance;
		if (target && startOnTarget) {
			transform.position = GetTargetPosition();
		}
	}

    /**
     *
     */
    void Update()
    {
		if (!target) {
			return;
		}

		if (!asleep) {
			Move();
		} else {
			Sleep();
		}
	}

    /**
     *
     */
    void Sleep()
    {
		var distanceSquared = (transform.position - target.position).sqrMagnitude;

		if (distanceSquared < wakeDistanceSquared) {
            WakeUp();
		}
	}

    /**
     *
     */
    protected void WakeUp()
    {
        asleep = false;

        gameObject.SendMessage("OnWakeUp", null, SendMessageOptions.DontRequireReceiver);
    }

    /**
     *
     */
    protected void FallAsleep()
    {
        asleep = true;

        gameObject.SendMessage("OnFallAsleep", null, SendMessageOptions.DontRequireReceiver);
    }

    /**
     *
     */
    protected void Move()
    {
        if (this.faceTarget)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x) * GetTargetPosition().x > transform.position.x ? 1 : -1,
                transform.localScale.y,
                transform.localScale.z
            );
        }


        if (moveMode == MoveMode.linear)
        {
			transform.position = Vector3.MoveTowards(
				transform.position,
				GetTargetPosition(),
				speed * Time.deltaTime
			);
		}
        else if (moveMode == MoveMode.interpolated)
        {
			transform.position = Vector3.Lerp(
				transform.position,
				GetTargetPosition(),
				speed * Time.deltaTime
			);
		}
	}

    /**
     *
     */
    protected Vector3 GetTargetPosition()
    {
		return new Vector3(
			followXAxis ? target.position.x + targetOffset.x : transform.position.x,
			followYAxis ? target.position.y + targetOffset.y : transform.position.y,
			transform.position.z
		);
	}

    /**
     *
     */
    public void OnDrawGizmos()
    {
		if (asleep) {
			Gizmos.DrawWireSphere(transform.position, wakeDistance);
		}
	}

}
