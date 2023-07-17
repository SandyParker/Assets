using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackandTakedown : MonoBehaviour
{
    private PlayerControls playercontrols;
    private PlayerInput input;
    public AdenturerAnimation anim;
    public Tackdownable takedown;
    public GameObject point;

    void Awake()
    {
        playercontrols = new PlayerControls();
        input = GetComponent<PlayerInput>();
        playercontrols.Controls.Attack.performed += _ => Attack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        Debug.Log("Called");
        if (takedown.tackdownable)
        {
            transform.position = point.transform.position;
            anim.Attack();
        }
        else
        {
            anim.Attack();
        }  
    }
}
