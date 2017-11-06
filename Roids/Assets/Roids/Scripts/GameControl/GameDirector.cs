using UnityEngine;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    // __________________________________________________________________________________________EDITOR

    public PlayerController player;
    public AsteroidManager asteroidManager;
    public SaucerManager saucerManager;

    // __________________________________________________________________________________________GAME VARIABLES

    public int maxLevel = 10;

    public int asteroidsSpawnedStart = 3;
    public int asteroidsSpawnedEnd = 6;

    public float maxAsteroidSizeStart = 4f;
    public float maxAsteroidSizeEnd = 2f;

    public float initialAsteroidSpeedStart = 0.5f;
    public float initialAsteroidSpeedEnd = 3f;


    // __________________________________________________________________________________________PRIVATES (heh)

    int currentLevel = 0;

    bool isAsteroidsCleared = false;
    bool isSaucersCleared = false;


    // __________________________________________________________________________________________METHODS


    void Start()
    {
        player.Setup();
        asteroidManager.Setup(OnAsteroidsCleared);
        saucerManager.Setup(OnSaucersCleared);

        StartNewLevel(0);
    }

    void Update()
    {
        player.Logic();
        asteroidManager.Logic();
        saucerManager.Logic();
    }

    public void StartNewLevel(int newLevel)
    {
        currentLevel = newLevel;

        SpawnAsteroids();
        SpawnSaucers();
    }

    private void SpawnAsteroids()
    {
        isAsteroidsCleared = false;
        asteroidManager.SpawnNewSet((int)CalculateValueForLevel(asteroidsSpawnedStart, asteroidsSpawnedEnd),
            CalculateValueForLevel(maxAsteroidSizeStart, maxAsteroidSizeEnd),
            CalculateValueForLevel(initialAsteroidSpeedStart, initialAsteroidSpeedEnd));
    }

    private void SpawnSaucers()
    {
        isSaucersCleared = false;
        saucerManager.SpawnNewSet(currentLevel + 1, 2, 3);
    }

    private void OnAsteroidsCleared()
    {
        isAsteroidsCleared = true;
        CheckLevelComplete();
    }

    private void OnSaucersCleared()
    {
        isSaucersCleared = true;
        CheckLevelComplete();
    }

    private void CheckLevelComplete()
    {
        if (isSaucersCleared && isAsteroidsCleared)
        {
            currentLevel++;
            StartNewLevel(currentLevel);
        }
    }

    private float CalculateValueForLevel(float valueAtFirstLevel, float valueAtLastLevel)
    {
        return valueAtFirstLevel + (valueAtLastLevel - valueAtFirstLevel) * Mathf.Clamp01(currentLevel / (float)maxLevel);
    }
}
