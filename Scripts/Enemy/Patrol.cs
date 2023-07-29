using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Patrol : MonoBehaviour
{
    [Header("Pathfinding")]
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpCheckOffset = 0.1f;
    private Vector2 direction;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;
    Seeker seeker;
    Rigidbody2D rb;

    public Transform[] patrolpoints;
    public float waittime;
    int currPointIndex;
    public Vector2 velocity;
    private float MaxSpeedChange;
    private float acceration;
    private Vector2 desiredVelocity;
    private Ground ground;

    public bool isjump;

    private Path path;
    private int currentWaypoint = 0;
    private bool isGrounded;

    [SerializeField, Range(0, 100f)] private float MaxSpeed = 4f;
    [SerializeField, Range(0, 100f)] private float MaxAcc = 35f;
    [SerializeField, Range(0, 100f)] private float MaxAirAcc = 20f;

    [SerializeField, Range(0, 100f)] private float JumpHeight = 3f;
    [SerializeField, Range(0, 10f)] private float DownwardMovementMultiplier = 3f;
    [SerializeField, Range(0, 10f)] private float UpwardMovementMultiplier = 1.7f;

    public ParticleSystem dust;


    bool once;

    void Start()
    {
        currPointIndex = 0;
        once = false;
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    // Update is called once per frame

    private void UpdatePath()
    {
        if (followEnabled)
        {
            //Debug.Log(patrolpoints[currPointIndex].position);
            seeker.StartPath(rb.position, patrolpoints[currPointIndex].position, OnPathComplete);
        }
        
    }

    void Update()
    {
        isGrounded = ground.GetOnGround();
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(MaxSpeed - ground.GetFriction(), 0f);
        if (followEnabled)
        {
            PathFollow();
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }


        // See if colliding with anything
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                dust.Play();
                float jumpspeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * JumpHeight);
                //if (velocity.y>0f)
                //{
                jumpspeed = Mathf.Max(jumpspeed - velocity.y, 0f);
                //}

                velocity.y += jumpspeed;
                rb.velocity = velocity;
            }
        }

        

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x - patrolpoints[currPointIndex].position.x) > 0.1f)
        {
            velocity = rb.velocity;

            acceration = isGrounded ? acceration : MaxAirAcc;
            MaxSpeedChange = acceration * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, MaxSpeedChange);
            rb.velocity = velocity;
        }
        else
        {
            if (once == false)
            {
                once = true;
                velocity.x = 0;
                rb.velocity = velocity;
                StartCoroutine(Wait());
            }
        }

        if (rb.velocity.y > 0f)
        {
            rb.gravityScale = UpwardMovementMultiplier;
        }

        else if (rb.velocity.y < 0f)
        {
            rb.gravityScale = DownwardMovementMultiplier;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waittime);
        if (currPointIndex+1<patrolpoints.Length)
        {
            currPointIndex++;
        }
        else
        {
            currPointIndex = 0;
        }
        once = false;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
