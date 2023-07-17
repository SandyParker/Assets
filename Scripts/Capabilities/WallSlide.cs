using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlide : MonoBehaviour
{
    RaycastHit2D istouching;
    public float offset;
    private Rigidbody2D body;
    public Vector3 direction;
    public bool wallright;
    public bool wallleft;
    private Vector3 startOffset;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (body.velocity.x > 0)
        {
            direction = transform.right;
            startOffset = transform.position + new Vector3(GetComponent<Collider2D>().bounds.extents.x + offset, 0f);
            istouching = Physics2D.Raycast(startOffset, direction, 0.05f);
            if (istouching)
                wallright = true;
            else
                wallright = false;
        }
        else if (body.velocity.x < 0)
        {
            direction = -transform.right;
            startOffset = transform.position - new Vector3(GetComponent<Collider2D>().bounds.extents.x + offset, 0f);
            istouching = Physics2D.Raycast(startOffset, direction, 0.05f);
            if (istouching)
                wallleft = true;
            else
                wallleft = false;
        }
        if (direction.x>0)
        {
            wallleft = false;
        }

        if (direction.x<0)
        {
            wallright = false;
        }
        
        Debug.DrawLine(transform.position, (Vector3)transform.position+direction, Color.red);
    }

    public bool shouldWallrideright()
    {
        return wallright;
    }

    public bool shouldWallrideleft()
    {
        return wallleft;
    }
}
