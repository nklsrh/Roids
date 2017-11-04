using UnityEngine;
using System.Collections;

public class Projectile : BaseObject
{
    Vector3 velocity;

    public override void Setup()
    {
    }

    public override void Logic()
    {
        transform.position += velocity * Time.deltaTime;
    }

    public void SetVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
    }

}
