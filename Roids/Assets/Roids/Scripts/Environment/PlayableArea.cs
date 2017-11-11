using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class PlayableArea : MonoBehaviour
{
    public Vector3 areaSize = Vector2.one * 1;
    public bool blockInstead = false;

    private Vector3 areaSizeScaled;

    public static System.Action onLeavingPlayArea;

    public void Teleport(Transform t)
    {
        areaSizeScaled = Vector3.Scale(areaSize, transform.localScale);
        Vector3 halfAreaSizeScaled = areaSizeScaled / 2;

        PlayerController player = t.GetComponent<PlayerController>();

        //Vector3 positionLocalToArea = transform.InverseTransformPoint(t.transform.position);

        float newValX = t.position.x;
        float newValZ = t.position.z;

        if (t.position.x > halfAreaSizeScaled.x)
        {
            newValX = -halfAreaSizeScaled.x;
        }
        if (t.position.x < -halfAreaSizeScaled.x)
        {
            newValX = halfAreaSizeScaled.x;
        }
        if (t.position.z > halfAreaSizeScaled.z)
        {
            newValZ = -halfAreaSizeScaled.z;
        }
        if (t.position.z < -halfAreaSizeScaled.z)
        {
            newValZ = halfAreaSizeScaled.z;
        }


        bool reachedEdgeX = newValX != t.position.x;
        bool reachedEdgeZ = newValZ != t.position.z;
        bool reachedEdge = (reachedEdgeX || reachedEdgeZ);

        if (reachedEdge)
        {
            if (player && blockInstead)
            {
                player.Slowdown();
                player.healthController.Damage(0.5f * Time.deltaTime);

                if (onLeavingPlayArea != null)
                {
                    onLeavingPlayArea.Invoke();
                }
            }
            else
            {
                t.position = new Vector3(newValX, t.position.y, newValZ);
            }
        }
    }
}
