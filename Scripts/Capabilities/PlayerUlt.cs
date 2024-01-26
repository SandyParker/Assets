using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUlt : MonoBehaviour
{
    public AdenturerAnimation anim;
    public float damage;
    public Transform pos;
    public Transform fix;
    public float tpRange;
    public LayerMask obstacleLayer;
    public GameObject player;

    private Vector2 top_right_corner;
    private Vector2 bottom_left_corner;
    public Vector2 topright;
    public Vector2 bottomleft;
    public KnifeThrow kt;

    public Cooldowns cooldown;
    public Player playerscript;
    [SerializeField, Range(0f, 100f)] public float manacost;

    private Collider2D[] results;

    public CamExplosion explosion;

    private void Start()
    {
        results = new Collider2D[10];
        top_right_corner = new Vector2(topright.x + pos.position.x, topright.y + pos.position.y);
        bottom_left_corner = new Vector2(bottomleft.x + pos.position.x, bottomleft.y + pos.position.y);
    }

    private void Update()
    {
        top_right_corner = new Vector2(topright.x + pos.position.x, topright.y + pos.position.y);
        bottom_left_corner = new Vector2(bottomleft.x + pos.position.x, bottomleft.y + pos.position.y);
        Vector2 center_offset = (top_right_corner + bottom_left_corner) * 0.5f;
        Vector2 displacement_vector = top_right_corner - bottom_left_corner;
        float x_projection = Vector2.Dot(displacement_vector, Vector2.right);
        float y_projection = Vector2.Dot(displacement_vector, Vector2.up);

        Vector2 top_left_corner = new Vector2(-x_projection * 0.5f, y_projection * 0.5f) + center_offset;
        Vector2 bottom_right_corner = new Vector2(x_projection * 0.5f, -y_projection * 0.5f) + center_offset;

        Debug.DrawLine(top_right_corner, top_left_corner,Color.cyan);
        Debug.DrawLine(top_left_corner, bottom_left_corner, Color.cyan);
        Debug.DrawLine(bottom_left_corner, bottom_right_corner, Color.cyan);
        Debug.DrawLine(bottom_right_corner, top_right_corner, Color.cyan);
        if (MoveUpdate.allowed)
            fix.position = pos.position;


    }

    /*public void UltTp()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, tpRange, obstacleLayer);

        if (hit)
        {
            pos.position = hit.point - (Vector2)transform.right;
        }
        else
        {
            pos.position = (Vector2)transform.position + (Vector2)transform.right * tpRange;
        }
        
    }*/

    public void UltDamage()
    {

        results = Physics2D.OverlapAreaAll(top_right_corner, bottom_left_corner);

        foreach (Collider2D col in results)
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.GetComponent<Enemy>().TakeDamage(damage);
                col.GetComponent<Enemy>().blood.Play();
            }
        }

    }
    public void Ult()
    {
        if (!cooldown.iscooldown && playerscript.mana >= manacost)
        {
            playerscript.mana -= manacost;
            kt.slow = true;
            anim.Ult();
        }
    }

    public void UltEnd()
    {
        kt.slow = false;
    }

    public void cambreak()
    {
        explosion.active = true;
    }

}
