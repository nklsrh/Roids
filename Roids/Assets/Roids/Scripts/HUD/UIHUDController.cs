using UnityEngine;
using System.Collections;

public class UIHUDController : BaseObject 
{
	public UIHUDHealth health;
	public UIHUDMission mission;
	public UIHUDTimer timer;

	public UIHUDNotifyWaveComplete notifyWaveComplete;

	public override void Setup(){}

	public void Setup (GameDirector gameDirector) 
	{
		gameDirector.onWaveStarted += OnWaveStarted;
		gameDirector.onWaveComplete += OnWaveComplete;
	}

	public override void Logic()
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
}
