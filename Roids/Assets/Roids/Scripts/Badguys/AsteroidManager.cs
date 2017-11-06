using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidManager : BaseObject
{
    public Asteroid asteroidTemplate;

    public float smallerAsteroidSpeedMultiplier = 2.5f;
    public int initialAsteroidHealth = 3;

    public float maxInitialAsteroidSpeed = 3f;
    public float minInitialAsteroidSpeed = 0.2f;

    public float[] asteroidSizesByHealth;

    PoolManager asteroidPoolManager;

    System.Action onAsteroidsCleared;

    public override void Setup()
    {
        asteroidPoolManager = new PoolManager(50);

        asteroidTemplate.gameObject.SetActive(false);
    }

    public void Setup(System.Action onAsteroidsCleared)
    {
        this.onAsteroidsCleared = onAsteroidsCleared;

        Setup();
    }

    public void SpawnNewSet(int count, float maxSize, float baseSpeed)
    {
        for (int i = 0; i < count; i++)
        {
            Asteroid a = GetNewAsteroid();
            a.transform.position = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));

            Vector3 randomDir = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized;
            float randomSpeed = baseSpeed * Random.Range(minInitialAsteroidSpeed, maxInitialAsteroidSpeed);
            float randomSize = Random.Range(1.0f, maxSize);

            a.Setup(randomDir, randomSpeed, initialAsteroidHealth, randomSize, AsteroidHit);
        }
        
        CheckAllAsteroidsCleared();
    }

    public override void Logic()
    {
        asteroidPoolManager.Logic();
    }

    public void AsteroidHit(Asteroid asteroid)
    {
        if (asteroid.Lives > 1)
        {
            for (int i = 0; i < asteroid.Lives; i++)
            {
                Asteroid newAsteroid = GetNewAsteroid();

                // add a little magic to make the new asteroids disperse away from the collision
                // so you have to chase them down / they don't hurtle toward you
                Vector3 randomDir = (asteroid.HitFromDirection + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1))).normalized;
                float newSpeed = asteroid.Speed * smallerAsteroidSpeedMultiplier;
                float newSize = asteroidSizesByHealth[asteroid.Lives - 1]; //asteroid.transform.localScale.x * (asteroid.Lives - 1) / asteroid.Lives;

                newAsteroid.Setup(randomDir, newSpeed, asteroid.Lives - 1, newSize, AsteroidHit);

                newAsteroid.transform.position = asteroid.transform.position + newAsteroid.Direction * newAsteroid.Speed * 0.5f;
            }
        }

        asteroidPoolManager.DisableObject(asteroid);
        asteroid.Die();

        CheckAllAsteroidsCleared();
    }

    void CheckAllAsteroidsCleared()
    {
        if (asteroidPoolManager.Count == 0)
        {
            if (onAsteroidsCleared != null)
            {
                onAsteroidsCleared.Invoke();
            }
        }
    }

    public Asteroid GetNewAsteroid()
    {
        return asteroidPoolManager.AddObject(asteroidTemplate) as Asteroid;
    }
}
