using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : BaseObject
{
    public ProtectBase[] protectBases;

    public bool IsEndless
    {
        get
        {
            return levelData.isEndless;
        }
    }

    public Wave Wave
    {
        get
        {
            if (currentWave < waves.Length)
            {
                wave = waves[currentWave];
            }
            return wave;
        }
    }

    public ProtectBase[] ActiveProtectBases
    {
        get; private set;
    }


    LevelData levelData;
    Wave[] waves;
    Wave wave;
    int currentWave = 0;
    float currentWaveTime = 0;
    int enemiesKilled = 0;
    bool isWaveActive = false;
    int basesLost = 0;

    int endlessLevelsComplete = 0;
    float overrideEndlessDifficulty = 0f;

    System.Action<Wave> actionWaveStarted;
    System.Action actionWaveComplete;
    System.Action actionWaveFailed;

    public override void Setup() { }

    public void Setup(LevelData levelData, System.Action<Wave> actionWaveStarted, System.Action actionWaveComplete, System.Action actionWaveFailed)
    {
        this.levelData = levelData;
        waves = levelData.waves;
        endlessLevelsComplete = 0;

        this.actionWaveStarted = actionWaveStarted;
        this.actionWaveComplete = actionWaveComplete;
        this.actionWaveFailed = actionWaveFailed;

        Setup();
    }

    public bool CanStartWave()
    {
        return Wave != null;
    }

    public bool HasCurrentWave()
    {
        return isWaveActive && Wave != null;
    }

    private bool IsCurrentWaveTimeBased()
    {
        return Wave.IsTimeBased;
    }

    public bool HasWaveStarted()
    {
        return currentWaveTime > 0;
    }

    public void StartWave()
    {
        isWaveActive = true;
        currentWaveTime = 0;
        enemiesKilled = 0;
        basesLost = 0;

        SetupProtectBases();

        CalculateEndlessDifficulty();

        Wave.SetOverrideDifficulty(overrideEndlessDifficulty);

        if (actionWaveStarted != null)
        {
            actionWaveStarted.Invoke(Wave);
        }
    }

    private void CalculateEndlessDifficulty()
    {
        overrideEndlessDifficulty = Mathf.Min(endlessLevelsComplete * 0.5f + (currentWave * 0.15f), 6.0f);
    }

    private void FinishWave()
    {
        isWaveActive = false;
        if (actionWaveComplete != null)
        {
            actionWaveComplete.Invoke();
        }
    }

    public void NextWave()
    {
        currentWave++;
        wave = null;
    }

    public void RestartWaves()
    {
        currentWave = 0;
        endlessLevelsComplete++;
    }

    public void BadguyCleared(Badguy badguy)
    {
        if (badguy.EnemyType == Wave.enemyType)
        {
            enemiesKilled++;
        }
    }

    public override void Logic()
    {
        if (!HasCurrentWave())
        {
            return;
        }

        if (IsWaveComplete())
        {
            FinishWave();
        }
        else if (IsWaveFailed())
        {
            Debug.Log("WAVE FAILED!");
            if (actionWaveFailed != null)
            {
                actionWaveFailed.Invoke();
            }
        }

        currentWaveTime += Time.deltaTime;
    }

    private void SetupProtectBases()
    {
        int basesRequired = 0;
        if (Wave.objective == Wave.ObjectiveType.Protect)
        {
            basesRequired = Wave.objectiveRequiredValue;
        }

        List<int> basesRemaining = new List<int>();
        for (int i = 0; i < protectBases.Length; i++)
        {
            basesRemaining.Add(i);
        }

        int[] chosenBases = new int[basesRequired];
        for (int i = 0; i < basesRequired; i++)
        {
            int index = Random.Range(0, basesRemaining.Count);
            chosenBases[i] = basesRemaining[index];
            basesRemaining.RemoveAt(index);
        }

        // first disable all the bases
        for (int i = 0; i < protectBases.Length; i++)
        {
            protectBases[i].onDeath -= OnBaseLost;
            protectBases[i].Disable();
        }

        ActiveProtectBases = new ProtectBase[chosenBases.Length];

        // then bring some of them online as required
        for (int i = 0; i < chosenBases.Length; i++)
        {
            protectBases[chosenBases[i]].Setup();
            protectBases[chosenBases[i]].onDeath += OnBaseLost;
            ActiveProtectBases[i] = protectBases[chosenBases[i]];
        }
    }

    private void OnBaseLost(ProtectBase protectBase)
    {
        Debug.Log("BASE LOST!");
        basesLost++;
    }

    private bool IsWaveComplete()
    {
        switch (Wave.objective)
        {
            case Wave.ObjectiveType.KillAll:
                return (enemiesKilled >= Wave.enemyCount);
            case Wave.ObjectiveType.Survive:
                return (currentWaveTime >= Wave.duration);
            case Wave.ObjectiveType.Protect:
                return (currentWaveTime >= Wave.duration && basesLost < Wave.objectiveRequiredValue);
        }
        return false;
    }

    private bool IsWaveFailed()
    {
        switch (Wave.objective)
        {
            case Wave.ObjectiveType.Protect:
                return (basesLost >= Wave.objectiveRequiredValue);
        }
        return false;
    }
}
