using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public float visiondistance;
    private Rigidbody2D body;
    private Vector2 direction;
    private RaycastHit2D hitInfo;
    private Patrol patrol;
    private EnemyAI AI;
    public float chasetime;
    public float chasing;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        patrol = GetComponent<Patrol>();
        AI = GetComponent<EnemyAI>();
        chasing = chasetime;
    }

    // Update is called once per frame
    void Update()
    {

        if (body.velocity.x > 0)
        {
            direction=transform.right;
        }
        else if (body.velocity.x < 0)
        {
            direction = -transform.right;
        }
        Physics2D.queriesStartInColliders = false;
        hitInfo= Physics2D.Raycast(transform.position, direction, visiondistance);
        Physics2D.queriesStartInColliders = true;
        if (hitInfo.collider!=null)
        {
            if (hitInfo.collider.tag == "Player")
            {
                hit();
                patrol.enabled = false;
                AI.enabled = true;
                chasing = 0;
            }
            else
            {
                hitnoplayer();
            }
        }
        else
        {
            nothit();
        }
        if (chasing<chasetime)
        {
            chasing += Time.deltaTime;
        }
        else
        {
            patrol.enabled = true;
            AI.enabled = false;
        }
    }

    void nothit()
    {
        Debug.DrawLine(transform.position, (Vector2)transform.position + direction * visiondistance, Color.green);
    }
    void hit()
    {
        Debug.DrawLine(transform.position, hitInfo.point, Color.red);
    }

    void hitnoplayer()
    {
        Debug.DrawLine(transform.position, hitInfo.point, Color.red);
    }
}
