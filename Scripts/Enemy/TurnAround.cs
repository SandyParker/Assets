using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAround : MonoBehaviour
{
    private Rigidbody2D body;
    private bool facingright = false;
    private Vector3 localscale;
    public float Vel;

    [SerializeField, Range(0, 10f)] private float DownwardMovementMultiplier = 3f;
    [SerializeField, Range(0, 10f)] private float UpwardMovementMultiplier = 1.7f;
    private float defaultGravityScale;
    public ParticleSystem dust;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        localscale = transform.localScale;
        defaultGravityScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (body.velocity.y > 0f)
        {
            body.gravityScale = UpwardMovementMultiplier;
        }
        else if (body.velocity.y < 0f)
        {
            body.gravityScale = DownwardMovementMultiplier;
        }
        else if (body.velocity.y == 0f)
        {
            body.gravityScale = defaultGravityScale;
        }
    }

    private void LateUpdate()
    {
        if (body.velocity.x > 0)
        {
            facingright = true;
        }
        else if (body.velocity.x < 0)
        {
            facingright = false;
        }

        Vel = body.velocity.x;

        if ((facingright && (localscale.x < 0)) || (!facingright && (localscale.x > 0)))
        {
            dust.Play();
            localscale.x *= -1;
        }
        transform.localScale = localscale;
    }
}
