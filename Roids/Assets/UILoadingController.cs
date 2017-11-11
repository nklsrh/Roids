using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UILoadingController : MonoBehaviour
{
    public List<float> screenDurations = new List<float>();
    public List<RectTransform> screens = new List<RectTransform>();
    public RectTransform objectToShowWhenSkippable;
    public Animation animationControl;

    public string sceneName = "scene_menu";
    public float skipAfterSeconds = 10.0f;

    float timeLeft = -1;
    int currentScreen = 0;
    float totalTimeTaken = 0;

    void Start ()
    {
        timeLeft = -1;
        currentScreen = 0;
        totalTimeTaken = 0;

        for (int i = 0; i < screens.Count; i++)
        {
            screens[i].gameObject.SetActive(false);
        }
        SetCurrentScreen(0);

        if (objectToShowWhenSkippable != null)
        {
            objectToShowWhenSkippable.gameObject.SetActive(false);
        }

        if (animationControl != null)
        {
            animationControl.Play("Menu_WipeIn");
        }
    }
	
	void Update ()
    {
        if (totalTimeTaken > skipAfterSeconds)
        {
            if (objectToShowWhenSkippable != null && !objectToShowWhenSkippable.gameObject.activeSelf)
            {
                objectToShowWhenSkippable.gameObject.SetActive(true);
            }

            if (Input.GetButtonUp("Fire"))
            {
                FinishAllScreens();
            }
        }

        if (timeLeft < 0)
        {
            timeLeft = 0;
            ShowNextScreen();
        }
        else if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }

        totalTimeTaken += Time.deltaTime;
    }

    void ShowNextScreen()
    {
        if (currentScreen < screens.Count - 1)
        {
            SetCurrentScreen(currentScreen + 1);
        }
        else
        {
            FinishAllScreens();
        }
    }

    void FinishAllScreens()
    {
        if (animationControl != null)
        {
            animationControl.Play("Menu_WipeOut");
        }
        StartCoroutine(WaitThenLoadLevel());
    }

    IEnumerator WaitThenLoadLevel()
    {
        yield return new WaitForSeconds(1f);
        Application.LoadLevel(sceneName);
        yield return null;
    }

    void SetCurrentScreen(int i)
    {
        screens[currentScreen].gameObject.SetActive(false);
        currentScreen = i;
        timeLeft = screenDurations[currentScreen];
        screens[currentScreen].gameObject.SetActive(true);
    }
}
