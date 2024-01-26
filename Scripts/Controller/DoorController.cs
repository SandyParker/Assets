using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator animator;

    public void Open()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        animator.SetTrigger("Open");
    }
}
