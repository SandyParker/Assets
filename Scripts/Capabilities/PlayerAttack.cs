using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask Enemy;
    public float damage;
    public Player player;
    public float manahit;
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
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            collision.GetComponent<Enemy>().blood.Play();
            player.mana += manahit;
        }
        if (collision.CompareTag("Switch"))
        {
            collision.GetComponent<SwitchController>().SwitchTrigger.Invoke();
            collision.GetComponent<Animator>().SetTrigger("On");
        }
    }
}
