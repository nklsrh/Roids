using UnityEngine;
using UnityEngine.UI;

public class UIHUDTimer : MonoBehaviour 
{
	[SerializeField]
	Image imgTimeFill;

    [SerializeField]
    TMPro.TextMeshProUGUI txtTimer;

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

            txtTimer.text = currentTime.ToString("0.0");

            if (currentTime <= 0)
			{
				Disable();
			}
		}
	}
}
