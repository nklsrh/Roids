using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Badguy : BaseObject
{
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
    public int Lives
    {
        get;
        private set;
    }

    public Vector3 HitFromDirection
    {
        get;
        private set;
    }

    // __________________________________________________________________________________________PRIVATES (heh)

    protected Wave.EnemyType enemyType = Wave.EnemyType.Stomper;

    public Wave.EnemyType EnemyType
    {
        get
        {
            return enemyType;
        }
    }

    protected System.Action<Badguy> onHit;

    // __________________________________________________________________________________________METHODS

    public Badguy(Wave.EnemyType enemyType) { }
    
    public virtual void Setup(Vector3 direction, float speed, int health, float size, System.Action<Badguy> onHit)
    {
        this.Direction = direction;
        this.Speed = speed;

        this.Lives = health;
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
        transform.position += Direction * Speed * Time.deltaTime;
    }


    public virtual void Damage()
    {
        Lives--;
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnTriggerEnter(Collider collider)
    {
        ProjectilePlayer projectile = collider.gameObject.GetComponent<ProjectilePlayer>();
        if (projectile != null)
        {
            projectile.Die();

            Damage();
            
            HitFromDirection = projectile.Velocity.normalized; //(transform.position - collider.transform.position).normalized;
            if (onHit != null)
            {
                onHit.Invoke(this);
            }
        }
    }
}
