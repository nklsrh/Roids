using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaucerManager : BadguyManager
{
    public int initialSaucerHealth = 1;

    public float maxInitialSaucerSpeed = 3f;
    public float minInitialSaucerSpeed = 0.2f;

    public override void SpawnNewSet(int count, float maxSize, float baseSpeed)
    {
        for (int i = 0; i < count; i++)
        {
            Saucer a = GetNewBadguy() as Saucer;
            a.transform.position = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));

            Vector3 randomDir = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized;
            float randomSpeed = baseSpeed * Random.Range(minInitialSaucerSpeed, maxInitialSaucerSpeed);
            float randomSize = Random.Range(1.0f, maxSize);

            a.Setup(randomDir, randomSpeed, initialSaucerHealth, randomSize, SaucerHit);
        }
    }

    public void SaucerHit(Saucer saucer)
    {
        if (saucer.Lives <= 0)
        {
            RemoveBadguy(saucer);
        }
    }
}
