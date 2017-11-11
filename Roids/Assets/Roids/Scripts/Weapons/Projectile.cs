using UnityEngine;
using System.Collections;

public class Projectile : BaseObject
{
    public Vector3 Velocity
    {
        get;
        protected set;
    }

    public float lifetime = 1.0f;

    public float damage;

    protected System.Action<Projectile> onDeath;

    public override void Setup()
    {
        gameObject.SetActive(true);
    }

    public virtual void SetupProjectile(System.Action<Projectile> onDeath)
    {
        this.onDeath = onDeath;
    }

    public override void Logic()
    {
        transform.position += Velocity * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Die();
        }
    }

    public virtual void SetVelocity(Vector3 velocity)
    {
        this.Velocity = velocity;
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
        if (onDeath != null)
        {
            onDeath.Invoke(this);
        }
    }
}
