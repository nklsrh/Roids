using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHUDHealth : MonoBehaviour 
{
	public Image imgFill;

    [HideInInspector]
    public HealthController healthController;

    Transform tracking;
	CameraController trackingCamera;
    RectTransform canvas;
    bool trackWorldSpace = true;

	public void Enable()
	{
		gameObject.SetActive(true);
	}

	public void Disable()
	{
		gameObject.SetActive(false);
	}

	public void Setup(RectTransform canvas, HealthController healthController, CameraController cam, bool trackWorldSpace = true)
	{
		this.healthController = healthController;
		this.tracking = healthController.transform;
		this.trackingCamera = cam;
        this.canvas = canvas;
        this.trackWorldSpace = trackWorldSpace;
    }

	void LateUpdate()
    {
        if (healthController != null)
        {
            imgFill.fillAmount = healthController.Health / healthController.HealthMax;
        }

        if (tracking != null && trackWorldSpace)
		{
			Vector3 trackedPosition = tracking.transform.position;
			Camera cam = trackingCamera.Camera;
			Vector3 screenPos = cam.WorldToScreenPoint(trackedPosition);

            //transform.position = screenPos;
            GetComponent<RectTransform>().localPosition = screenPos;
        }
	}
}
