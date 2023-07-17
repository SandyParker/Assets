using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public float visiondistace;
    public Rigidbody2D body;
    public Rigidbody2D Player;
    private Vector2 direction;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (body.velocity.x > 0)
        {
            direction = transform.right;
        }
        else if (body.velocity.x < 0)
        {
            direction = -transform.right;
        }
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, direction, visiondistace);
        if (hitinfo.collider !=null)
        {
            Debug.DrawLine(transform.position, hitinfo.point, Color.red);
        }
        else
        {
            Debug.DrawLine(transform.position, (Vector2)transform.position + (Vector2) (direction * visiondistace), Color.red);
        }
    }
}
