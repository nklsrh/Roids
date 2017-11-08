using UnityEngine;
using System.Collections;

[System.Serializable]
public class HealthController
{
    public float Health
    {
        get; private set;
    }

    public float HealthMax
    {
        get; private set;
    }

    public System.Action<float> onDamage;
    public System.Action<float> onRegen;
    public System.Action onDeath;

    public HealthController (float initialHealth)
    {
        Setup(initialHealth);
    }

    public void Setup(float initialHealth)
    {
        Health = initialHealth;
        HealthMax = initialHealth;
    }

    public void Damage(float amount)
    {
        Health -= amount;

        if (onDamage != null)
        {
            onDamage.Invoke(amount);
        }

        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
    }

    public void Regen(float amount)
    {
        Health = Mathf.Min(HealthMax, Health + amount);

        if (onRegen != null)
        {
            onRegen.Invoke(amount);
        }
    }

    public void Die()
    {
        if (onDeath != null)
        {
            onDeath.Invoke();
        }
    }
}
