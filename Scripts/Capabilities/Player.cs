using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxhp;
    public float hp;
    public float showhp;
    public float hpdropspeed;
    public HealthBar bar;

    private void Awake()
    {
        hp = maxhp;
        showhp = maxhp;
        bar.SetMaxHealth(maxhp);
    }

    private void Update()
    {
        bar.SetHealth(showhp);
        if (showhp > hp) 
        {
            showhp -= hpdropspeed * Time.deltaTime;
        }
        else if (showhp < hp)
        {
            showhp += hpdropspeed * Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
    }
}
