using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSettings : ScriptableObject
{
    [SerializeField] float movementSpeed;
    [SerializeField] float smoothTime;
    [SerializeField] float rotationSpeed;

    public float MovSpeed { get { return movementSpeed; } }
    public float SmoothTime { get { return smoothTime; } }
    public float RotationSpeed { get { return rotationSpeed; } }
}
