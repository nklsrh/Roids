using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Badguy : BaseObject
{
    // __________________________________________________________________________________________EDITOR

    public float damagePlayerWhenCollide = 20.0f;

    // __________________________________________________________________________________________PUBLICS

    public Vector3 Direction
    {
        get;
        private set;
    }
    public float Speed
    {
        get;
        private set;
    }

    public Vector3 HitFromDirection
    {
        get;
        private set;
    }

    public float ExplosionSize
    {
        get; protected set;
    }


    // __________________________________________________________________________________________PRIVATES (heh)

    protected Wave.EnemyType enemyType = Wave.EnemyType.Asteroid;

    public Wave.EnemyType EnemyType
    {
        get
        {
            return enemyType;
        }
    }

    public HealthController healthController;

    protected System.Action<Badguy> onHit;

    // __________________________________________________________________________________________METHODS

    public Badguy(Wave.EnemyType enemyType) { }
    
    public virtual void Setup(Vector3 direction, float speed, float health, float size, System.Action<Badguy> onHit)
    {
        this.Direction = direction;
        this.Speed = speed;

        healthController.Setup(health);
        healthController.onDeath += Die;

        this.transform.localScale = Vector3.one * size;

        this.onHit = onHit;

        gameObject.SetActive(true);
    }

    public override void Setup()
    {
        gameObject.SetActive(true);
    }

    public override void Logic()
    {
        base.Logic();
        transform.position += Direction * Speed * Time.deltaTime;
    }

    public virtual void Die()
    {
        ExplosionSize = healthController.HealthMax > 50 ? healthController.HealthMax / 80f : 0.5f;
        gameObject.SetActive(false);
    }

    public virtual void OnTriggerEnter(Collider collider)
    {
        ProjectilePlayer projectile = collider.gameObject.GetComponent<ProjectilePlayer>();
        if (projectile != null)
        {
            projectile.Die();

            healthController.Damage(projectile.damage);

            HitFromDirection = projectile.Velocity.normalized; //(transform.position - collider.transform.position).normalized;
            if (onHit != null)
            {
                onHit.Invoke(this);
            }
        }

        PlayerController player = collider.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.healthController.Damage(damagePlayerWhenCollide);
            healthController.Damage(damagePlayerWhenCollide);
        }
    }
}
