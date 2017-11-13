using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class OutOfBoundsArea : MonoBehaviour
{
    public PlayableArea playAreaTransform;

    public void Setup()
    {

    }

    public void OnTriggerStay (Collider other)
    {
        playAreaTransform.Teleport(other.transform);
    }
}
