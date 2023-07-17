using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOver : MonoBehaviour
{
    public Rigidbody2D body;
    public bool isjump;
    void Start()
    {
        isjump = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isjump = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isjump = false;
    }

    public bool shouldjump()
    {
        return isjump;
    }
}
