using UnityEngine;
using System.Collections;

public class Projectile : BaseObject
{
    protected Vector3 velocity;
    protected float lifetime = 1.0f;

    protected System.Action<Projectile> onDeath;

    public override void Setup()
    {
        gameObject.SetActive(true);
    }

    public virtual void SetupProjectile(System.Action<Projectile> onDeath)
    {
        this.onDeath = onDeath;
        lifetime = 1.0f;
    }

    public override void Logic()
    {
        transform.position += velocity * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Die();
        }
    }

    public virtual void SetVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
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
