using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : BaseObject
{
    public Vector3 Velocity
    {
        get;
        protected set;
    }

    public float lifetime = 1.0f;
    float currentLifetime = 0;

    public float damage;

    public List<AudioSource> audioBlastOptions;

    protected System.Action<Projectile> onDeath;

    public override void Setup()
    {
        gameObject.SetActive(true);
        currentLifetime = lifetime;
    }

    public virtual void SetupProjectile(System.Action<Projectile> onDeath)
    {
        this.onDeath = onDeath;

        PlayRandomBlastSound();
    }

    public override void Logic()
    {
        transform.position += Velocity * Time.deltaTime;
        currentLifetime -= Time.deltaTime;
        if (currentLifetime <= 0)
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

    private void PlayRandomBlastSound()
    {
        for (int i = 0; i < audioBlastOptions.Count; i++)
        {
            audioBlastOptions[i].gameObject.SetActive(false);
        }
        audioBlastOptions[Random.Range(0, audioBlastOptions.Count)].gameObject.SetActive(true);
    }
}
