﻿using UnityEngine;
using System.Collections.Generic;

public class MainGameplayController : MonoBehaviour
{
    
    // __________________________________________________________________________________________EDITOR

	public UIHUDController uiController;
	public GameDirector gameDirector;
	public CameraController cameraController;
    public PostGameController postgameController;
    public UITouchController touchController;

    public List<CameraController> cameraList;

    // __________________________________________________________________________________________PUBLICS

    public bool isUsingTouchControls = false;

    public enum GameState
    {
        None,
        Pregame,
        Gameplay,
        Paused,
        Postgame,
    }
    
    // __________________________________________________________________________________________PRIVATES

    GameState currentState;

    System.Action currentLoop;
    System.Action currentLateLoop;

    float currentStateTime = 0.0f;
    int currentCamera = 0;

    const float TIME_WAIT_BEFORE_GAME_END = 1.0f;

    // __________________________________________________________________________________________METHODS

    void Start ()
    {
        postgameController.Disable();

        SwitchState(GameState.Pregame);

        gameDirector.Setup();
        gameDirector.onWaveFailed += OnGameOver;

        uiController.Setup(gameDirector, cameraController);

        if (isUsingTouchControls)
        {
            touchController.gameObject.SetActive(true);
            touchController.Setup(gameDirector.player);

            gameDirector.player.GetComponent<PlayerInput>().enabled = false;
        }
        else
        {
            touchController.gameObject.SetActive(false);

            gameDirector.player.GetComponent<PlayerInput>().enabled = true;
        }

        SelectCamera(0);

        gameDirector.StartWave();

        if (Boombox.Instance != null)
        {
            Boombox.Instance.PlayGameplayMusic();
            Boombox.Instance.PlayGameplayAmbience();
        }

        SwitchState(GameState.Gameplay);
    }

    private void SelectCamera(int chosenIndex)
    {
        for (int i = 0; i < cameraList.Count; i++)
        {
            cameraList[i].Setup(gameDirector.player);
            cameraList[i].transform.position = cameraController.transform.position;
            cameraList[i].gameObject.SetActive(i == chosenIndex);
        }

        currentCamera = chosenIndex;
        cameraController = cameraList[chosenIndex];
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            SelectCamera((currentCamera + 1) % cameraList.Count);
        }
    }

    void MainGameplayLateLoop()
    {
        cameraController.Logic();
    }

    void PausedLoop()
    {
        // TODO pause state logic
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
            Time.timeScale = 0.3f;      // TODO better timescale management
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
