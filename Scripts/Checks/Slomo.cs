using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slomo : MonoBehaviour
{
    [SerializeField, Range(0, 1f)] private float slomofactor;
    private bool slow;
    public float slowfactor;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            slow = true;
        }

        else if (Input.GetButtonUp("Fire2"))
        {
            slow = false;
        }

        if (slow)
        {
            Time.timeScale -= slowfactor;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        else
        {
            Time.timeScale += slowfactor;
        }

        Time.timeScale = Mathf.Clamp(Time.timeScale, slomofactor, 1f);
    }
}
