using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Asteroid : Badguy
{
    public float explosionSizeWhenHit = 0.17f;
    public float rotationMultiplier = 0.2f;

    public int ChunksRemaining
    {
        get; private set;
    }

    public Asteroid () : base(Wave.EnemyType.Asteroid)
    {
        enemyType = Wave.EnemyType.Asteroid;
    }

    public void Setup(Vector3 direction, float speed, float health, float size, int chunks, System.Action<Asteroid> onHit)
    {
        base.Setup(direction, speed, health, size, null);

        ChunksRemaining = chunks;

        this.onHit = ((Badguy b)=>
        {
            onHit.Invoke(b as Asteroid);
        });
    }

    public override void Die()
    {
        base.Die();

        GameDirector.Explosion(transform.position, (1 + ChunksRemaining) * explosionSizeWhenHit);
    }

    public override void Logic()
    {
        base.Logic();

        transform.Rotate(Speed * Speed * Random.onUnitSphere * rotationMultiplier + Speed * Quaternion.LookRotation(Direction).eulerAngles * rotationMultiplier * Time.deltaTime, Space.Self);
    }
}
