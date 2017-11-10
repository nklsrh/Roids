using UnityEngine;
using System.Collections;

public class MainGameplayController : MonoBehaviour {
	public UIHUDController uiController;
	public GameDirector gameDirector;
	public CameraController cameraController;

	void Start () 
	{
		gameDirector.Setup();
		uiController.Setup(gameDirector, cameraController);


		cameraController.Setup(gameDirector.player);


        gameDirector.StartWave();
	}
	
	// Update is called once per frame
	void Update () 
	{
		gameDirector.Logic();
		uiController.Logic();
	}

	void LateUpdate()
	{
		cameraController.Logic();	
	}
}
