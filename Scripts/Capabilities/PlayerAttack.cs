using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask Enemy;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Attack()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(("Enemy")))
        {
            Debug.Log("Called");
            collision.GetComponent<Enemy>().TakeDamage(damage);
            collision.GetComponent<Enemy>().blood.Play();
        }
    }
}
