using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    public Vector3 ActualVelocity { get { return movementDirection * actualVelocity; } }
    public float VerticalVelocity { get { return verticalVelocity; } }

    [Header("Movement Settings")]
    [SerializeField] protected float velocity = 2f;
    [SerializeField] protected float smoothTime = 0.3f;

    protected CharacterController charController;

    protected Vector3 movementDirection;
    protected float actualVelocity;
    protected float desiredVelocity;
    private float dampVel = 0;
    private float verticalVelocity;

    protected virtual void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    protected virtual void Start()
    {
        actualVelocity = velocity;
    }

    protected virtual void FixedUpdate()
    {
        float smoothVel = desiredVelocity * movementDirection.magnitude;
        actualVelocity = Mathf.SmoothDamp(actualVelocity, smoothVel, ref dampVel, smoothTime);

        Vector3 movement = movementDirection * actualVelocity * Time.fixedDeltaTime;
        movement.y += verticalVelocity;
        
        charController.Move(movement);
    }

    public void SetMovementDirection(Vector3 direction)
    {
        //Debug.Log("Set Movement Direction: " + direction);
        movementDirection = direction;
    }

    public void SetVelocity(float newVel)
    {
        //Debug.Log("Set Velocity: " + newVel);
        desiredVelocity = newVel;
    }

    public void SetVerticalVelocity(float velocity)
    {
        verticalVelocity = velocity;
    }

    public void SetSmoothTime(float time)
    {
        smoothTime = time;
    }
}
