using UnityEngine;
using System.Collections;

public class CameraController : BaseObject
{
    public Vector3 offset = Vector3.up + Vector3.back * 20;
    public bool useLocalOffset = false;

    public float lookForward = 1;
    public float lookUsingVelocity = 1;
    public bool disableRotation = false;

    public float lookLerp = 10.0f;
    public float moveLerp = 10.0f;

    public Camera Camera
    {
        get
        {
            if (thisCamera != null)
            {
                return thisCamera;
            }
            return (thisCamera = GetComponent<Camera>());
        }
    }
    Camera thisCamera;

    PlayerController targetPlayer;

    Vector3 movementPosition;
    Vector3 lookAtPosition;

    public override void Setup()
    {
    }

    public void Setup(PlayerController player)
    {
        this.targetPlayer = player;
        lookAtPosition = player.transform.position;
    }

    public override void Logic()
    {
        transform.position = movementPosition;

        if (!disableRotation)
        {
            transform.LookAt(lookAtPosition);
        }

        Vector3 wantedPosition = useLocalOffset ? targetPlayer.transform.TransformPoint(offset) : targetPlayer.transform.position + offset;
        movementPosition = Vector3.Slerp(movementPosition, wantedPosition, moveLerp * Time.deltaTime);
        lookAtPosition = Vector3.Slerp(lookAtPosition, targetPlayer.transform.position + (targetPlayer.transform.forward * lookForward) + (targetPlayer.Velocity * lookUsingVelocity), lookLerp * Time.deltaTime);
    }

}
