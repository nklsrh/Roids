using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMenuController : MonoBehaviour
{
    public Button btnStart;

    void Start()
    {
        btnStart.onClick.RemoveAllListeners();
        btnStart.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        Application.LoadLevel("scene_buffer");
    }
}
