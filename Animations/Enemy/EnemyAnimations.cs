using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    public Rigidbody2D body;
    public Animator anim;
    private Vector3 localscale;
    public Enemy core;
    public EnemyAttack enemyattack;
    void Start()
    {
        anim = GetComponent<Animator>();
        localscale = transform.localScale;
    }

    void AttackDone()
    {
        anim.SetBool("IsAttack", false);
        enemyattack.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(body.velocity.x) > 0)
        {
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }

        if (core.HP<=0)
        {
            anim.SetBool("IsDead", true);
        }
    }
}
