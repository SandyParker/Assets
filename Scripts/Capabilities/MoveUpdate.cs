using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;

public class MoveUpdate : MonoBehaviour
{
    public Animator anim;
    public static bool allowed;
    [Header("Contols")]
    private PlayerInput input;
    public static PlayerControls playercontrols;
    public bool IsGamepad;

    [Header("Jumps")]
    [SerializeField, Range(0, 100f)] private float JumpHeight = 3f;
    [SerializeField, Range(0, 5)] private float MaxAirJumps = 0;
    [SerializeField, Range(0, 10f)] private float DownwardMovementMultiplier = 3f;
    [SerializeField, Range(0, 10f)] private float UpwardMovementMultiplier = 1.7f;
    [SerializeField, Range(0, 10f)] private float fallpen = 1f;

    [Header("Movement")]
    public Vector2 velocity;
    private Rigidbody2D body;
    private Ground ground;
    public float maxfallspeed;
    private PlayerAttackandTakedown take;

    public int JumpPhase;
    public float jumperem;
    public float jumpremtime;
    private float defaultGravityScale;

    
    public float horizontal;
    public float vertical;
    public Vector2 dir;
    public float fallremember;
    public float fallrem;
    public float jumpmod;
    public bool shortJump;
    public bool OnGround;
    public ParticleSystem dust;
    [SerializeField, Range(0, 100f)] private float MaxSpeed = 4f;
    [SerializeField, Range(0, 100f)] private float MaxAcc = 35f;
    [SerializeField, Range(0, 100f)] private float MaxAirAcc = 20f;
    [SerializeField, Range(0, 1f)] private float directionfactor;

    public Vector2 direction;
    private Vector2 desiredVelocity;

    private float MaxSpeedChange;
    private float acceration;
    private bool dashallow;

    [Header("Wall Slide")]
    public Wall wall;
    [SerializeField, Range(0, 10f)] private float wallslidespeed;
    [SerializeField, Range(0, 10f)] private float wallrunspeed;
    [SerializeField, Range(0, 50f)] private float maxwallslidespeed;
    public bool isWallSliding;
    public bool isWallJumping;
    public float walljumpdirection;
    public float walljumptime;
    private float walljumpcounter;
    public float walljumpduration;
    public Vector2 WallJumpPower;
    public Vector2 smallwalljump;
    private Camera maincam;
    public Vector2 aim;

    [Header("Ledge Climb")]
    public bool OnEdge;
    private EdgeClimb edge;
    public bool readytomove;
    public Vector3 moveOffset;

    [Header("Attack")]
    public float attackspeedreduser;

    [Header("Dash")]
    public float dashspeed;
    public float dashtime;
    public float dashtimer;

    [Header("Ult")]
    private PlayerUlt ult;

    void Awake()
    {
        allowed = true;
        playercontrols = new PlayerControls();
        input = GetComponent<PlayerInput>();
        shortJump = false;
        defaultGravityScale = 1f;
        dashtimer = dashtime;

        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        take = GetComponent<PlayerAttackandTakedown>();
        ult = GetComponent<PlayerUlt>();
        edge = GetComponent<EdgeClimb>();

        playercontrols.Controls.Jump.performed += _ => Jump();
        playercontrols.Controls.Jump.canceled += _ => JumpOver();
        playercontrols.Controls.Attack.performed += _ => take.Attack();
        playercontrols.Controls.Dash.performed += _ => Dash();
        playercontrols.Controls.Ult.performed += _ => ult.Ult();

    }

    public void OnControlsChanged(PlayerInput pi)
    {
        IsGamepad = pi.currentControlScheme.Equals("Controller") ? true : false;
    }

    public void Dash()
    {
        if (dashallow)
        {
            dashtimer = 0;
            dashallow = false;
        }
    }

    private void OnEnable()
    {
        playercontrols.Enable();
    }

    private void OnDisable()
    {
        playercontrols.Disable();
    }

    // Update is called once per frame


