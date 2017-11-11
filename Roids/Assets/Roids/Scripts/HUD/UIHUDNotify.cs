using UnityEngine;
using System.Collections;

public class UIHUDNotify : MonoBehaviour 
{
	[SerializeField]
	TMPro.TextMeshProUGUI txtText;
    [SerializeField]
    TMPro.TextMeshProUGUI txtHeading;

    float timeSinceOpened = 0;
	float duration = 0;

	public void Popup(string text, string heading, float duration)
	{
		txtText.text = text;
        txtHeading.text = heading;

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
