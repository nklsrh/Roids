using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ProjectilePoolManager : PoolManager
{
    public ProjectilePoolManager(int poolSize)
    {
        Setup(poolSize);
    }

    public Projectile AddProjectile(Projectile template)
    {
        Projectile newProjectile = AddObject(template) as Projectile;
        newProjectile.SetupProjectile(DisableObject);
        return newProjectile;
    }
}
