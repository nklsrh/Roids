using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TeleportTrigger : MonoBehaviour
{
    public Transform teleportDestination;

    public void OnTriggerEnter (Collider other)
    {
        other.transform.position = teleportDestination.transform.position;
    }
}
