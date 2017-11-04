using UnityEngine;
using System.Collections;

public class PlayerInputTouch : PlayerInput
{
     
    public override void InputLogic()
    {
        Vector3 inputAxis = Input.mousePosition.normalized;

        playerController.Turn(inputAxis.x);
    }
}
