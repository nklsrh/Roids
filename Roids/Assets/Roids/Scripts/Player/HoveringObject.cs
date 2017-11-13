using UnityEngine;
using System.Collections;

public class HoveringObject : MonoBehaviour
{
    public float hoverBobSpeed = 3;
    public float hoverHeight = 0.1f;
    public float hoverWobble = 4;
    public float hoverWobbleSpeed = 4.5f;
    public bool startRandom = true;

    Vector3 startingPosition;
    Quaternion startingRotation;
    float t;

    void Start()
    {
        startingPosition = transform.localPosition;
        startingRotation = transform.localRotation;

        if (startRandom)
        {
            t = UnityEngine.Random.Range(0, 100.0f);
        }
    }

    void Update ()
    {
        t += Time.deltaTime;

        transform.localPosition = startingPosition + Vector3.up * ((Mathf.Sin(t * hoverBobSpeed) + 1) / 2) * hoverHeight;    // sine curve with lowest point at 0, not -1

        transform.localRotation = startingRotation 
        * Quaternion.AngleAxis(Mathf.Sin(t * hoverWobbleSpeed) * hoverWobble, Vector3.left)
        * Quaternion.AngleAxis(Mathf.Cos(t * hoverWobbleSpeed) * hoverWobble * 0.5f, Vector3.forward);
	}
}
