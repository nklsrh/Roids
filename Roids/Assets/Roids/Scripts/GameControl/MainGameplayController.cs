using UnityEngine;
using System.Collections;

public class MainGameplayController : MonoBehaviour {
	public UIHUDController uiController;
	public GameDirector gameDirector;
	public CameraController cameraController;

    public enum GameState
    {
        Pregame,
        Gameplay,
        Paused,
        Postgame
    }
    GameState currentState;

    System.Action currentLoop;
    System.Action currentLateLoop;

    void Start ()
    {
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
                    break;
            }

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

    }

    private void OnGameOver(Wave wave)
    {
        SwitchState(GameState.Postgame);
    }



    void Update()
    {
        if (currentLoop != null)
        {
            currentLoop.Invoke();
        }
    }

    void LateUpdate()
    {
        if (currentLateLoop != null)
        {
            currentLateLoop.Invoke();
        }
    }
}
