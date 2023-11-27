using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterGravitable))]
public class CharacterFloorMovement : CharacterMovement
{
    [Tooltip("The maximum slope degree a character can walk")]
    [SerializeField] [Range(0, 90)] float maxSlope = 45f;

    protected CharacterGravitable characterGravitable;

    protected override void Awake()
    {
        base.Awake();
        characterGravitable = GetComponent<CharacterGravitable>();
    }

    protected override void FixedUpdate()
    {
        movementDirection = ProjectMovementOnGround(movementDirection);

        Debug.DrawLine(transform.position, transform.position + (movementDirection * 2), Color.blue);

        base.FixedUpdate();
    }

    protected Vector3 ProjectMovementOnGround(Vector3 movement)
    {
        RaycastHit groundHit = characterGravitable.GetGroundHit();

        if (GroundWalkable(groundHit))
            return Vector3.ProjectOnPlane(movement, groundHit.normal).normalized;

        else
            return movement;
    }

    protected bool GroundWalkable(RaycastHit groundHit)
    {
        if (groundHit.collider != null)
        {
            //Check slope
            Vector3 localNormal = groundHit.normal.normalized;
            float angle = Vector3.Angle(Vector3.up, localNormal);
            bool isOnValidGround = angle < maxSlope;

            Debug.DrawRay(groundHit.transform.position, localNormal, Color.red);
            return isOnValidGround;
        }
        else
        {
            //Debug.Log("Not grounded");
            return false;
        }
    }
}
