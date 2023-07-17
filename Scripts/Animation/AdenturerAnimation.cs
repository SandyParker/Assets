using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class AdenturerAnimation : MonoBehaviour
{
    public Rigidbody2D body;
    public Animator anim;
    private bool facingright = true;
    public ParticleSystem dust;
    public Wall wall;
    public Ground ground;
    public EdgeClimb edge;
    private float horizontal;
    private float vertical;
    public PlayerAttack playerattack;
    public MoveUpdate moveupdate;
    private PlayerInput input;
    private PlayerControls playercontrols;
    void Awake()
    {
        //Cursor.visible = false;
        anim.SetBool("IsAttack1", false);
        playercontrols = new PlayerControls();
        input = GetComponent<PlayerInput>();

        
    }

    private void OnEnable()
    {
        playercontrols.Enable();
    }

    private void OnDisable()
    {
        playercontrols.Disable();
    }

    /*public void edgeclimb()
    {
        body.transform.position += new Vector3(moveupdate.moveOffset.x * body.transform.right.x, moveupdate.moveOffset.y);
        moveupdate.readytomove = false;
        anim.SetBool("IsClimb", false);
    }*/

    void Attack1Done()
    {
        anim.SetBool("IsAttack1", false);
        playerattack.enabled = false;

    }

    void Attack2Done()
    {
        anim.SetBool("IsAttack2", false);
        playerattack.enabled = false;
    }  
    
    void Attack3Done()
    {
        anim.SetBool("IsAttack3", false);
        playerattack.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (body.velocity.y == 0f)
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);
            anim.SetBool("IsDouble", false);
        }


        if (body.velocity.y > 1f)
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsJumping", true);
            anim.SetBool("IsFalling", false);
            anim.SetBool("IsDouble", false);
        }

        else if (body.velocity.y < -1f)
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", true);
            anim.SetBool("IsDouble", false);
        }

        else if (Mathf.Abs(body.velocity.x) > 0.1)    
        {
            anim.SetBool("IsRunning", true);
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);
            anim.SetBool("IsDouble", false);
        }
        else
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);
            anim.SetBool("IsDouble", false);
        }

        if (wall.iswalled() && !ground.GetOnGround() && moveupdate.isWallSliding)
        {
            anim.SetBool("IsWalled", true);
        }
        
        else if (!wall.iswalled() || !moveupdate.isWallSliding)
        {
            anim.SetBool("IsWalled", false);
        }

        if (body.velocity.x == 0 && body.velocity.y == 0)
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);
            anim.SetBool("IsDouble", false);
        }
    }

    public void Attack()
    {
        if (ground.OnGround)
        {
            if (anim.GetBool("IsAttack2"))
            {
                anim.SetBool("IsAttack3", true);
                playerattack.enabled = true;
            }

            else if (anim.GetBool("IsAttack1"))
            {
                anim.SetBool("IsAttack2", true);
                playerattack.enabled = true;
            }

            else if (!anim.GetBool("IsAttack2") && !anim.GetBool("IsAttack3"))
            {
                anim.SetBool("IsAttack1", true);
                playerattack.enabled = true;
            }
        }
    }

    private void LateUpdate()
    {
        /*if (body.velocity.x>0)
        {
            facingright = true;
        }
        else if (body.velocity.x<0)
        {
            facingright = false;
        }

        if ((facingright && (localscale.x<0)) || (!facingright && (localscale.x>0)))
        {
            dust.Play();
            localscale.x *= -1;
        }*/

        if (body.velocity.x>0 && !facingright)
        {
            Flip();
        }
        else if(body.velocity.x<0 && facingright)
        {
            Flip();
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

    public bool GetSide()
    {
        return facingright;
    }
    public void Flip()
    {
        facingright = !facingright;
        body.transform.Rotate(0, 180, 0);
    }
}
