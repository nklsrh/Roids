using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : BaseObject
{
    public Projectile projectileTransform;

    ProjectilePoolManager pool;

    public override void Setup()
    {
        // stub -- to be maybe used for reloading and other weapon system innovations

        projectileTransform.gameObject.SetActive(false);
    }

    public void Setup(ProjectilePoolManager pool)
    {
        this.pool = pool;

        Setup();
    }

    public override void Logic()
    {
        pool.Logic();
    }

    public void Fire(Vector3 direction, float speed)
    {
        Projectile t = pool.AddProjectile(projectileTransform);

        t.transform.forward = direction;
        t.transform.position = transform.position;
        t.Setup();
        t.SetVelocity(direction * speed);
    }
}
