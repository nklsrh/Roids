using UnityEngine;

public class Rotator : MonoBehaviour 
{
	public Vector3 rotationAmount = Vector3.up;
	public float rotationSpeed = 10;

	void Update () 
	{
		transform.Rotate(rotationAmount * rotationSpeed * Time.deltaTime, Space.Self);
	}
}
