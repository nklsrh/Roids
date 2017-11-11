using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDirector : BaseObject
{
    // __________________________________________________________________________________________EDITOR

    public PlayerController player;
    public AsteroidManager asteroidManager;
    public SaucerManager saucerManager;
    public ExplosionManager explosionManager;
    public LevelController levelController;
    public string levelName = "Level_1_Endless";

    // __________________________________________________________________________________________GAME VARIABLES

    public int maxLevel = 10;

    public int asteroidsSpawnedStart = 3;
    public int asteroidsSpawnedEnd = 6;

    public float maxAsteroidSizeStart = 4f;
    public float maxAsteroidSizeEnd = 2f;

    public float initialAsteroidSpeedStart = 0.5f;
    public float initialAsteroidSpeedEnd = 3f;

    public float saucerSpeedStart = 1f;
    public float saucerSpeedEnd = 3f;

    // __________________________________________________________________________________________EVENTS

    public System.Action<Wave> onWaveStarted;
    public System.Action<Wave> onWaveComplete;
    public System.Action<Wave> onWaveFailed;
    public System.Action<int> onLevelComplete;
    public System.Action<Wave.EnemyType> onSpawnEnemies;
    public System.Action<Wave.EnemyType> onBadguyKilled;
    public System.Action<int, int> onBaseKilled;

    // __________________________________________________________________________________________PRIVATES (heh)

    LevelData levelData;

    int currentLevel = 0;

    float timeSinceLevelStarted = 0;
    float timeWhenWaveFinished = 0;

    const float WAIT_BETWEEN_WAVES = 3.0f;

    // __________________________________________________________________________________________METHODS


    public override void Setup()
    {
        player.Setup();
        player.healthController.onDeath += OnWaveFailed;

        LoadLevelData();

        levelController.Setup(levelData);
        levelController.onWaveStarted += OnWaveStarted;
        levelController.onWaveComplete += OnWaveComplete;
        levelController.onWaveFailed += OnWaveFailed;

        asteroidManager.Setup(null, OnBadguyKilled);
        saucerManager.Setup(null, OnBadguyKilled);
        explosionManager.Setup();

        ExplosionManagerStatic = explosionManager;
    }

    private void LoadLevelData()
    {
        levelData = Instantiate(Resources.Load<LevelData>(System.IO.Path.Combine("LevelData", levelName)));
    }

    public override void Logic()
    {
        base.Logic();

        player.Logic();
        asteroidManager.Logic();
        saucerManager.Logic();
        levelController.Logic();
        explosionManager.Logic();

        timeSinceLevelStarted += Time.deltaTime;

        if (timeWhenWaveFinished > 0 && timeSinceLevelStarted > timeWhenWaveFinished + WAIT_BETWEEN_WAVES)
        {
            FinishWave();
        }
    }

    public void StartWave()
    {
        levelController.StartWave();

        if (onWaveStarted != null)
        {
            onWaveStarted.Invoke(levelController.Wave);
        }
    }

    private void FinishWave()
    {
        timeWhenWaveFinished = 0;
        GoToNextWave();
    }

    public void GoToNextWave()
    {
        levelController.NextWave();

        if (!levelController.CanStartWave())
        {
            LevelComplete();
        }

        StartWave();
    }

    void LevelComplete()
    {
        if (onLevelComplete != null)
        {
            onLevelComplete.Invoke(currentLevel);
        }

        timeSinceLevelStarted = 0;
        currentLevel++;

        if (levelController.IsEndless)
        {
            levelController.RestartWaves();
        }
    }

    private void SpawnEnemies(Wave wave)
    {
        if (onSpawnEnemies != null)
        {
            onSpawnEnemies.Invoke(wave.enemyType);
        }

        switch (wave.enemyType)
        {
            case Wave.EnemyType.Asteroid:
                asteroidManager.SpawnNewSet(wave.enemyCount, 
                    CalculateForDifficulty(maxAsteroidSizeStart, maxAsteroidSizeEnd, wave.Difficulty), 
                    CalculateForDifficulty(initialAsteroidSpeedStart, initialAsteroidSpeedEnd, wave.Difficulty),
                    wave.Difficulty);
                break;
            case Wave.EnemyType.Droid:
                if (wave.objective == Wave.ObjectiveType.Protect)
                {
                    saucerManager.SetTargets(SelectProtectionTargets());
                }
                else
                {
                    saucerManager.SetTargets(new List<HealthController> { player.healthController, player.healthController, null });    // 33% chance of shooting randomly
                }
                saucerManager.SpawnNewSet(wave.enemyCount,
                    1f,
                    CalculateForDifficulty(saucerSpeedStart, saucerSpeedEnd, wave.Difficulty),
                    wave.Difficulty);
                break;
        }
    }

    private void OnBadguyKilled(Badguy badguy)
    {
        if (onBadguyKilled != null)
        {
            onBadguyKilled.Invoke(badguy.EnemyType);
        }

        levelController.BadguyCleared(badguy);
    }

    private void OnWaveStarted(Wave wave)
    {
        SpawnEnemies(wave);

        player.healthController.SetInvincible(false);
    }

    private void OnWaveComplete()
    {
        timeWhenWaveFinished = timeSinceLevelStarted;
        
        if (onWaveComplete != null)
        {
            onWaveComplete.Invoke(levelController.Wave);
        }

        player.healthController.SetInvincible(true);
    }

    private void OnWaveFailed()
    {
        if (onWaveFailed != null)
        {
            onWaveFailed.Invoke(levelController.Wave);
        }

        player.healthController.SetInvincible(true);
    }

    private float CalculateValueForLevel(float valueAtFirstLevel, float valueAtLastLevel)
    {
        return CalculateForDifficulty(valueAtFirstLevel, valueAtLastLevel, Mathf.Clamp01(currentLevel / (float)maxLevel));
    }


    private List<HealthController> SelectProtectionTargets()
    {
        List<HealthController> targets = new List<HealthController>();
        for (int i = 0; i < levelController.ActiveProtectBases.Length; i++)
        {
            targets.Add(levelController.ActiveProtectBases[i].healthController);
        }
        targets.Add(player.healthController);
        return targets;
    }


    private static ExplosionManager ExplosionManagerStatic;
    public static ExplosionObject Explosion(Vector3 position, float scale)
    {
        if (ExplosionManagerStatic != null)
        {
            return ExplosionManagerStatic.CreateExplosion(position, scale);
        }
        return null;
    }

    /// <summary>
    /// Given value at highest difficulty, lowest difficulty, calculate what value it would be at the required difficulty percentage level
    /// </summary>
    /// <param name="valueEasiest"></param>
    /// <param name="valueHardest"></param>
    /// <param name="difficulty"></param>
    /// <returns></returns>
    public static float CalculateForDifficulty(float valueEasiest, float valueHardest, float difficulty)
    {
        return valueEasiest + (valueHardest - valueEasiest) * difficulty;
    }
}
