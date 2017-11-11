using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Saucer : Badguy
{
    [SerializeField]
    WeaponController weaponController;

    public float fireSpeedBase = 5.0f;
    public float fireSpeedMax = 10.0f;

    float timeUntilFire = 3.0f;

    Transform target;
    float skill = 0.0f;

    public Saucer() : base (Wave.EnemyType.Saucer)
    {
        enemyType = Wave.EnemyType.Saucer;
    }

    public void Setup(Vector3 direction, float speed, int health, float size, float skill, Transform target, ProjectilePoolManager projectileManager, System.Action<Saucer> onDeath)
    {
        base.Setup(direction, speed, health, size, null);

        this.skill = skill;
        this.target = target;

        weaponController.Setup(projectileManager);

        if (onDeath != null)
        {
            this.healthController.onDeath += (() =>
            {
                onDeath.Invoke(this);
            });
        }
    }

    public override void Logic()
    {
        base.Logic();

        weaponController.Logic();

        if (weaponController.IsReady)
        {
            float fireSpeed = fireSpeedBase + Mathf.Min(skill * fireSpeedMax, fireSpeedMax);
            Vector3 randomness = Mathf.Clamp01(1 / (skill + 0.1f)) * Vector3.Scale(Random.onUnitSphere, new Vector3(1, 0, 1));
            Vector3 targetDelta = target != null ? (target.position - transform.position) : Vector3.zero;

            weaponController.Fire((targetDelta + randomness).normalized, fireSpeed);
        }
    }
}
