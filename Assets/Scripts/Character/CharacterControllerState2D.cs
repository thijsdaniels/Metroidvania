using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class CharacterControllerState2D
{
	public bool collisionRight;
	public bool collisionLeft;
	public bool collisionAbove;
	public bool collisionBelow;

	public bool slopeUp;
	public bool slopeDown;
	public float slopeAngle;

    public bool rolling;
    public bool climbing;
    public bool swimming;
    public bool aiming;
    public bool attacking;

    /**
     *
     */
    public bool IsGrounded()
    {
        return collisionBelow || climbing;
	}

    /**
     *
     */
    public bool IsRolling()
    {
        return rolling;
    }

    /**
     *
     */
    public bool IsClimbing()
    {
        return climbing;
    }

    /**
     *
     */
    public bool IsSwimming()
    {
        return swimming;
    }

    /**
     *
     */
    public bool IsAiming()
    {
        return aiming;
    }

    /**
     *
     */
    public bool IsAttacking()
    {
        return attacking;
    }

    /**
     *
     */
    public bool IsColliding()
    {
		return collisionRight || collisionLeft || collisionAbove || collisionBelow;
	}

    /**
     *
     */
    public bool IsOnSlope()
    {
		return slopeUp || slopeDown;
	}

    /**
     *
     */
    public void Reset()
    {
		collisionRight = collisionLeft = collisionAbove = collisionBelow = slopeUp = slopeDown = false;
        slopeAngle = 0;
	}
}
