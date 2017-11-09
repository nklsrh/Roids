using UnityEngine;
using System.Collections;

public class MainGameplayController : MonoBehaviour {
	public UIHUDController uiController;
	public GameDirector gameDirector;

	void Start () 
	{
		gameDirector.Setup();
		uiController.Setup(gameDirector);
		

        gameDirector.StartWave();
	}
	
	// Update is called once per frame
	void Update () 
	{
		gameDirector.Logic();
		uiController.Logic();
	}
}
