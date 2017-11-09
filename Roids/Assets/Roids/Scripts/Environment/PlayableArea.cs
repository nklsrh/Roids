using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class PlayableArea : MonoBehaviour
{
    public Vector3 areaSize = Vector2.one * 1;

    private Vector3 areaSizeScaled;

    public void Teleport(Transform t)
    {
        areaSizeScaled = Vector3.Scale(areaSize, transform.localScale);
        Vector3 halfAreaSizeScaled = areaSizeScaled / 2;

        //Vector3 positionLocalToArea = transform.InverseTransformPoint(t.transform.position);

        if (t.position.x > halfAreaSizeScaled.x)
        {
            t.position = new Vector3(-halfAreaSizeScaled.x, t.position.y, t.position.z);
        }
        if (t.position.x < -halfAreaSizeScaled.x)
        {
            t.position = new Vector3(halfAreaSizeScaled.x, t.position.y, t.position.z);
        }
        if (t.position.z > halfAreaSizeScaled.z)
        {
            t.position = new Vector3(t.position.x, t.position.y, -halfAreaSizeScaled.z);
        }
        if (t.position.z < -halfAreaSizeScaled.z)
        {
            t.position = new Vector3(t.position.x, t.position.y, halfAreaSizeScaled.z);
        }


        //t.position = transform.TransformPoint(positionLocalToArea);
    }
}
