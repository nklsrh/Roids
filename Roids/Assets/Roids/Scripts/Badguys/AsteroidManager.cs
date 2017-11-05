using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidManager : BaseObject
{
    public Asteroid asteroidTemplate;

    PoolManager asteroidPoolManager;

    public override void Setup()
    {
        asteroidPoolManager = new PoolManager();
        asteroidPoolManager.Setup();
    }

    public void SpawnNewSet(int count, float size)
    {
        for (int i = 0; i < count; i++)
        {
            Asteroid a = asteroidPoolManager.AddObject(asteroidTemplate) as Asteroid;
            a.transform.position = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
            a.transform.localScale = Vector3.one * size;
            a.SetupAsteroid(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized, Random.Range(0.1f, 1f), asteroidPoolManager.DisableObject);
        }
    }

    public override void Logic()
    {
        asteroidPoolManager.Logic();
        if (asteroidPoolManager.Count == 0)
        {
            SpawnNewSet(3, 3);
        }
    }
}
