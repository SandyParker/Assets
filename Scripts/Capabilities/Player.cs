using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxhp;
    public float hp;
    public HealthBar bar;

    private void Awake()
    {
        hp = maxhp;
        bar.SetMaxHealth(maxhp);
    }

    private void Update()
    {
        bar.SetHealth(hp);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log("Player Damage Taken");
    }
}
