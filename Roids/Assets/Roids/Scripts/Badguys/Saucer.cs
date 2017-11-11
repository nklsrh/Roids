﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Saucer : Badguy
{
    [SerializeField]
    WeaponController weaponController;

    public float fireSpeedBase = 5.0f;
    public float fireSpeedMax = 10.0f;

    public float explosionSizeWhenHit = 0.02f;
    public float explosionSizeWhenKilled = 0.9f;

    List<HealthController> targets;
    HealthController currentTarget;
    float skill = 0.0f;

    public Saucer() : base (Wave.EnemyType.Saucer)
    {
        enemyType = Wave.EnemyType.Saucer;
    }

    public void Setup(Vector3 direction, float speed, float health, float size, float skill, List<HealthController> targets, ProjectilePoolManager projectileManager, System.Action<Saucer> onDeath)
    {
        base.Setup(direction, speed, health, size, null);

        this.skill = skill;
        this.targets = targets;

        SelectNewTarget();

        weaponController.Setup(projectileManager);

        if (onDeath != null)
        {
            healthController.onDeath += (() =>
            {
                onDeath.Invoke(this);
            });
        }

        healthController.onDamage += OnDamage;
    }

    private void SelectNewTarget()
    {
        foreach (HealthController hc in targets)
        {
            if (!hc.IsAlive)
            {
                targets.Remove(hc);
            }
        }

        if (targets.Count > 0)
        {
            currentTarget = targets[Random.Range(0, targets.Count)];
        }
        else
        {
            currentTarget = null;
        }
    }

    public override void Logic()
    {
        base.Logic();

        weaponController.Logic();

        if (weaponController.IsReady)
        {
            if (currentTarget == null || !currentTarget.IsAlive)
            {
                SelectNewTarget();
            }

            float fireSpeed = fireSpeedBase + Mathf.Min(skill * fireSpeedMax, fireSpeedMax);
            Vector3 randomness = Mathf.Clamp01(1 / (skill + 0.1f)) * Vector3.Scale(Random.onUnitSphere, new Vector3(1, 0, 1));
            Vector3 targetDelta = currentTarget != null ? (currentTarget.transform.position - transform.position) : Vector3.zero;

            weaponController.Fire((targetDelta + randomness).normalized, fireSpeed);
        }
    }

    public override void Die()
    {
        GameDirector.Explosion(transform.position, explosionSizeWhenKilled);

        weaponController.Disable();

        base.Die();
    }

    private void OnDamage(float damage)
    {
        GameDirector.Explosion(transform.position, damage * explosionSizeWhenHit);
    }
}
