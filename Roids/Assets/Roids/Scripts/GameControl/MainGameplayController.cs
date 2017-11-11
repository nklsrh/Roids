using UnityEngine;
using System.Collections;

public class MainGameplayController : MonoBehaviour
{
	public UIHUDController uiController;
	public GameDirector gameDirector;
	public CameraController cameraController;
    public PostGameController postgameController;

    public enum GameState
    {
        None,
        Pregame,
        Gameplay,
        Paused,
        Postgame,
    }
    GameState currentState;

    System.Action currentLoop;
    System.Action currentLateLoop;

    float currentStateTime = 0.0f;

    const float TIME_WAIT_BEFORE_GAME_END = 1.0f;

    void Start ()
    {
        postgameController.Disable();

        SwitchState(GameState.Pregame);

        gameDirector.Setup();
        gameDirector.onWaveFailed += OnGameOver;

        uiController.Setup(gameDirector, cameraController);

		cameraController.Setup(gameDirector.player);

        gameDirector.StartWave();

        SwitchState(GameState.Gameplay);
    }

    void SwitchState(GameState newState)
    {
        if (newState != currentState)
        {
            currentLoop = null;
            currentLateLoop = null;

            switch (newState)
            {
                case GameState.Gameplay:
                    currentLoop = MainGameplayLoop;
                    currentLateLoop = MainGameplayLateLoop;
                    break;
                case GameState.Paused:
                    currentLoop = PausedLoop;
                    break;
                case GameState.Postgame:
                    currentLoop = PostgameLoop;
                    currentLateLoop = PostgameLateLoop;
                    break;
            }

            currentStateTime = 0;
            currentState = newState;
        }
    }

    void MainGameplayLoop()
    {
        gameDirector.Logic();
        uiController.Logic();
    }

    void MainGameplayLateLoop()
    {
        cameraController.Logic();
    }

    void PausedLoop()
    {

    }

    void PostgameLoop()
    {
        if (currentStateTime > TIME_WAIT_BEFORE_GAME_END)
        {
            Time.timeScale = 1f;
            if (!postgameController.IsSetup)
            {
                uiController.HideHUD();
                postgameController.Setup(gameDirector.levelController.Score);
            }
            SwitchState(GameState.None);
        }
        else
        {
            Time.timeScale = 0.3f;
            MainGameplayLoop();
        }
    }

    void PostgameLateLoop()
    {
        if (currentStateTime > TIME_WAIT_BEFORE_GAME_END)
        {
        }
        else
        {
            cameraController.Logic();
        }
    }

    private void OnGameOver(Wave wave)
    {
        ExplosionObject explosion = GameDirector.Explosion(gameDirector.player.transform.position, 1.0f);

        cameraController.Setup(explosion.transform);
        cameraController.offset += Vector3.right * 10 + Vector3.up * 8;

        SwitchState(GameState.Postgame);
    }


    void Update()
    {
        if (currentLoop != null)
        {
            currentLoop.Invoke();
        }
        currentStateTime += Time.deltaTime;
    }

    void LateUpdate()
    {
        if (currentLateLoop != null)
        {
            currentLateLoop.Invoke();
        }
    }
}
