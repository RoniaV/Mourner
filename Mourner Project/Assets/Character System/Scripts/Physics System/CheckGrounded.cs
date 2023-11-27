using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrounded
{
    private Transform groundPoint;
    private LayerMask groundMask;
    private float range = 0.1f;
    private Vector3 boxcastHalfExtents = new Vector3(0.5f, 0.1f, 0.5f);
    private float sphereCastRadius = 0.5f;

    private RaycastHit lastHit;

    public CheckGrounded(Transform groundPoint, LayerMask groundMask)
    {
        this.groundPoint = groundPoint;
        this.groundMask = groundMask;
    }

    public CheckGrounded(Transform groundPoint, LayerMask groundMask, float range, Vector3 boxcastHalfExtents)
    {
        this.groundPoint = groundPoint;
        this.groundMask = groundMask;
        this.range = range;
        this.boxcastHalfExtents = boxcastHalfExtents;
    }

    public bool IsGrounded()
    {
        RaycastHit hit;

        bool isGrounded = Physics.SphereCast(
            groundPoint.position + groundPoint.up * sphereCastRadius,
            sphereCastRadius,
            -groundPoint.up * range,
            out hit,
            range,
            groundMask);

        lastHit = hit;

        return isGrounded;
    }

    public RaycastHit GetLastHit()
    {
        return lastHit;
    }

    public void DrawBoxGizmos()
    {
        Gizmos.DrawWireSphere(groundPoint.position, range);

        RaycastHit hit;


        if (Physics.BoxCast(
            groundPoint.position + groundPoint.up * range,
            boxcastHalfExtents,
            -groundPoint.up * range,
            out hit,
            Quaternion.identity,
            range,
            groundMask))
        {
            Gizmos.color = Color.green;
            Vector3 boxCastMidpoint = groundPoint.position + (-groundPoint.up * hit.distance);
            Gizmos.DrawWireCube(boxCastMidpoint, boxcastHalfExtents * 2);
            Gizmos.DrawSphere(hit.point, 0.1f);
            Debug.DrawLine(groundPoint.position + groundPoint.up * range, boxCastMidpoint, Color.green);
        }
        else
        {
            Gizmos.color = Color.red;
            Vector3 boxCastMidpoint = groundPoint.position + (-groundPoint.up * range);
            Gizmos.DrawWireCube(boxCastMidpoint, boxcastHalfExtents * 2);
            Debug.DrawLine(groundPoint.position + groundPoint.up * range, boxCastMidpoint, Color.red);
        }
    }

    public void DrawSphereGizmos()
    {
        Gizmos.DrawWireSphere(groundPoint.position, range);

        RaycastHit hit;

        if (Physics.SphereCast(
            groundPoint.position + groundPoint.up * sphereCastRadius,
            sphereCastRadius,
            -groundPoint.up * range,
            out hit,
            range,
            groundMask))
        {
            Gizmos.color = Color.green;
            Vector3 sphereCastMidpoint = groundPoint.position + (groundPoint.up * sphereCastRadius) + (-groundPoint.up * hit.distance);
            Gizmos.DrawWireSphere(sphereCastMidpoint, sphereCastRadius);
            Gizmos.DrawSphere(hit.point, 0.1f);
            Debug.DrawLine(groundPoint.position + groundPoint.up * sphereCastRadius, sphereCastMidpoint, Color.green);
        }
        else
        {
            Gizmos.color = Color.red;
            Vector3 sphereCastMidpoint = groundPoint.position + (groundPoint.up * sphereCastRadius) + (-groundPoint.up * range);
            Gizmos.DrawWireSphere(sphereCastMidpoint, sphereCastRadius);
            Debug.DrawLine(groundPoint.position + groundPoint.up * sphereCastRadius, sphereCastMidpoint, Color.red);
        }
    }
}
