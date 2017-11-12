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
    bool trackWorldSpace = true;

	public void Enable()
	{
		gameObject.SetActive(true);
	}

	public void Disable()
	{
		gameObject.SetActive(false);
	}

	public void Setup(HealthController healthController, CameraController cam, bool trackWorldSpace = true)
	{
		this.healthController = healthController;
		this.tracking = healthController.transform;
		this.trackingCamera = cam;
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
            TrackTargetWorldSpace();
        }
    }

    void TrackTargetWorldSpace()
    {
        Vector3 trackedPosition = tracking.transform.position;
        Camera cam = trackingCamera.Camera;
        Vector3 screenPos = cam.WorldToScreenPoint(trackedPosition);

        GetComponent<RectTransform>().localPosition = screenPos;
    }
}
