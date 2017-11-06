using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Saucer : Badguy
{
    [SerializeField]
    WeaponController weaponController;

    // how many shots per second
    public float fireRate = 2.0f;

    float timeUntilFire = 3.0f;

    public void Setup(Vector3 direction, float speed, int health, float size, ProjectilePoolManager projectileManager, System.Action<Saucer> onHit)
    {
        base.Setup(direction, speed, health, size, null);

        weaponController.Setup(projectileManager);
        
        this.onHit = ((Badguy b)=>
        {
            onHit.Invoke(b as Saucer);
        });
    }

    public override void Logic()
    {
        base.Logic();

        weaponController.Logic();

        if (timeUntilFire <= 0)
        {
            weaponController.Fire(Vector3.Scale(Random.onUnitSphere, new Vector3(1, 0, 1)), 2.0f);
            timeUntilFire = 1 / fireRate;
        }
        else
        {
            timeUntilFire -= Time.deltaTime;
        }
    }
}
