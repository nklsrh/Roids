using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Asteroid : BaseObject
{
    Vector3 direction;
    float speed;

    System.Action<Asteroid> onDeath;

    public void SetupAsteroid(Vector3 direction, float speed, System.Action<Asteroid> onDeath)
    {
        this.direction = direction;
        this.speed = speed;

        this.onDeath = onDeath;
        gameObject.SetActive(true);
    }

    public override void Setup()
    {
        gameObject.SetActive(true);
    }

    public override void Logic()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Die()
    {
        gameObject.SetActive(false);
        if (onDeath != null)
        {
            onDeath.Invoke(this);
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<ProjectilePlayer>())
        {
            Die();
        }
    }
}
