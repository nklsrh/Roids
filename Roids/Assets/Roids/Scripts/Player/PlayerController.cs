using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class PlayerController : BaseObject
{
    // __________________________________________________________________________________________EDITOR

    public HealthController healthController;

    [SerializeField]
    WeaponController weaponController;

    [SerializeField]
    Transform pivotObject;

    // __________________________________________________________________________________________PUBLIC

    public Vector3 Velocity
    {
        get
        {
            return velocity;
        }
    }

    // __________________________________________________________________________________________GAME VARIABLES

    [Header("Input")]
    public float thrustMultiplier = 0.1f;
    public float rotationMultiplier = 1f;

    [Header("Movement")]
    public float accelerationSlowdown = 10.0f;
    public float velocitySlowdown = 1.0f;

    public float accelerationSlowdownWhenNotThrusting = 10.0f;
    public float velocitySlowdownWhenNotThrusting = 1.0f;

    public float rotationSlowdown = 10.0f;
    public float maxRotationAmount = 10;

    public float maximumVelocityMagnitude = 10.0f;
    public float maximumAccelerationMagnitude = 1.0f;

    [Header("Weapon")]
    public float fireSpeed = 25.0f;

    [Header("Visuals")]
    public float leanLerp = 5.0f;
    public float leanRollMultiplier = 0.2f;
    public float leanPitchMultiplier = 0.2f;

    // __________________________________________________________________________________________PRIVATES (heh)
            
    Vector3 velocity;
    Vector3 acceleration;

    float rotationAmount = 0;
    float currentThrustAmount = 0;
    Quaternion pivotRotation;

    // __________________________________________________________________________________________METHODS

    public override void Setup()
    {
		healthController.Setup(100);

        ProjectilePoolManager projectileManagerPlayer = new ProjectilePoolManager(50);
        weaponController.Setup(projectileManagerPlayer);

    }

    public override void Logic ()
    {
        base.Logic();

        MovementLogic();
        WeaponLogic();
    }

    void MovementLogic()
    {
        bool isThrusting = currentThrustAmount > 0;

        Debug.DrawLine(transform.position, transform.position + velocity * 10, isThrusting ? Color.yellow : Color.blue);

        // MOVEMENT PHYSICS LOGIC


        velocity += acceleration;

        velocity = Vector3.ClampMagnitude(velocity, maximumVelocityMagnitude);

        transform.Translate(velocity * Time.deltaTime, Space.World);


        // ROTATION LOGIC

        transform.Rotate(Vector3.up * rotationAmount * Time.deltaTime);

        rotationAmount = Mathf.Lerp(rotationAmount, 0, rotationSlowdown * Time.deltaTime);



        // WEAPON LOGIC

        weaponController.Logic();

        // MOVEMENT SLOWDOWN

        acceleration = Vector3.Lerp(acceleration, Vector3.zero, (isThrusting ? accelerationSlowdown : accelerationSlowdownWhenNotThrusting) * Time.deltaTime);
        velocity = Vector3.Lerp(velocity, Vector3.zero, (isThrusting ? velocitySlowdown : velocitySlowdownWhenNotThrusting) * Time.deltaTime);


        pivotRotation = Quaternion.Euler(currentThrustAmount * leanPitchMultiplier, 0, rotationAmount * leanRollMultiplier);

        pivotObject.localRotation = Quaternion.Slerp(pivotObject.localRotation, pivotRotation, leanLerp * Time.deltaTime);


        currentThrustAmount = 0;
    }

    void WeaponLogic()
    {
        weaponController.Logic();
    }

    public void Turn(float amount)
    {
        rotationAmount += rotationMultiplier * amount;
        rotationAmount = Mathf.Clamp(rotationAmount, -maxRotationAmount, maxRotationAmount);
    }

    public void Thrust(float amount)
    {
        Vector3 thrustAcceleration = transform.forward * amount * thrustMultiplier;

        Debug.DrawLine(transform.position, transform.position + thrustAcceleration * 100, Color.red);
        
        acceleration += thrustAcceleration;

        acceleration = Vector3.ClampMagnitude(acceleration, maximumAccelerationMagnitude);

        currentThrustAmount = amount;
    }

    public void Fire()
    {
        if (weaponController.IsReady)
        {
            weaponController.Fire(transform.forward, fireSpeed);
        }
    }

    public void Slowdown()
    {
        velocity = Vector3.Lerp(velocity, Vector3.zero, 10 * Time.deltaTime);
    }


    void OnTriggerEnter(Collider other)
    {
        bool isHit = false;
        float damage = 0;

		if (healthController.IsAlive)
		{
			ProjectileEnemy projectile = other.gameObject.GetComponent<ProjectileEnemy>();
			if (projectile != null)
			{
				projectile.Die();
                damage = projectile.damage;
                isHit = true;
			}
            else
            {
                Badguy b = other.gameObject.GetComponent<Badguy>();
                if (b != null)
                {
                    damage = b.healthController.Health;
                }
            }
		}        

        
        if (isHit)
        {
            healthController.Damage(damage);
        }
    }
}
