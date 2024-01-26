using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShoot : MonoBehaviour
{
    public float speed = 30;
    public float damage = 30;
    public Rigidbody2D rb;
    public float mana;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.up * speed;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);

        if (collision.CompareTag("Player") || collision.CompareTag("Obstacle"))
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Player>().TakeDamage(damage,mana);
            }
            Destroy(gameObject);
        }
    }
}
