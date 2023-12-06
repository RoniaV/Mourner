using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] protected float velocity = 2f;
    [SerializeField] protected float smoothTime = 0.3f;

    protected CharacterController charController;

    protected Vector3 movementDirection;
    protected float actualVelocity;
    protected float desiredVelocity;
    private float dampVel = 0;

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

        charController.Move(movementDirection * actualVelocity * Time.fixedDeltaTime);
    }

    public void SetMovementDirection(Vector3 direction)
    {
        movementDirection = direction;
    }

    public void SetVelocity(float newVel)
    {
        desiredVelocity = newVel;
    }

    public void SetSmoothTime(float time)
    {
        smoothTime = time;
    }
}
