using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BadguyManager : BaseObject
{
    public Badguy templateObject;

    protected int poolSize = 50;
    PoolManager poolManager;

    System.Action onBadguysCleared;
    System.Action<Badguy> onBadguyKilled;

    public override void Setup()
    {
        poolManager = new PoolManager(poolSize);

        templateObject.gameObject.SetActive(false);
    }

    public virtual void Setup (System.Action onBadguysCleared, System.Action<Badguy> onBadguyKilled)
    {
        this.onBadguysCleared = onBadguysCleared;
        this.onBadguyKilled = onBadguyKilled;

        Setup();
    }

    public virtual void SpawnNewSet(int count, float maxSize, float baseSpeed)
    {
        // stub
    }

    public override void Logic()
    {
        poolManager.Logic();
    }

    protected virtual void RemoveBadguy(Badguy badguy)
    {
        poolManager.DisableObject(badguy);
        badguy.Die();

        if (onBadguyKilled != null)
        {
            onBadguyKilled.Invoke(badguy);
        }

        CheckAllBadguysCleared();
    }

    protected virtual void CheckAllBadguysCleared()
    {
        if (poolManager.Count == 0)
        {
            if (onBadguysCleared != null)
            {
                onBadguysCleared.Invoke();
            }
        }
    }

    protected virtual Badguy GetNewBadguy()
    {
        return poolManager.AddObject(templateObject) as Badguy;
    }
}
