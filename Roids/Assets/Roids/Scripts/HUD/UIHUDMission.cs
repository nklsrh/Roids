using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHUDMission : MonoBehaviour 
{
	[SerializeField]
	TMPro.TextMeshProUGUI txtMission;

	public void Enable()
	{
		gameObject.SetActive(true);
	}

	public void Disable()
	{
		gameObject.SetActive(false);
	}

	public void SetText(string text)
	{
		txtMission.text = text;
	}
}
