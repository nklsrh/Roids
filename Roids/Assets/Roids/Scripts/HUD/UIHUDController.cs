using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHUDController : BaseObject 
{
	public UIHUDHealth health;
	public UIHUDMission mission;
	public UIHUDTimer timer;

	public UIHUDNotifyWaveComplete notifyWaveComplete;

	private CameraController mainCamera;
	private List<UIHUDHealth> healthBarList;

	public override void Setup(){}

	public void Setup (GameDirector gameDirector, CameraController cam) 
	{
		healthBarList = new List<UIHUDHealth>();

		HealthController.onCreated += OnHealthCreated;
		HealthController.onDestroyed += OnHealthDestroyed;

		gameDirector.onWaveStarted += OnWaveStarted;
		gameDirector.onWaveComplete += OnWaveComplete;		

		mainCamera = cam;
	}

	public override void Logic()
	{

	}

	void OnHealthCreated(HealthController healthController)
	{
		AddHealthBar(healthController);
	}
	void OnHealthDestroyed(HealthController healthController)
	{

	}
	
	void OnWaveStarted(Wave wave)
	{
		timer.Disable();
		if (wave.IsTimeBased)
		{
			timer.Enable();
			timer.StartTimer(wave.duration);
		}

		mission.Enable();
		mission.SetText(wave.objective.ToString());

		notifyWaveComplete.Popup("NEW MISSION: " + wave.objective.ToString(), 1.5f);
	}

	void OnWaveComplete(Wave wave)
	{
		timer.Disable();
		notifyWaveComplete.Popup("Wave Complete", 1.5f);
	}


	void AddHealthBar(HealthController healthController)
	{
		if (healthController.isTrackedByUI)
		{
			UIHUDHealth h = Instantiate(health);
			h.Setup(healthController, mainCamera);

			h.transform.SetParent(health.transform.parent, false);
			
			healthBarList.Add(h);
		}
	}
}
