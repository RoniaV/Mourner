using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "WalkSettings", menuName = "PlayerStates Settings/Walk Settings")]
public class WalkSettings : ScriptableObject
{
    [SerializeField] InputAction walkingAction;
    [SerializeField] InputAction aimAction;
    [SerializeField] InputAction runAction;
    [SerializeField] float walkSpeed;
    [SerializeField] float smoothTime;

    public InputAction WalkingAction { get { return walkingAction; } }
    public InputAction AimAction { get { return aimAction; } }
    public InputAction RunAction { get { return runAction; } }
    public float WalkSpeed { get { return walkSpeed; } }
    public float SmoothTime { get { return smoothTime; } }
}
