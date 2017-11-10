using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class ProtectBase : BaseObject 
{
	public HealthController healthController;

	public Transform indicator;

	public System.Action<ProtectBase> onDeath;

	public override void Setup()
	{
		healthController.Setup(50);
		healthController.onDeath += OnDeath;

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

	void OnDeath()
	{
		if(onDeath != null)
		{
			onDeath.Invoke(this);
		}
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
