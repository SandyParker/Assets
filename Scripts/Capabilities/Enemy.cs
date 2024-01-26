using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHP;
    public float HP;
    public ParticleSystem blood;
    public Patrol patrol;
    public EnemyAI AI;
    public FOV fov;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP<=0) 
        {
            patrol.enabled = false;
            AI.enabled = false;
            fov.enabled = false;
            rb.velocity = new Vector2(0, 0);
            Physics2D.IgnoreLayerCollision(6, 7, true);
        }
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        Debug.Log("Damage Taken");
    }
}