    void Update()
    {
        if (!PauseScreen.paused)
        {
            dir = body.transform.right;
            direction = playercontrols.Controls.Movement.ReadValue<Vector2>();
            aim = playercontrols.Controls.Aim.ReadValue<Vector2>();
            horizontal = direction.x;
            vertical = direction.y;
            desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(MaxSpeed - ground.GetFriction(), 0f);
            if (isWallSliding || isWallJumping)
            {
                shortJump = true;
            }
            velocity = body.velocity;

            OnGround = ground.GetOnGround();
            /*if (isWallSliding && velocity.y <= 0)
            {
                OnGround = false;
            }*/
            velocity = body.velocity;
            fallremember -= Time.deltaTime;

            acceration = OnGround ? acceration : MaxAirAcc;
            MaxSpeedChange = acceration * Time.deltaTime;

            if (velocity.x < 0 && horizontal > 0 && !isWallSliding)
            {
                velocity.x += directionfactor;
            }

            else if (velocity.x > 0 && horizontal < 0 && !isWallSliding)
            {
                velocity.x -= directionfactor;
            }

            if (Mathf.Abs(velocity.x) != 0 && isWallSliding)
            {
                velocity.x = 0;
            }

            if (!isWallJumping && !isWallSliding && allowed)
            {
                velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, MaxSpeedChange);
                body.velocity = velocity;
            }

            if (OnGround)
            {
                fallremember = fallrem;
                shortJump = false;
                JumpPhase = 0;
                if (!dashallow)
                    dashallow = true;
            }

            if (velocity.y > 1)
            {
                JumpPhase = 1;
            }


            jumpremtime -= Time.deltaTime;
            dashtimer += Time.deltaTime;


            if ((jumpremtime > 0) && (fallremember > 0))
            {
                JumpAction();
                jumpremtime = 0;
            }

            if (!allowed && (velocity.x > 0.5 || velocity.x < -0.5))
            {
                velocity.x -= attackspeedreduser * transform.right.x;
                velocity.y = 0;
                body.velocity = velocity;
                if (Mathf.Abs(velocity.x) <= 1f)
                {
                    velocity.x = 0;
                }
            }

            if (dashtimer >= dashtime)
            {
                anim.SetBool("IsDash", false);
            }

            if (dashtimer < dashtime && !isWallSliding)
            {
                anim.SetBool("IsDash", true);
                velocity.x = dashspeed * transform.right.x;
                velocity.y = 0;
                //allowed = false;
                body.velocity = velocity;

            }
            /*else if (anim.GetBool("IsAttack1") || anim.GetBool("IsAttack2") || anim.GetBool("IsAttack3") || anim.GetBool("IsUlt") || anim.GetBool("IsUltEnd"))
            {
                allowed = false;
            }
            else
            {
                allowed = true;
            }*/

            if (allowed)
            {
                if (body.velocity.y > 0f)
                {
                    if (shortJump)
                    {
                        body.gravityScale = UpwardMovementMultiplier / jumpmod;
                    }
                    else
                    {
                        body.gravityScale = UpwardMovementMultiplier;
                    }

                }
                else if (body.velocity.y < 0f)
                {
                    body.gravityScale = DownwardMovementMultiplier;
                }
                else if (body.velocity.y == 0f)
                {
                    body.gravityScale = defaultGravityScale;
                }

                if (-velocity.y > maxfallspeed)
                    velocity.y = -maxfallspeed;
                if (velocity.y > WallJumpPower.y)
                {
                    velocity.y = WallJumpPower.y;
                }
                OnEdge = edge.IsOnEdge();
                WallSlide();
                WallJump();

            }
            body.velocity = velocity;
        }
    }
        
    private void JumpAction()
    {
        if (JumpPhase <= MaxAirJumps)
        {
            dust.Play();
            JumpPhase += 1;
            float jumpspeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * JumpHeight);

            velocity.y = jumpspeed;
        }
    }

    private void WallSlide()
    {
        if (OnGround || !wall.iswalled())
        {
            isWallSliding = false;
        }

        if ((!OnGround && !isWallSliding && velocity.y < 0f) || isWallSliding)
        {

            if (wall.iswalled() && !OnGround && (body.transform.right.x == -1 && horizontal <= -0.75f || body.transform.right.x == 1 && horizontal >= 0.75f))
            {
                isWallSliding = true;
                
            }

            /*else if (wall.iswalled() && !OnGround && (body.transform.right.x == -1 && horizontal >= 0.25f || body.transform.right.x == 1 && horizontal <= -0.25f))
            {
                //isWallSliding = true;
                velocity.x = 0;
                velocity.y = -wallslidespeed;
            }*/

            if (isWallSliding)
            {
                if (!dashallow)
                    dashallow = true;
                isWallSliding = true;
                velocity.x = 0;
                velocity.y = -wallslidespeed;
            }

            else
            {
                isWallSliding = false;
            }

            if (isWallSliding && !edge.Redbox)
            {
                velocity.x = 0;
                velocity.y = 0;
                body.gravityScale = 0;
                if (!edge.greenBox && allowed)
                {
                    body.gravityScale = wallslidespeed;
                    velocity.y = -maxwallslidespeed;
                }
                if (!dashallow)
                    dashallow = true;
            }
        }

    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            walljumpdirection = -transform.right.x;

            CancelInvoke(nameof(stopwalljumping));
        }
    }

    public float FindAnimation(Animator animator, string name)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip.length;
            }
        }

        return 0;
    }

    private void stopwalljumping()
    {
        isWallJumping = false;
    }
    public void Jump()
    {
        if (allowed)
        {
            jumpremtime = jumperem;

            if (OnEdge && horizontal != -(transform.right.x))
            {
                readytomove = true;
            }

            if (isWallSliding/* && (body.transform.right.x == -1 && horizontal >= -0.25f || body.transform.right.x == 1 && horizontal <= 0.25f)*/)
            {
                if (vertical <= -0.75f /*(body.transform.right.x == -1 && horizontal <= -0.75f || body.transform.right.x == 1 && horizontal >= 0.75f)*/)
                {
                    isWallJumping = true;
                    isWallSliding = false;
                    velocity.y = 0;
                    velocity.x = -body.transform.right.x * 0.05f;
                }
                else if (body.transform.right.x == -1 && horizontal >= -0.25f || body.transform.right.x == 1 && horizontal <= 0.25f)
                {
                    isWallJumping = true;
                    isWallSliding = false;
                    velocity.x = WallJumpPower.x * (walljumpdirection);
                    velocity.y = WallJumpPower.y;
                }

                Invoke(nameof(stopwalljumping), walljumpduration);
            }

            if (OnEdge && (vertical >= 0.75f || ((body.transform.right.x == -1 && horizontal <= -0.75f) || (body.transform.right.x == 1 && horizontal >= 0.75f))))
            {
                anim.SetBool("IsClimb", true);
            }

            body.velocity = velocity;
        }
       

    }

    public void JumpOver()
    {
        if (body.velocity.y > 0f)
            shortJump |= true;
    }

}
