using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class OutOfBoundsArea : MonoBehaviour
{
    public Vector3 teleportAxes = new Vector3(1, 0, 1);     // only teleport on the X and Z axes so we stay on a flat surface
    public Transform playAreaTransform;

    public void Setup()
    {

    }

    public void OnTriggerEnter (Collider other)
    {
        // take the players position relative to this area
        // and then decide where to move player relative to that
        // use teleportAxes to decide which axes contribute to the teleport

        Vector3 relativePosition = playAreaTransform.InverseTransformPoint(other.transform.position);

        Vector3 newPosition = Vector3.Scale(-relativePosition, teleportAxes);

        other.transform.position = playAreaTransform.TransformPoint(newPosition * 0.9f);
    }
}
