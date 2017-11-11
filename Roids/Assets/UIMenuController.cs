using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMenuController : MonoBehaviour
{
    public Button btnStart;
    public string nextSceneName = "scene_buffer";

    void Start()
    {
        btnStart.onClick.RemoveAllListeners();
        btnStart.onClick.AddListener(StartGame);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire"))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        Application.LoadLevel(nextSceneName);
    }
}
