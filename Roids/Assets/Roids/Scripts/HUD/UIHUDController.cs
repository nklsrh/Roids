using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHUDController : BaseObject 
{
    public Canvas canvas;

	public UIHUDHealth health;
	public UIHUDMission mission;
	public UIHUDTimer timer;

    public UIHUDHealth playerHealthBar;

    public UIHUDNotify notifyWaveComplete;
    public UIHUDNotify notifyDamaged;
    public UIHUDNotify notifyWarning;

    private CameraController mainCamera;
	private List<UIHUDHealth> healthBarList;

	public override void Setup(){}

	public void Setup (GameDirector gameDirector, CameraController cam) 
	{
		healthBarList = new List<UIHUDHealth>();

		HealthController.onCreated += OnHealthCreated;
		HealthController.onDestroyed += OnHealthDestroyed;

        PlayableArea.onLeavingPlayArea += OnLeavingPlayArea;

        gameDirector.onWaveStarted += OnWaveStarted;
		gameDirector.onWaveComplete += OnWaveComplete;
        gameDirector.levelController.onBaseLost += OnBaseLost;		

		mainCamera = cam;

        AddHealthBar(gameDirector.player.healthController);

        playerHealthBar.Setup(canvas.GetComponent<RectTransform>(), gameDirector.player.healthController, cam, false);

        gameDirector.player.healthController.onDamage += OnPlayerDamaged;

        notifyDamaged.Disable();
        notifyWarning.Disable();
        notifyWaveComplete.Disable();
    }


    private void OnPlayerDamaged(float damage)
    {
        notifyDamaged.Popup("Damaged", "Warning", 1f);
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
        for (int i = 0; i < healthBarList.Count; i++)
        {
            if (healthBarList[i].healthController == healthController)
            {
                Destroy(healthBarList[i].gameObject);
                healthBarList.RemoveAt(i);
                break;
            }
        }
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
		mission.SetText(wave.GetObjectiveString());

		notifyWaveComplete.Popup(wave.GetObjectiveString(), "NEW MISSION", 1.5f);
	}

	void OnWaveComplete(Wave wave)
	{
		timer.Disable();
		notifyWaveComplete.Popup("Wave Complete", "", 1.5f);
	}

	void AddHealthBar(HealthController healthController)
	{
		if (healthController.isTrackedByUI)
		{
			UIHUDHealth h = Instantiate(health);
			h.Setup(canvas.GetComponent<RectTransform>(), healthController, mainCamera);

			h.transform.SetParent(health.transform.parent, false);
			
			healthBarList.Add(h);
		}
    }

    private void OnLeavingPlayArea()
    {
        notifyWarning.Popup("Return to the battle", "Out of bounds", 1.5f);
    }

    private void OnBaseLost(int basesLost, int totalBases)
    {
        notifyWarning.Popup(basesLost + "/" + totalBases + " bases lost!", "Warning", 1.5f);
    }
}
