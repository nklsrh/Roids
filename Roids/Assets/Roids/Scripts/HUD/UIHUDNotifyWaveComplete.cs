using UnityEngine;
using System.Collections;

public class UIHUDNotifyWaveComplete : MonoBehaviour 
{
	[SerializeField]
	TMPro.TextMeshProUGUI txtText;

	float timeSinceOpened = 0;
	float duration = 0;

	public void Popup(string text, float duration)
	{
		txtText.text = text;
		timeSinceOpened = 0;
		this.duration = duration;

	
		Enable();
	}

	public void Enable()
	{
		gameObject.SetActive(true);
	}

	public void Disable()
	{
		gameObject.SetActive(false);
	}

	void Update()
	{
		if (duration != 0 && timeSinceOpened < duration)
		{
			timeSinceOpened += Time.deltaTime;
			if (timeSinceOpened >= duration)
			{
				duration = 0;
				Disable();
			}
		}
	}
}
