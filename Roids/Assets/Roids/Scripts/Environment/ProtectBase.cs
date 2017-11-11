using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class ProtectBase : BaseObject 
{
	public HealthController healthController;
	public Transform indicator;

    public float explosionSizeWhenHit = 0.1f;
    public float explosionSizeWhenKilled = 0.2f;

    public System.Action<ProtectBase> onDeath;
    
    public override void Setup()
	{
		healthController.Setup(50);
		healthController.onDeath += OnDeath;
        healthController.onDamage += OnDamage;

		indicator.gameObject.SetActive(true);
	}

    public void Disable()
    {
        indicator.gameObject.SetActive(false);
        healthController.Die();
	}

	public override void Logic()
	{

    }

    private void OnDamage(float damage)
    {
        GameDirector.Explosion(transform.position, damage * explosionSizeWhenHit);
    }

    private void OnDeath()
	{
		if(onDeath != null)
		{
			onDeath.Invoke(this);
        }
        indicator.gameObject.SetActive(false);
        GameDirector.Explosion(transform.position, explosionSizeWhenKilled);
    }

	void OnTriggerStay(Collider other)
	{
		if (healthController.IsAlive)
		{
			ProjectileEnemy projectile = other.gameObject.GetComponent<ProjectileEnemy>();
			if (projectile != null)
			{
				projectile.Die();
				healthController.Damage(projectile.damage);
			}
		}
	}
}
