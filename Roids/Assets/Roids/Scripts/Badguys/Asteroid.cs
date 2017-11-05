using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Asteroid : BaseObject
{
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

    System.Action<Asteroid> onHit;

    public void SetupAsteroid(Vector3 direction, float speed, int health, float size, System.Action<Asteroid> onHit)
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


    public void Damage()
    {
        Lives--;
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider collider)
    {
        ProjectilePlayer projectile = collider.gameObject.GetComponent<ProjectilePlayer>();
        if (projectile != null)
        {
            projectile.Die();

            HitFromDirection = projectile.Velocity.normalized; //(transform.position - collider.transform.position).normalized;
            if (onHit != null)
            {
                onHit.Invoke(this);
            }
        }
    }
}
