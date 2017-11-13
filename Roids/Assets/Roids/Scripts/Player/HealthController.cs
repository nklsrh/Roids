using UnityEngine;

[System.Serializable]
public class HealthController : BaseObject
{
    // __________________________________________________________________________________________EDITOR
    public bool isTrackedByUI = true;

    public System.Action<float> onDamage;
    public System.Action<float> onRegen;
    public System.Action onDeath;
    
    public float Health
    {
        get; private set;
    }

    public float HealthMax
    {
        get; private set;
    }

    public bool IsAlive
    {
        get
        {
            return Health > 0;
        }
    }

    // __________________________________________________________________________________________PRIVATES

    private bool isInvincible = false;

    // __________________________________________________________________________________________METHODS

    public override void Setup() { }

    public void Setup(float initialHealth)
    {
        Health = initialHealth;
        HealthMax = initialHealth;

        if (onCreated != null)
        {
            onCreated.Invoke(this);
        }
    }


    public void Damage(float amount)
    {
        if (isInvincible)
        {
            return;
        }

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
        
        if (onDestroyed != null)
        {
            onDestroyed.Invoke(this);
        }
    }

    public void SetInvincible(bool isInvincible)
    {
        this.isInvincible = isInvincible;
    }


    // __________________________________________________________________________________________STATICS

    public static System.Action<HealthController> onCreated;
    public static System.Action<HealthController> onDestroyed;
}
