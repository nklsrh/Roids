using UnityEngine;
using System.Collections;

public class UILoadingController : MonoBehaviour
{
    public string sceneName = "scene_menu";
    public float duration = 5f;

    float timeLeft = 0;

	void Start ()
    {
        timeLeft = duration;
    }
	
	void Update ()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Application.LoadLevel(sceneName);
        }
	}
}
