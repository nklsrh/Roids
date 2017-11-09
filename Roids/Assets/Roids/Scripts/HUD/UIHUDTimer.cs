using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHUDTimer : MonoBehaviour 
{
	[SerializeField]
	Image imgTimeFill;

	float duration;
	float currentTime;

	public void Enable()
	{
		gameObject.SetActive(true);
	}

	public void Disable()
	{
		gameObject.SetActive(false);
	}

	public void StartTimer(float duration)
	{
		this.duration = duration;
		currentTime = duration; 
	}

	void Update()
	{
		if (currentTime > 0)
		{
			currentTime -= Time.deltaTime;
			imgTimeFill.fillAmount = currentTime / duration;

			if (currentTime <= 0)
			{
				Disable();
			}
		}
	}
}
