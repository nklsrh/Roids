using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Wave
{
    public enum ObjectiveType
    {
        None,
        KillAll,
        Protect,
        Rebuild,
        Survive
    }

    public enum EnemyType
    {
        Asteroid,
        Saucer,
        Juggernaut,
        Stomper
    }

    public ObjectiveType objective;
    public EnemyType enemyType;
    public int enemyCount = 1;
    public float difficulty = 0.0f;

    public float duration = -1;

    public bool IsTimeBased
    {
        get
        {
            return duration > 0;
        }
    }
}

public class LevelController : BaseObject
{
    public List<Wave> waves;

    public Wave Wave
    {
        get
        {
            if (wave == null || currentWave < waves.Count)
            {
                wave = waves[currentWave];
            }
            return wave;
        }
    }

    Wave wave;
    int currentWave = 0;
    float currentWaveTime = 0;
    int enemiesKilled = 0;
    bool isWaveActive = false;

    System.Action<Wave> actionSpawnEnemy;
    System.Action actionWaveComplete;

    public override void Setup() { }

    public void Setup(System.Action<Wave> actionSpawnEnemy, System.Action actionWaveComplete)
    {
        this.actionSpawnEnemy = actionSpawnEnemy;
        this.actionWaveComplete = actionWaveComplete;

        Setup();
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

        if (actionSpawnEnemy != null)
        {
            actionSpawnEnemy.Invoke(Wave);
        }
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

        currentWaveTime += Time.deltaTime;
    }

    private bool IsWaveComplete()
    {
        switch (Wave.objective)
        {
            case Wave.ObjectiveType.None:
                return (Wave.duration < currentWaveTime);
            case Wave.ObjectiveType.KillAll:
                return (enemiesKilled >= Wave.enemyCount);
            case Wave.ObjectiveType.Survive:
                return (currentWaveTime >= Wave.duration);
        }
        return false;
    }
}
