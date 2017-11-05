using UnityEngine;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    public PlayerController player;
    public AsteroidManager asteroidManager;

    ProjectilePoolManager projectileManager;

    int currentLevel = 0;

    public int maxLevel = 10;

    public int asteroidsSpawnedStart = 3;
    public int asteroidsSpawnedEnd = 6;

    public float maxAsteroidSizeStart = 4f;
    public float maxAsteroidSizeEnd = 2f;

    public float initialAsteroidSpeedStart = 0.5f;
    public float initialAsteroidSpeedEnd = 3f;

    void Start()
    {
        player.Setup();
        asteroidManager.Setup();
        asteroidManager.SetupAsteroidManager(OnLevelCleared);

        projectileManager = new ProjectilePoolManager();
        projectileManager.Setup();
        projectileManager.SetPoolSize(30);

        StartNewLevel(0);
    }

    void Update()
    {
        player.Logic();
        projectileManager.Logic();
        asteroidManager.Logic();
    }

    public void StartNewLevel(int newLevel)
    {
        currentLevel = newLevel;
        asteroidManager.SpawnNewSet((int)CalculateValueForLevel(asteroidsSpawnedStart, asteroidsSpawnedEnd), 
            CalculateValueForLevel(maxAsteroidSizeStart, maxAsteroidSizeEnd),
            CalculateValueForLevel(initialAsteroidSpeedStart, initialAsteroidSpeedEnd));
    }

    private void OnLevelCleared()
    {
        currentLevel++;
        StartNewLevel(currentLevel);
    }

    private float CalculateValueForLevel(float valueAtFirstLevel, float valueAtLastLevel)
    {
        return valueAtFirstLevel + (valueAtLastLevel - valueAtFirstLevel) * Mathf.Clamp01(currentLevel / (float)maxLevel);
    }
}
