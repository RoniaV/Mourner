using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "RunSettings", menuName = "PlayerStates Settings/Run Settings")]
public class RunSettings : ScriptableObject
{
    [SerializeField] InputAction walkingAction;
    [SerializeField] InputAction aimAction;
    [SerializeField] InputAction runAction;
    [SerializeField] float runSpeed;
    [SerializeField] float smoothTime;

    public InputAction WalkingAction { get { return walkingAction; } }
    public InputAction AimAction { get { return aimAction; } }
    public InputAction RunAction { get { return runAction; } }
    public float RunSpeed { get { return runSpeed; } }
    public float SmoothTime { get { return smoothTime; } }
}
