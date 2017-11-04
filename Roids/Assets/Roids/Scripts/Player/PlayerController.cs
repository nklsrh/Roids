using UnityEngine;
using System.Collections;

public class PlayerController : BaseObject
{
    public WeaponController weaponController;

    //int score;

    Vector3 velocity;
    Vector3 acceleration;

    public float thrustMultiplier = 0.1f;

    public float accelerationSlowdown = 10.0f;
    public float velocitySlowdown = 1.0f;
    public bool slowdownAcclerationOnlyWhenNotThrusting = false;
    public bool slowdownVelocityOnlyWhenNotThrusting = false;

    float rotationAmount = 0;
    public float rotationSlowdown = 10.0f;
    public float maxRotationAmount = 10;


    float currentThrustAmount = 0;

    public override void Setup ()
    {
        weaponController.Setup();
    }

    public override void Logic ()
    {
        velocity += acceleration;
        transform.Translate(velocity * Time.deltaTime);


        transform.Rotate(Vector3.up * rotationAmount);

        rotationAmount = Mathf.Lerp(0, rotationAmount, rotationSlowdown * Time.deltaTime);

        weaponController.Logic();


        if (!slowdownAcclerationOnlyWhenNotThrusting || (slowdownAcclerationOnlyWhenNotThrusting && currentThrustAmount == 0))
        {
            acceleration = Vector3.Lerp(acceleration, Vector3.zero, accelerationSlowdown * Time.deltaTime);
        }

        if (!slowdownVelocityOnlyWhenNotThrusting || (slowdownVelocityOnlyWhenNotThrusting && currentThrustAmount == 0))
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, velocitySlowdown * Time.deltaTime);
        }

        //velocity = Vector3.Lerp(velocity, transform.forward * 0.1f, velocitySlowdown * Time.deltaTime);
    }

    public void Turn(float amount)
    {
        rotationAmount += amount;
        rotationAmount = Mathf.Clamp(rotationAmount, -maxRotationAmount, maxRotationAmount);
    }

    public void Thrust(float amount)
    {
        acceleration += transform.forward * amount * thrustMultiplier;
        currentThrustAmount = amount;
    }

    public void Fire()
    {
        weaponController.Fire(transform.forward, 10);
    }
}
