using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidManager : BadguyManager
{
    public float smallerAsteroidSpeedMultiplier = 2.5f;
    public int initialAsteroidHealth = 3;

    public float maxInitialAsteroidSpeed = 3f;
    public float minInitialAsteroidSpeed = 0.2f;

    public float[] asteroidSizesByHealth;

    public override void SpawnNewSet(int count, float maxSize, float baseSpeed)
    {
        for (int i = 0; i < count; i++)
        {
            Asteroid a = GetNewBadguy() as Asteroid;
            a.transform.position = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));

            Vector3 randomDir = GetRandomDirection();
            float randomSpeed = baseSpeed * Random.Range(minInitialAsteroidSpeed, maxInitialAsteroidSpeed);
            float randomSize = Random.Range(1.0f, maxSize);

            a.Setup(randomDir, randomSpeed, initialAsteroidHealth, randomSize, AsteroidHit);
        }
    }

    public void AsteroidHit(Asteroid asteroid)
    {
        if (asteroid.ChunksRemaining > 1)
        {
            for (int i = 0; i < asteroid.ChunksRemaining; i++)
            {
                Asteroid newAsteroid = GetNewBadguy() as Asteroid;

                // add a little magic to make the new asteroids disperse away from the collision
                // so you have to chase them down / they don't hurtle toward you
                Vector3 randomDir = (asteroid.HitFromDirection + GetRandomDirection()).normalized;
                float newSpeed = asteroid.Speed * smallerAsteroidSpeedMultiplier * Random.Range(0.6f, 2f);
                float newSize = asteroidSizesByHealth[asteroid.ChunksRemaining - 1]; //asteroid.transform.localScale.x * (asteroid.Lives - 1) / asteroid.Lives;

                newAsteroid.Setup(randomDir, newSpeed, asteroid.ChunksRemaining - 1, newSize, AsteroidHit);

                newAsteroid.transform.position = asteroid.transform.position + newAsteroid.Direction * newAsteroid.Speed * 0.5f;
            }
        }

        RemoveBadguy(asteroid);
    }
}
