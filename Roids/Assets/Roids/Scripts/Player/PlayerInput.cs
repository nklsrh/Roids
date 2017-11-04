using UnityEngine;
using System.Collections;

public abstract class PlayerInput : MonoBehaviour
{
    public PlayerController playerController;

    void Update()
    {
        InputLogic();
    }

    public abstract void InputLogic();
}
