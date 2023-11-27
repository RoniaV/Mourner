using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] protected float velocity = 2f;

    protected CharacterController charController;

    protected Vector3 movementDirection;
    protected float actualVelocity;

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
        charController.Move(movementDirection * actualVelocity * Time.fixedDeltaTime);
    }

    public void SetMovementDirection(Vector3 direction)
    {
        movementDirection = direction;
    }

    public void ModifyVelocity(float newVel, float time = 0)
    {
        actualVelocity = newVel != 0 ? newVel : velocity;

        if (time != 0)
            StartCoroutine(ModifiedVelocityCoroutine(time));
    }

    protected IEnumerator ModifiedVelocityCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        actualVelocity = velocity;
    }
}
