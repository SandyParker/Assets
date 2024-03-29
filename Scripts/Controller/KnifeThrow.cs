using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KnifeThrow : MonoBehaviour
{
    public Vector3 defaultpos;
    public bool isthrown;
    public float speed;
    public GameObject thrower;
    public float firespeed;
    public Transform Firepoint;
    private Rigidbody2D rb;
    private Vector2 direction;
    private Vector2 velocity;
    private float timer;
    private PlayerControls playercontrols;
    public Blink playertp;
    public bool pressed;
    public Rewind revind;
    // Start is called before the first frame update

    [Header("Slomo")]
    [SerializeField, Range(0, 1f)] private float slomofactor;
    public bool slow;
    public float slowfactor;
    private bool onair;
    public Cooldowns cooldown;
    public Player player;
    public SpriteRenderer arrow;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isthrown = false;
        velocity = transform.right*speed*0;
        rb.velocity = velocity;
        onair = true;
        pressed = false;
        playercontrols = new PlayerControls();

        playercontrols.Controls.Skill.canceled += _ => tpunpressed();
        playercontrols.Controls.Skill.performed += _ => tppressed();

    }

    public void tppressed()
    {
        if (!cooldown.iscooldown && (player.mana>= playertp.manacost || playertp.isthrown))
        {
            slow = true;
            if (onair)
            {
                onair = false;
            }  
            pressed = true;
            playertp.Throw();
        }
    }

    public void tpunpressed()
    {
        slow = false;
        if (pressed && !cooldown.iscooldown)
        {
            pressed = false;
            if (isthrown)
            {
                timer = 0;
                isthrown = false;
                onair = true;
            }
            else
            {
                Shoot();
            }
        }
    }

    private void OnEnable()
    {
        playercontrols.Enable();
    }

    private void OnDisable()
    {
        playercontrols.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseScreen.paused)
        {
            timer += Time.deltaTime;

            if (slow)
            {
                Time.timeScale -= slowfactor;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                
            }
            else
            {
                if (!revind.isRewinding)
                {
                    Time.timeScale = 1;
                    Time.fixedDeltaTime = Time.timeScale * 0.02f;
                }        
            }

            if (pressed)
            {
                arrow.color += new Color(0, 0, 0, Time.unscaledDeltaTime * 2);
                if (arrow.color.a > 1)
                    arrow.color += new Color(255, 255, 255, 1);
            }

            else
            {
                arrow.color = new Color(255, 255, 255, 0);
                
            }

            if (!revind.isRewinding)
                Time.timeScale = Mathf.Clamp(Time.timeScale, slomofactor, 1f);
            velocity = direction * speed;
            rb.velocity = velocity;
            if (!isthrown && timer >= 0.001f)
            {
                transform.position = defaultpos;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            speed = 0;
            velocity.y = 0;
        }
    }

    void Shoot()
    {
        transform.rotation = Firepoint.rotation;
        direction = Firepoint.right;
        transform.position = Firepoint.position;
        speed = firespeed;
        isthrown = true;
    }

    public Vector3 destination()
    {
        return transform.position;
    }
}
