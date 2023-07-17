using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float HP;
    public ParticleSystem blood;
    // Start is called before the first frame update
    void Start()
    {
        HP = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        Debug.Log("Damage Taken");
    }
}
