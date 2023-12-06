using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "WalkSettings", menuName = "PlayerStates Settings/Walk Settings")]
public class WalkSettings : ScriptableObject
{
    [SerializeField] float walkSpeed;
    [SerializeField] float smoothTime;

    public float WalkSpeed { get { return walkSpeed; } }
    public float SmoothTime { get { return smoothTime; } }
}
