using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public GameObject guy;
    private float val;
    private Rigidbody2D body;
    private bool facingright;

    public float xoffset;
    public float yoffset;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        body = guy.GetComponent<Rigidbody2D>();
        val = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (body.velocity.x > 0)
        {
            facingright = true;
        }
        else if (body.velocity.x < 0)
        {
            facingright = false;
        }

        if (facingright && val<=xoffset)
            val += speed*Time.deltaTime;
        else if ((!facingright) && val>=-(xoffset))
            val -= speed*Time.deltaTime;

        transform.position = player.transform.position + new Vector3(val, yoffset, -5);
    }
}
