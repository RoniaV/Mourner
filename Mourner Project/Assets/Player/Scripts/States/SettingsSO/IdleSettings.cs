using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Idle Settings", menuName = "PlayerStates Settings/Idle Settings")]
public class IdleSettings : ScriptableObject
{
    [SerializeField] float turnSpeed;
    [SerializeField] float smoothTime;

    public float TurnSpeed { get { return turnSpeed; } }
    public float SmoothTime { get { return smoothTime; } }
}
