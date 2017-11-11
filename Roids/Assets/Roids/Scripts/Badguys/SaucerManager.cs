using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaucerManager : BadguyManager
{
    public int initialSaucerHealth = 10;

    public float maxInitialSaucerSpeed = 3f;
    public float minInitialSaucerSpeed = 0.2f;

    Transform[] targets;

    ProjectilePoolManager projectileManager;

    public override void Setup()
    {
        base.Setup();

        projectileManager = new ProjectilePoolManager(50);
        targets = new Transform[0];
    }

    public override void SpawnNewSet(int count, float maxSize, float baseSpeed, float skill)
    {
        for (int i = 0; i < count; i++)
        {
            Saucer saucer = GetNewBadguy() as Saucer;
            saucer.transform.position = Vector3.Scale(Random.onUnitSphere * 230, new Vector3(1, 0, 1));// new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));

            Vector3 randomDir = GetRandomDirection();
            float randomSpeed = baseSpeed * Random.Range(minInitialSaucerSpeed, maxInitialSaucerSpeed);
            float randomSize = maxSize;

            saucer.Setup(randomDir, randomSpeed, initialSaucerHealth, randomSize, skill, GetRandomTarget(), projectileManager, RemoveBadguy);
        }
    }

    public void SetTargets(Transform[] targets)
    {
        this.targets = targets;
    }

    public Transform GetRandomTarget()
    {
        if (targets.Length > 0)
        {
            return targets[Random.Range(0, targets.Length)];
        }
        return null;
    }
}
