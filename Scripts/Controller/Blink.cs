using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Blink : MonoBehaviour
{
    public bool isthrown;
    public GameObject knife;
    private PlayerControls playercontrols;
    public Cooldowns cooldown;
    public KnifeThrow knifethrow;
    public Player player;
    [SerializeField,Range(0f,100f)] public float manacost;
    void Awake()
    {
        playercontrols = new PlayerControls();
        playercontrols.Controls.Skill.performed += _ => Throw();
    }

    // Update is called once per frame
    public void Throw()
    {
        if (isthrown)
        {
             isthrown = false;
             knifethrow.pressed = false;
             knifethrow.slow = false;
             knifethrow.isthrown = false;
             transform.position = knife.transform.position;
             cooldown.iscooldown = true;
             cooldown.image.fillAmount = 0;
        }
        else if (!isthrown && !cooldown.iscooldown && player.mana>=manacost)
        {
            isthrown = true;
            player.mana-=manacost;
        }
    }

    
}
