using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tackdownable : MonoBehaviour
{
    public bool tackdownable;
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(("Player")))
        {
            tackdownable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(("Player")))
        {
            tackdownable = false;
        }
    }
}
