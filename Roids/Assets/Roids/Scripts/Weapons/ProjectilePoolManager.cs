using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectilePoolManager : PoolManager
{
    public static ProjectilePoolManager Instance
    {
        get; private set;
    }



    public override void Setup()
    {
        Instance = this;

        base.Setup();
    }

    void OnDestroy()
    {
        Instance = null;
    }

    public Projectile AddProjectile(Projectile template)
    {
        Projectile newProjectile = AddObject(template) as Projectile;
        newProjectile.SetupProjectile(DisableObject);
        return newProjectile;
    }
}
