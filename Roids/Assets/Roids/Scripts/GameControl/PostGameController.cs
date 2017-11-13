using UnityEngine;

public class PostGameController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI txtScore;
    public string sceneName = "scene_menu";

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
        Application.LoadLevel(sceneName);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
