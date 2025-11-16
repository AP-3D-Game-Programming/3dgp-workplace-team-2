using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }

    [System.Serializable]
    public struct Wheel
    {
        public GameObject WheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public float breakAcceleration = 50.0f;
    public float boostMultiplier = 1.8f;
    public float boostDuration = 2.5f;
    public float boostCooldown = 5f;
    public float boostKickForce = 4000f;
    public float maxSpeed = 50f;
    public float motorTorque = 50f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    private bool isBoosting = false;
    private float boostTimer = 0f;
    private float cooldownTimer = 0f;
    private float currentBoostMultiplier = 1f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput;

    float steerInput;

    private Rigidbody carRb;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
    }

    void Update()
    {
        GetInputs();
        AnimatedWheels();
        HandleBoostTimers();
    }

    void LateUpdate()
    {
        HandleBrakingAndReverse();
        Move();
        Steer();
    }

    void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isBoosting && cooldownTimer <= 0f)
            StartBoost();
    }
    void StartBoost()
    {
        isBoosting = true;
        boostTimer = boostDuration;
        cooldownTimer = boostCooldown;
        carRb.AddForce(transform.forward * boostKickForce, ForceMode.Impulse);
    }
    void HandleBoostTimers()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;

            // apply multiplier while boost is active
            currentBoostMultiplier = Mathf.Lerp(currentBoostMultiplier, boostMultiplier, Time.deltaTime * 8f);

            if (boostTimer <= 0f)
                isBoosting = false;
        }
        else
        {
            // smooth fade-out back to normal
            currentBoostMultiplier = Mathf.Lerp(currentBoostMultiplier, 1f, Time.deltaTime * 2f);
        }
    }

    void Move()
    {
        float speedFactor = Mathf.Clamp01((maxSpeed - carRb.linearVelocity.magnitude) / maxSpeed);
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = currentBoostMultiplier * moveInput * motorTorque * speedFactor;
        }

    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    void HandleBrakingAndReverse()
    {
        float forwardSpeed = Vector3.Dot(carRb.linearVelocity, transform.forward);
        float speedFactor = Mathf.Clamp01((maxSpeed - carRb.linearVelocity.magnitude) / maxSpeed);

        // 1. Handbrake (SPACE)
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 600 * breakAcceleration;
                wheel.wheelCollider.motorTorque = 0f;
            }
            return;
        }

        // 2. Reverse / Brake (S key)
        if (Input.GetKey(KeyCode.S))
        {
            foreach (var wheel in wheels)
            {
                if (forwardSpeed > 0.1f)
                {
                    // moving forward → brake first
                    wheel.wheelCollider.brakeTorque = 400 * breakAcceleration;
                    wheel.wheelCollider.motorTorque = 0f;
                }
                else
                {
                    // stopped or rolling backward → reverse
                    wheel.wheelCollider.brakeTorque = 0f;
                    wheel.wheelCollider.motorTorque = -speedFactor * motorTorque;
                }
            }
            return;
        }

        // 3. No braking or reversing
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.brakeTorque = 0f;
        }
    }


    void AnimatedWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;

            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.WheelModel.transform.position = pos;
            wheel.WheelModel.transform.rotation = rot;
        }
    }
}
