using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Idle Settings", menuName = "PlayerStates Settings/Idle Settings")]
public class IdleSettings : ScriptableObject
{
    [SerializeField] InputAction walkingAction;
    [SerializeField] InputAction aimAction;
    [SerializeField] float turnSpeed;
    [SerializeField] float smoothTime;

    public InputAction WalkingAction { get { return walkingAction; } }
    public InputAction AimAction { get { return aimAction; } }
    public float TurnSpeed { get { return turnSpeed; } }
    public float SmoothTime { get { return smoothTime; } }
}
