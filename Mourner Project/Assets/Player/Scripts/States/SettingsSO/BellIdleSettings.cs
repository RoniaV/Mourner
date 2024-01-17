using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bell Idle Settings", menuName = "PlayerStates Settings/Bell Idle Settings")]
public class BellIdleSettings : ScriptableObject
{
    [SerializeField] float turnSpeed;
    [SerializeField] float stepVel;
    [SerializeField] float smoothTime;

    public float TurnSpeed { get { return turnSpeed; } }
    public float StepVel { get { return stepVel; } }
    public float SmoothTime { get { return smoothTime; } }
}
