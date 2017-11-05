using UnityEngine;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    public PlayerController player;
    public AsteroidManager asteroidManager;
    public SaucerManager saucerManager;

    ProjectilePoolManager projectileManager;

    int currentLevel = 0;

    public int maxLevel = 10;

    public int asteroidsSpawnedStart = 3;
    public int asteroidsSpawnedEnd = 6;

    public float maxAsteroidSizeStart = 4f;
    public float maxAsteroidSizeEnd = 2f;

    public float initialAsteroidSpeedStart = 0.5f;
    public float initialAsteroidSpeedEnd = 3f;

    bool isAsteroidsCleared = false;
    bool isSaucersCleared = false;

    void Start()
    {
        player.Setup();

        asteroidManager.Setup();
        asteroidManager.SetupAsteroidManager(OnAsteroidsCleared);

        projectileManager = new ProjectilePoolManager();
        projectileManager.Setup();
        projectileManager.SetPoolSize(30);

        saucerManager.SetupManager(OnSaucersCleared);

        StartNewLevel(0);
    }

    void Update()
    {
        player.Logic();
        projectileManager.Logic();
        asteroidManager.Logic();
        saucerManager.Logic();
    }

    public void StartNewLevel(int newLevel)
    {
        currentLevel = newLevel;
        isAsteroidsCleared = false;
        isSaucersCleared = false;

        asteroidManager.SpawnNewSet((int)CalculateValueForLevel(asteroidsSpawnedStart, asteroidsSpawnedEnd), 
            CalculateValueForLevel(maxAsteroidSizeStart, maxAsteroidSizeEnd),
            CalculateValueForLevel(initialAsteroidSpeedStart, initialAsteroidSpeedEnd));

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
