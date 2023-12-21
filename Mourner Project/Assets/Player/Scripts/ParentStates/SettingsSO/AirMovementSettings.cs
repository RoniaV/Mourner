using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMovementSettings : ScriptableObject
{
    [SerializeField] float movementSpeed;
    [SerializeField] float smoothTime;
    [SerializeField] float rotationSpeed;
    [SerializeField] private float inertia = 0.5f;

    public float MovSpeed { get { return movementSpeed; } }
    public float SmoothTime { get { return smoothTime; } }
    public float RotationSpeed { get { return rotationSpeed; } }
    public float Inertia { get { return inertia; } }
}
