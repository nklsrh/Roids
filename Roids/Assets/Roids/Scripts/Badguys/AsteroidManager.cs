﻿using UnityEngine;

public class AsteroidManager : BadguyManager
{
    public float smallerAsteroidSpeedMultiplier = 2.5f;
    public int initialAsteroidChunks = 3;

    public float maxInitialAsteroidSpeed = 3f;
    public float minInitialAsteroidSpeed = 0.2f;

    public float[] asteroidSizesByHealth;

    public override void SpawnNewSet(int count, float maxSize, float baseSpeed, float skill)
    {
        for (int i = 0; i < count; i++)
        {
            Asteroid a = GetNewBadguy() as Asteroid;

            // Spawn it outside the play area so the trigger then moves it to the edge of the area
            a.transform.position = Vector3.Scale(Random.onUnitSphere * 1000, new Vector3(1, 0, 1));

            Vector3 randomDir = GetRandomDirection();
            float randomSpeed = baseSpeed * Random.Range(minInitialAsteroidSpeed, maxInitialAsteroidSpeed);
            float randomSize = Random.Range(1.0f, maxSize);

            a.Setup(randomDir, randomSpeed, 1, randomSize, initialAsteroidChunks, AsteroidHit);
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
                float newSize = asteroidSizesByHealth[asteroid.ChunksRemaining - 1];

                newAsteroid.Setup(randomDir, newSpeed, asteroid.healthController.HealthMax, newSize, asteroid.ChunksRemaining - 1, AsteroidHit);

                newAsteroid.transform.position = asteroid.transform.position + newAsteroid.Direction * newAsteroid.Speed * 0.5f;
            }
        }

        RemoveBadguy(asteroid);
    }
}
