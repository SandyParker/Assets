using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateFirepoint : MonoBehaviour
{
    private float ControllerDeadzone = 0.1f;
    private Camera maincam;
    private Vector3 mousepos;
    public Vector2 aim;
    private PlayerControls playercontrols;
    public MoveUpdate move;
    private float rotatesmoothing = 1000f;  
    void Awake()
    {
        maincam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playercontrols = new PlayerControls();
    }

    // Update is called once per frame
    void Update()
    {
        aim = move.direction;
        if (move.IsGamepad)
        {
            if (Mathf.Abs(aim.x) > ControllerDeadzone || Mathf.Abs(aim.y) > ControllerDeadzone)
            {
                Vector2 aimerdirection = Vector2.left * aim.y + Vector2.up * aim.x;
                if(aimerdirection.sqrMagnitude > 0.0f)
                {
                    Quaternion newrotation = Quaternion.LookRotation(Vector3.forward, aimerdirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, rotatesmoothing);
                }
            }
        }
        else
        {
            mousepos = maincam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 rotation = mousepos - transform.position;
            float rotz = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotz);
        }
        
    }
}
