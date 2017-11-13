using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaucerManager : BadguyManager
{
    public int initialSaucerHealthMin = 10;
    public int initialSaucerHealthMax = 200;

    public float maxInitialSaucerSpeed = 3f;
    public float minInitialSaucerSpeed = 0.2f;

    List<HealthController> targets;

    ProjectilePoolManager projectileManager;

    public override void Setup()
    {
        base.Setup();

        projectileManager = new ProjectilePoolManager(50);
        targets = new List<HealthController>();
    }

    public override void SpawnNewSet(int count, float maxSize, float baseSpeed, float skill)
    {
        for (int i = 0; i < count; i++)
        {
            Saucer saucer = GetNewBadguy() as Saucer;
            
            // Spawn it outside the play area so the trigger then moves it to the edge of the area
            saucer.transform.position = Vector3.Scale(Random.onUnitSphere * 1000, new Vector3(1, 0, 1));

            Vector3 randomDir = GetRandomDirection();
            float randomSpeed = baseSpeed * Random.Range(minInitialSaucerSpeed, maxInitialSaucerSpeed);

            saucer.Setup(randomDir, randomSpeed, GameDirector.CalculateForDifficulty(initialSaucerHealthMin, initialSaucerHealthMax, skill), maxSize, skill, targets, projectileManager, RemoveBadguy);
        }
    }

    public void SetTargets(List<HealthController> targets)
    {
        this.targets = targets;
    }
}
