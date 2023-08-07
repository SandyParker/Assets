using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float deactivateDistance = 3f;
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

    private Path path;
    private int currentWaypoint = 0;
    public float MaxSpeedChange;
    Seeker seeker;
    Rigidbody2D rb;

    [SerializeField, Range(0, 100f)] private float MaxSpeed = 4f;
    [SerializeField, Range(0, 100f)] private float MaxAcc = 35f;
    [SerializeField, Range(0, 100f)] private float MaxAirAcc = 20f;
    public Vector2 velocity;
    private Vector2 desiredVelocity;
    private float acceration;

    [SerializeField, Range(0, 100f)] private float JumpHeight = 3f;
    [SerializeField, Range(0, 10f)] private float DownwardMovementMultiplier = 3f;
    [SerializeField, Range(0, 10f)] private float UpwardMovementMultiplier = 1.7f;

    public ParticleSystem dust;
    private Ground ground;
    private bool isGrounded;
    public EnemyAnimations anim;

    void Start()
    {
        ground = GetComponent<Ground>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void Update()
    {
        //Debug.Log(Vector2.Distance(transform.position, target.transform.position));
        isGrounded = ground.GetOnGround();
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(MaxSpeed - ground.GetFriction(), 0f);
        if (TargetInDistance() && followEnabled && TargetCloseDistance())
        {
            PathFollow();
        }

        if (!TargetCloseDistance())
        {
            velocity.x = 0;
            velocity.y = 0;
            rb.velocity = velocity;
            anim.anim.SetBool("IsAttack", true);
        }

    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && TargetCloseDistance())
        {
            
            seeker.StartPath(rb.position, target.position, OnPathComplete);
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

        // Direction Calculation
        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        // Jump
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

            // Movement
            //rb.AddForce(force);
            velocity = rb.velocity;

        acceration = isGrounded ? acceration : MaxAirAcc;
        MaxSpeedChange = acceration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, MaxSpeedChange);
        rb.velocity = velocity;

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

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private bool TargetCloseDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) > deactivateDistance;
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
