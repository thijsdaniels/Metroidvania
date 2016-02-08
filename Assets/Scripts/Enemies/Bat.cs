using UnityEngine;
using System.Collections;

public class Bat : MonoBehaviour
{
    private Animator animator;

    /**
     *
     */
    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    /**
     *
     */
    public void OnWakeUp()
    {
        animator.SetBool("Asleep", false);
    }

    /**
     *
     */
    public void OnFallAsleep()
    {
        animator.SetBool("Asleep", true);
    }
}
