using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicObject : MonoBehaviour
{
    [SerializeField] protected float mass = 1;

    protected Vector3 acceleration;
    protected Vector3 velocity;

    protected virtual void FixedUpdate()
    {
        velocity += acceleration * Time.fixedDeltaTime;
    }

    public void AddForce(Vector3 force)
    {
        acceleration += force / mass;
    }
}
