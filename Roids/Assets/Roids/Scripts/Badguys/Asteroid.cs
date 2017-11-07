using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Asteroid : Badguy
{
    public Asteroid () : base(Wave.EnemyType.Asteroid)
    {
        enemyType = Wave.EnemyType.Asteroid;
    }

    public void Setup(Vector3 direction, float speed, int health, float size, System.Action<Asteroid> onHit)
    {
        base.Setup(direction, speed, health, size, null);

        this.onHit = ((Badguy b)=>
        {
            onHit.Invoke(b as Asteroid);
        });
    }
}
