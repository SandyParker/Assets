using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    public float speed;
    public Transform target;
    public float minDistance;
    public Vector2 velocity;
    private Rigidbody2D body;
    private float MaxSpeedChange;
    private float acceration;
    private Vector2 desiredVelocity;
    private Ground ground;
    public bool OnGround;
    private Vector2 direction;

    [SerializeField, Range(0, 100f)] private float MaxSpeed = 4f;
    [SerializeField, Range(0, 100f)] private float MaxAcc = 35f;
    [SerializeField, Range(0, 100f)] private float MaxAirAcc = 20f;

    [SerializeField, Range(0, 100f)] private float JumpHeight = 3f;
    [SerializeField, Range(0, 10f)] private float DownwardMovementMultiplier = 3f;
    [SerializeField, Range(0, 10f)] private float UpwardMovementMultiplier = 1.7f;


    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
    }

    void Update()
    {
        OnGround = ground.GetOnGround();
        if (body.position.x > target.position.x)
            direction.x = -1;
        else if (body.position.x < target.position.x)
            direction.x = 1;
        else
            direction.x = 0;

        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(MaxSpeed - ground.GetFriction(), 0f);

    }

    private void FixedUpdate()
    {

        if (Vector2.Distance(transform.position, target.position) > minDistance)
        {
            velocity = body.velocity;

            acceration = OnGround ? acceration : MaxAirAcc;
            MaxSpeedChange = acceration * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, MaxSpeedChange);
            body.velocity = velocity;

            if(body.velocity.y > 0f)
            {
                body.gravityScale = UpwardMovementMultiplier;
            }

            else if (body.velocity.y < 0f)
            {
                body.gravityScale = DownwardMovementMultiplier;
            }

            if (OnGround && velocity.x < 1f && velocity.x > -1f)
            {
                JumpAction();
            }
        }
        body.velocity = velocity;
    }

    private void JumpAction()
    {
        if (OnGround)
        {
            float jumpspeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * JumpHeight);
            //if (velocity.y>0f)
            //{
            jumpspeed = Mathf.Max(jumpspeed - velocity.y, 0f);
            //}

            velocity.y += jumpspeed;
        }
    }

}
