using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "PlayerController", menuName = "InputController/PlayerController")]

public class PlayerController : InputMovement
{
    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }
}
