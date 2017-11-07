using UnityEngine;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    // __________________________________________________________________________________________EDITOR

    public PlayerController player;
    public AsteroidManager asteroidManager;
    public SaucerManager saucerManager;
    public LevelController levelController;

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

    // __________________________________________________________________________________________PRIVATES (heh)

    int currentLevel = 0;

    bool isAsteroidsCleared = false;
    bool isSaucersCleared = false;

    float timeSinceLevelStarted = 0;
    float timeWhenWaveFinished = 0;

    // __________________________________________________________________________________________METHODS


    void Start()
    {
        player.Setup();
        levelController.Setup(OnSpawnEnemies, OnWaveComplete);
        asteroidManager.Setup(OnAsteroidsCleared);
        saucerManager.Setup(OnSaucersCleared, levelController.BadguyCleared);

        StartWave();
    }

    void Update()
    {
        player.Logic();
        asteroidManager.Logic();
        saucerManager.Logic();
        levelController.Logic();

        timeSinceLevelStarted += Time.deltaTime;

        if (timeWhenWaveFinished > 0 && timeSinceLevelStarted > timeWhenWaveFinished + 3)
        {
            FinishWave();
        }
    }

    private void StartWave()
    {
        timeWhenWaveFinished = 0;
        levelController.StartWave();
    }

    private void FinishWave()
    {
        GoToNextWave();
    }

    public void GoToNextWave()
    {
        levelController.NextWave();

        if (levelController.HasCurrentWave())
        {
            timeSinceLevelStarted = 0;
            currentLevel++;
        }
        else
        {
            StartWave();
        }
    }

    private void OnSpawnEnemies(Wave wave)
    {
        switch (wave.enemyType)
        {
            case Wave.EnemyType.Asteroid:
                isAsteroidsCleared = false;
                asteroidManager.SpawnNewSet(wave.enemyCount, 
                    CalculateForDifficulty(maxAsteroidSizeStart, maxAsteroidSizeEnd, wave.difficulty), 
                    CalculateForDifficulty(initialAsteroidSpeedStart, initialAsteroidSpeedEnd, wave.difficulty));
                break;
            case Wave.EnemyType.Saucer:
                isSaucersCleared = false;
                saucerManager.SpawnNewSet(wave.enemyCount,
                    2f,
                    CalculateForDifficulty(saucerSpeedStart, saucerSpeedEnd, wave.difficulty));
                break;
        }
    }

    private void OnWaveComplete()
    {
        timeWhenWaveFinished = timeSinceLevelStarted;
    }

    private void OnAsteroidsCleared()
    {
        isAsteroidsCleared = true;
    }

    private void OnSaucersCleared()
    {
        isSaucersCleared = true;
    }


    private float CalculateValueForLevel(float valueAtFirstLevel, float valueAtLastLevel)
    {
        return CalculateForDifficulty(valueAtFirstLevel, valueAtLastLevel, Mathf.Clamp01(currentLevel / (float)maxLevel));
    }

    /// <summary>
    /// Given value at highest difficulty, lowest difficulty, calculate what value it would be at the required difficulty percentage level
    /// </summary>
    /// <param name="valueEasiest"></param>
    /// <param name="valueHardest"></param>
    /// <param name="difficulty"></param>
    /// <returns></returns>
    private float CalculateForDifficulty(float valueEasiest, float valueHardest, float difficulty)
    {
        return valueEasiest + (valueHardest - valueEasiest) * difficulty;
    }
}
