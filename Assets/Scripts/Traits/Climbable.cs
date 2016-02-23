using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Interactable))]

public class Climbable : MonoBehaviour
{
    public float offset;

    private Interactable interactable;

    public void Start()
    {
        interactable = GetComponent<Interactable>();
    }

	public void OnCharacterControllerEnter2D(CharacterController2D controller)
    {
		controller.AddClimbable(this);

        if (controller.State.IsClimbing())
        {
            interactable.action = "Drop";
        }
        else
        {
            interactable.action = "Climb";
        }
	}

    public void OnCharacterControllerExit2D(CharacterController2D controller)
    {
		controller.RemoveClimbable();
	}

    public void OnInteraction(Player player)
    {
        CharacterController2D controller = player.GetComponent<CharacterController2D>();

        if (controller)
        {
            if (!controller.State.IsClimbing() && controller.CanClimb())
            {
                controller.StartClimbing();
                interactable.action = "Drop";
            }
            else if (controller.State.IsClimbing())
            {
                controller.StopClimbing();
                interactable.action = "Climb";
            }
        }
    }
}
