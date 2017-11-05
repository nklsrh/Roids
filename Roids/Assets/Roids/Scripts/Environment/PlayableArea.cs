using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class PlayableArea : MonoBehaviour
{
    public Vector3 teleportAxes = new Vector3(1, 0, 1);     // only teleport on the X and Z axes so we stay on a flat surface

    public void Setup()
    {

    }

    public void OnTriggerExit (Collider other)
    {
        // take the players position relative to this area
        // and then decide where to move player relative to that
        // use teleportAxes to decide which axes contribute to the teleport

        Vector3 relativePosition = transform.InverseTransformPoint(other.transform.position);

        Vector3 newPosition = Vector3.Scale(-relativePosition, teleportAxes);

        other.transform.position = transform.TransformPoint(newPosition);
    }
}
