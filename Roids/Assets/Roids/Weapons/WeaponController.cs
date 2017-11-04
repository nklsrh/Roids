using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : BaseObject
{
    public Projectile projectileTransform;

    public override void Setup()
    {
    }

    public override void Logic()
    {
    }

    public void Fire(Vector3 direction, float speed)
    {
        Projectile t = ProjectilePoolManager.Instance.AddProjectile(projectileTransform);

        t.gameObject.SetActive(true);
        t.transform.forward = direction;
        t.transform.position = transform.position;
        t.Setup();
        t.SetVelocity(direction * speed);
    }
}
