using UnityEngine;
using System.Collections;

public class ExplosionManager : BaseObject
{
    public ExplosionObject templateObject;

    PoolManager poolManager;

    const int poolSize = 15;

    public override void Setup()
    {
        poolManager = new PoolManager(poolSize);

        templateObject.gameObject.SetActive(false);
    }

    public override void Logic()
    {
    }

    public ExplosionObject CreateExplosion(Vector3 position, float scale)
    {
        ExplosionObject explosion = poolManager.AddObject(templateObject) as ExplosionObject;

        explosion.transform.position = position;
        explosion.transform.localScale = Vector3.one * scale;

        for (int i = 0; i < explosion.transform.childCount; i++)
        {
            explosion.transform.GetChild(i).localScale = Vector3.one * scale;
        }

        explosion.Setup();
        return explosion;
    }
}
