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

    public int Score
    {
        get; private set;
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

    public System.Action<Wave> onWaveStarted;
    public System.Action onWaveComplete;
    public System.Action onWaveFailed;
    public System.Action<int, int> onBaseLost;
    public System.Action<int, string, int> onScoreAdded;

    const int SCORE_ASTEROIDHIT_1 = 50;
    const int SCORE_ASTEROIDHIT_2 = 30;
    const int SCORE_ASTEROIDHIT_3 = 10;

    const int SCORE_WAVE_KILLENEMY = 100;
    const int SCORE_WAVE_PROTECTEDBASE = 100;
    const int SCORE_WAVE_COMPLETE = 100;

    const int SCORE_LEVEL_COMPLETE = 1000;


    public override void Setup() { }

    public void Setup(LevelData levelData)
    {
        this.levelData = levelData;
        waves = levelData.waves;
        endlessLevelsComplete = 0;
        currentWaveTime = 0;
        enemiesKilled = 0;
        basesLost = 0;

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

        if (onWaveStarted != null)
        {
            onWaveStarted.Invoke(Wave);
        }
    }

    private void CalculateEndlessDifficulty()
    {
        overrideEndlessDifficulty = Mathf.Min(endlessLevelsComplete * 0.5f + (currentWave * 0.15f), 6.0f);
    }

    private void FinishWave()
    {
        isWaveActive = false;
        if (onWaveComplete != null)
        {
            onWaveComplete.Invoke();
        }

        AddScore(SCORE_WAVE_COMPLETE, "Completed Wave");
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

        AddScore(SCORE_LEVEL_COMPLETE, "Leveled Up!");
    }

    public void BadguyCleared(Badguy badguy)
    {
        if (badguy.EnemyType == Wave.enemyType)
        {
            enemiesKilled++;

            AddScore(SCORE_WAVE_KILLENEMY, "Killed Enemy");
        }

        if (badguy.EnemyType == Wave.EnemyType.Asteroid)
        {
            Asteroid ab = badguy as Asteroid;
            if (ab.ChunksRemaining == 3)
            {
                AddScore(SCORE_ASTEROIDHIT_3, "Asteroid destroyed");
            }
            else if (ab.ChunksRemaining == 2)
            {
                AddScore(SCORE_ASTEROIDHIT_2, "Asteroid crushed");
            }
            else if (ab.ChunksRemaining == 1)
            {
                AddScore(SCORE_ASTEROIDHIT_1, "Asteroid exploded");
            }
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
            if (onWaveFailed != null)
            {
                onWaveFailed.Invoke();
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
        basesLost++;
        if (onBaseLost != null)
        {
            onBaseLost.Invoke(basesLost, Wave.objectiveRequiredValue);
        }
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
                bool result = (currentWaveTime >= Wave.duration && basesLost < Wave.objectiveRequiredValue);
                if (result)
                {
                    int basesProtected = (Wave.objectiveRequiredValue - basesLost);
                    AddScore(SCORE_WAVE_PROTECTEDBASE * basesProtected, "Protected " + basesProtected + " bases");
                    return true;
                }
                break;
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

    private void AddScore(int score, string reason)
    {
        Score += score; 
        if (onScoreAdded != null)
        {
            onScoreAdded.Invoke(score, reason, Score);
        }
    }

}
