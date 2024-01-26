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

    public SpriteRenderer sprite;
    public int flickerAmnt;
    public float flickerDuration;

    public float maxmana;
    public float mana;
    public float showmana;
    public float manadropspeed;
    public ManaBar manabar;

    private void Awake()
    {
        hp = maxhp;
        showhp = maxhp;
        bar.SetMaxHealth(maxhp);
        mana = 0;
        showmana = 0;
        manabar.SetMaxMana(maxmana);
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

        if (hp+1>showhp && hp-1<showhp)
        {
            showhp = hp;
        }

        if (mana + 1 > showmana && mana - 1 < showmana)
        {
            showmana = mana;
        }

        manabar.SetMana(showmana);
        if (showmana > mana)
        {
            showmana -= manadropspeed * Time.deltaTime;
        }
        else if (showmana < mana)
        {
            showmana += manadropspeed * Time.deltaTime;
        }

        if (mana>maxmana)
        {
            mana = maxmana;
        }
    }

    public void TakeDamage(float damage, float hitmana)
    {
        hp -= damage;
        mana += hitmana;
        StartCoroutine(DamageFlickers());
    }

    IEnumerator DamageFlickers()
    {
        Debug.Log("Flick");
        for (int i = 0; i < flickerAmnt; i++)
        {
            sprite.color = new Color(1f, 1f, 1f, .5f);
            yield return new WaitForSeconds(flickerDuration);
            sprite.color = Color.white;
            yield return new WaitForSeconds(flickerDuration);
        }
    }
}
