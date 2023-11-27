using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitableObject : PhysicObject
{
    [SerializeField] Vector3 gravity = new Vector3(0, -9.8f, 0);
    [SerializeField] LayerMask groundLayer;

    private CheckGrounded grounded;

    protected virtual void Awake()
    {
        grounded = new CheckGrounded(transform, groundLayer);
    }

    protected override void FixedUpdate()
    {
        if (!grounded.IsGrounded())
            acceleration += gravity * Time.deltaTime;
        else
        {
            acceleration = Vector3.zero;
            velocity = Vector3.zero;
        }
        base.FixedUpdate();
    }
}
