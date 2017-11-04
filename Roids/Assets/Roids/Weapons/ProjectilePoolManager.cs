using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectilePoolManager : BaseObject
{
    List<Projectile> pooledProjectiles;
    int lastUsedProjectileFromPool = -1;

    const int MAX_PROJECTILES_IN_POOL = 20;

    public static ProjectilePoolManager Instance
    {
        get; private set;
    }

    void Awake()
    {
        Instance = this;
        Setup();
    }

    void OnDestroy()
    {
        Instance = null;
    }

    public override void Setup()
    {
        pooledProjectiles = new List<Projectile>();
    }

    public Projectile AddProjectile(Projectile template)
    {
        Projectile p;
        if (pooledProjectiles.Count < MAX_PROJECTILES_IN_POOL)
        {
            p = Instantiate(template);
            pooledProjectiles.Add(p);
        }
        else if (lastUsedProjectileFromPool >= MAX_PROJECTILES_IN_POOL - 1)
        {
            lastUsedProjectileFromPool = -1;
        }
        p = pooledProjectiles[++lastUsedProjectileFromPool];
        return p;
    }

    public override void Logic()
    {
        for (int i = 0; i < pooledProjectiles.Count; i++)
        {
            pooledProjectiles[i].Logic();
        }
    }

    public void ClearAll()
    {
        for (int i = 0; i < pooledProjectiles.Count; i++)
        {
            Destroy(pooledProjectiles[i]);
        }
        pooledProjectiles.Clear();
    }
}
