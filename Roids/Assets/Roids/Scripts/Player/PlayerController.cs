using UnityEngine;
using System.Collections;

public class PlayerController : BaseObject
{
    public WeaponController weaponController;

    //int score;

    Vector3 velocityDirection;
    Vector3 accelerationDirection;

    Vector3 velocity;
    Vector3 acceleration;

    public float thrustMultiplier = 0.1f;
    public float rotationMultiplier = 1f;

    public float accelerationSlowdown = 10.0f;
    public float velocitySlowdown = 1.0f;

    public float accelerationSlowdownWhenNotThrusting = 10.0f;
    public float velocitySlowdownWhenNotThrusting = 1.0f;

    float rotationAmount = 0;
    public float rotationSlowdown = 10.0f;
    public float maxRotationAmount = 10;

    public float maximumVelocityMagnitudeSquared = 10.0f;
    public float maximumAccelerationMagnitudeSquared = 1.0f;

    float currentThrustAmount = 0;

    public override void Setup ()
    {
        weaponController.Setup();
    }

    public override void Logic ()
    {

        Debug.DrawLine(transform.position, transform.position + velocity * 10, Color.yellow);
        //Debug.DrawLine(transform.position, transform.position + acceleration * 100, Color.blue);

        // MOVEMENT PHYSICS LOGIC


        velocity += acceleration;

        transform.Translate(velocity * Time.deltaTime, Space.World);


        // ROTATION LOGIC

        transform.Rotate(Vector3.up * rotationAmount * Time.deltaTime);

        rotationAmount = Mathf.Lerp(rotationAmount, 0, rotationSlowdown * Time.deltaTime);



        // WEAPON LOGIC

        weaponController.Logic();



        // MOVEMENT SLOWDOWN

        bool isThrusting = currentThrustAmount > 0;
        acceleration = Vector3.Lerp(acceleration, Vector3.zero, (isThrusting ? accelerationSlowdown : accelerationSlowdownWhenNotThrusting)  * Time.deltaTime);
        velocity = Vector3.Lerp(velocity, Vector3.zero, (isThrusting ? velocitySlowdown : velocitySlowdownWhenNotThrusting) * Time.deltaTime);

        currentThrustAmount = 0;
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

        currentThrustAmount = amount;
    }

    public void Fire()
    {
        weaponController.Fire(transform.forward, 10);
    }
}
