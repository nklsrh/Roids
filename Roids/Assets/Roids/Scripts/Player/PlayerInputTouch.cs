using UnityEngine;
using System.Collections;

public class PlayerInputTouch : PlayerInput
{
    [SerializeField]
    float dragMultiplier = 1.0f;
    [SerializeField]
    float deadzone = 10.0f;

    private Vector2 currentTouchAnchor;
    private Vector2 currentTouchPos;
    
    private Vector2 GetFingerPos()
    {
        if (Input.touchCount > 0)
        {
            return Input.touches[0].position;
        }
        else
        {
            return Input.mousePosition;
        }
    }

    bool DidJustTouched
    {
        get
        {
            return (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) || (Input.GetMouseButtonDown(0));
        }
    }

    bool IsTouching
    {
        get
        {
            return (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved) || (Input.GetMouseButton(0));
        }
    }
    
    public override void InputLogic()
    {
        if (DidJustTouched)
        {
            currentTouchAnchor = GetFingerPos();
        }
        else if (IsTouching)
        {
            currentTouchAnchor = currentTouchPos;
            currentTouchPos = GetFingerPos();
        }
        else
        {
            currentTouchAnchor = currentTouchPos;
        }
        
        float delta = (currentTouchPos - currentTouchAnchor).x / Screen.width;
        float turn = delta * dragMultiplier;

        if (Mathf.Abs(delta) <= deadzone)
        {
            turn *= 0.25f;
        }

        if (turn != 0);
        {
            playerController.Turn(turn);
        }

        if (Input.GetButton("Fire"))
        {
            playerController.Fire();
        }

        float thrust = Input.touchCount > 1 ? 1.0f : Input.GetAxis("Vertical");
        // float thrust = Input.GetAxis("Vertical");
        // if (thrust > 0)
        {
            playerController.Thrust(thrust);
        }
    }
}
