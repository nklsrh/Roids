using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaucerManager : BadguyManager
{
    public int initialSaucerHealth = 10;

    public float maxInitialSaucerSpeed = 3f;
    public float minInitialSaucerSpeed = 0.2f;

    ProjectilePoolManager projectileManager;

    public override void Setup()
    {
        base.Setup();

        projectileManager = new ProjectilePoolManager(50);
    }

    public override void SpawnNewSet(int count, float maxSize, float baseSpeed)
    {
        for (int i = 0; i < count; i++)
        {
            Saucer saucer = GetNewBadguy() as Saucer;
            saucer.transform.position = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));

            Vector3 randomDir = GetRandomDirection();
            float randomSpeed = baseSpeed * Random.Range(minInitialSaucerSpeed, maxInitialSaucerSpeed);
            float randomSize = maxSize;

            saucer.Setup(randomDir, randomSpeed, initialSaucerHealth, randomSize, projectileManager, RemoveBadguy);
        }
    }
}
