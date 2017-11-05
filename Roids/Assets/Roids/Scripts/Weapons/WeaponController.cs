using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : BaseObject
{
    public Projectile projectileTransform;

    public override void Setup()
    {
        // stub -- to be maybe used for reloading and other weapon system innovations

        projectileTransform.gameObject.SetActive(false);
    }

    public override void Logic()
    {
        // stub -- to be maybe used for reloading and other weapon system innovations
    }

    public void Fire(Vector3 direction, float speed)
    {
        Projectile t = ProjectilePoolManager.Instance.AddProjectile(projectileTransform);

        t.transform.forward = direction;
        t.transform.position = transform.position;
        t.Setup();
        t.SetVelocity(direction * speed);
    }
}
