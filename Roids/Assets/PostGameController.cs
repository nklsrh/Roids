using UnityEngine;
using System.Collections;

public class PostGameController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI txtScore;

    public bool IsSetup
    {
        get; private set;
    }

    public void Setup(int score)
    {
        txtScore.text = "SCORE: " + score.ToString("##,##0");

        IsSetup = true;

        gameObject.SetActive(true);
    }

    public void OnAnimationComplete()
    {
        Application.LoadLevel("scene_menu");
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
