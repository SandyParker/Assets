using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool OnGround;
    private float friction;

    [SerializeField] private LayerMask platformLayertask;
    private void OnTriggerStay2D(Collider2D collider)
    {

        OnGround = collider != null && (((1 << collider.gameObject.layer) & platformLayertask) != 0);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnGround = false;
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluvateCollion(collision);
        RetriveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluvateCollion(collision);
        RetriveFriction(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //OnGround = false;
        friction = 0;
    }
    private void EvaluvateCollion(Collision2D collision)
    {
        for(int i=0;i<collision.contactCount;i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            OnGround |= normal.y >= 0.9f;
        }
    }

    private void RetriveFriction(Collision2D collision)
    {
        PhysicsMaterial2D material = collision.rigidbody.sharedMaterial;
        friction = 0;
        if (material !=null)
        {
            friction = material.friction;
        }
    }

    public bool GetOnGround()
    {
        return OnGround;
    }

    public float GetFriction()
    {
        return friction;
    }
}
