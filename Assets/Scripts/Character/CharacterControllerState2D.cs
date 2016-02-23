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

    public bool climbing;

	public bool IsGrounded()
    {
        return collisionBelow || climbing;
	}

    public bool IsClimbing()
    {
        return climbing;
    }

	public bool IsColliding()
    {
		return collisionRight || collisionLeft || collisionAbove || collisionBelow;
	}

	public bool IsOnSlope()
    {
		return slopeUp || slopeDown;
	}

	public void Reset()
    {
		collisionRight = collisionLeft = collisionAbove = collisionBelow = slopeUp = slopeDown = false;
        slopeAngle = 0;
	}

}
