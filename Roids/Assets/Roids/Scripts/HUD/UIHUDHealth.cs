using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHUDHealth : MonoBehaviour 
{
	public Image imgFill;

	Transform tracking;
	HealthController healthController;
	CameraController trackingCamera;

	public void Enable()
	{
		gameObject.SetActive(true);
	}

	public void Disable()
	{
		gameObject.SetActive(false);
	}

	public void Setup(HealthController healthController, CameraController cam)
	{
		this.healthController = healthController;
		this.tracking = healthController.transform;
		this.trackingCamera = cam;
	}

	void Update()
	{
		if (tracking != null)
		{
			if (healthController != null)
			{
				imgFill.fillAmount = healthController.Health / healthController.HealthMax;
			}

			Vector3 trackedPosition = tracking.transform.position;
			Camera cam = trackingCamera.Camera;
			Vector3 screenPos = cam.WorldToScreenPoint(trackedPosition);

			transform.position = screenPos;
		}
	}
}
