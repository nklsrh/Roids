using UnityEngine;

public class WeaponController : BaseObject
{
    public Projectile projectileTransform;
    public float fireRate = 2.0f;

    public bool IsReady
    {
        get; private set;
    }

    float timeUntilFire = 3.0f;
    ProjectilePoolManager pool;

    public override void Setup()
    {
        projectileTransform.gameObject.SetActive(false);
    }

    public void Setup(ProjectilePoolManager pool)
    {
        this.pool = pool;
        IsReady = false;

        Setup();
    }

    public override void Logic()
    {
        if (timeUntilFire <= 0)
        {
            IsReady = true;
        }
        else
        {
            timeUntilFire -= Time.deltaTime;
        }

        pool.Logic();
    }

    public void Fire(Vector3 direction, float speed)
    {
        Projectile t = pool.AddProjectile(projectileTransform);

        t.transform.forward = direction;
        t.transform.position = transform.position;
        t.Setup();
        t.SetVelocity(direction * speed);

        timeUntilFire = 1 / fireRate;

        IsReady = false;
    }

    public void Disable()
    {
        pool.DisableAll();
    }
}
