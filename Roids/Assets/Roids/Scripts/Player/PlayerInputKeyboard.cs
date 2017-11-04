using UnityEngine;
using System.Collections;

public class PlayerInputKeyboard : PlayerInput
{

    public override void InputLogic()
    {
        float turn = Input.GetAxis("Horizontal");
        playerController.Turn(turn);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerController.Fire();
        }

        float thrust = Input.GetAxis("Vertical");
        if (thrust > 0)
        {
            playerController.Thrust(thrust);
        }
    }
}
