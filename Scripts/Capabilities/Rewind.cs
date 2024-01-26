using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Rewind : MonoBehaviour
{
    public Animator anim;
    public bool isRewinding = false;
    private PlayerControls playercontrols;
    List<PointInTime> POI;
    public MoveUpdate moveUpdate;
    private Rigidbody2D rb;
    public float revindTime;
    public AdenturerAnimation AA;
    public PlayerGhost ghost;
    public Cooldowns cooldown;
    public Player player;
    public HealthBar bar;
    private float gain;
    private float gainshow;
    public float gainspeed;
    [SerializeField, Range(0f, 100f)] public float manacost;

    // Start is called before the first frame update
    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        POI= new List<PointInTime>();
        playercontrols = new PlayerControls();
        bar.SetMaxHealth(100);

        playercontrols.Controls.Enable();
        playercontrols.Controls.Rewind.performed += _ => Revind();
    }


    // Update is called once per frame
    void Update()
    {
        if (gainshow > gain)
        {
            gainshow -= Time.deltaTime * gainspeed;
        }
        else if (gainshow < gain) 
        {
            gainshow += Time.deltaTime * gainspeed;
        }

        if (gain + 1 > gainshow && gain - 1 < gainshow)
        {
            gainshow = gain;
        }

        bar.SetHealth(gainshow);

        if (isRewinding)
        {
            Revibe();
        }
        else
        {
            Record();
        }
    }


    void Revibe()
    {
        if (POI.Count>2 && !cooldown.iscooldown)
        {
            PointInTime pointIn = POI[0];
            transform.position = pointIn.Position;
            transform.rotation = pointIn.Rotation;
            anim.SetBool("IsRunning", pointIn.Running);
            anim.SetBool("IsJumping", pointIn.Jumping);
            anim.SetBool("IsDouble", pointIn.Double);
            anim.SetBool("IsFalling", pointIn.Falling);
            anim.SetBool("IsWalled", pointIn.Wall);
            anim.SetBool("IsClimb", pointIn.Climb);
            anim.SetBool("IsAttack1", pointIn.Attack1);
            anim.SetBool("IsAttack2", pointIn.Attack2);
            anim.SetBool("IsAttack3", pointIn.Attack3);
            anim.SetBool("IsDash", pointIn.Dash);
            if (pointIn.Dash && moveUpdate.dashtimer >= moveUpdate.dashtime)
                moveUpdate.dashtimer = 0;
            POI.RemoveAt(0);
            POI.RemoveAt(0);
        }
        else
        {
            stopRevind();
        }
    }

    void Record()
    {
        if (POI.Count> Mathf.Round(revindTime/Time.deltaTime))
        {
            POI.RemoveAt(POI.Count-1);
            gain = POI[0].HP + (POI[POI.Count - 1].HP - POI[0].HP) / 2;
        }
        POI.Insert(0,new PointInTime(transform.position,transform.rotation, player.hp, anim.GetBool("IsRunning"), anim.GetBool("IsJumping"), anim.GetBool("IsDouble"), anim.GetBool("IsFalling"), anim.GetBool("IsWalled"), anim.GetBool("IsClimb"), anim.GetBool("IsAttack1"), anim.GetBool("IsAttack2"), anim.GetBool("IsAttack3"), anim.GetBool("IsDash")));
    }

    public void Revind()
    {
        if (player.mana >= manacost)
        {
            player.mana -= manacost;
            Time.timeScale = 0;
            gain = POI[0].HP + (POI[POI.Count - 1].HP - POI[0].HP) / 2;
            if (!cooldown.iscooldown)
                player.hp = POI[POI.Count - 1].HP > POI[0].HP ? POI[0].HP + (POI[POI.Count - 1].HP - POI[0].HP) / 2 : POI[0].HP;
            isRewinding = true;
            rb.isKinematic = true;
            MoveUpdate.allowed = false;
            anim.SetFloat("Direction", -2);
            AA.enabled = false;
            ghost.makeghost = true;
            Physics2D.IgnoreLayerCollision(7, 6, true);
        }
    }

    public void stopRevind()
    {
        Time.timeScale = 1;
        MoveUpdate.allowed = true;
        isRewinding = false;
        rb.isKinematic = false;
        anim.SetFloat("Direction", 1);
        AA.enabled=true;
        ghost.makeghost = false;
        Physics2D.IgnoreLayerCollision(7, 6, false);
        if (cooldown.image.fillAmount>=1)
            cooldown.image.fillAmount = 0;
        cooldown.iscooldown = true;
    }
}