using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "RunSettings", menuName = "PlayerStates Settings/Run Settings")]
public class RunSettings : ScriptableObject
{
    [SerializeField] float runSpeed;
    [SerializeField] float smoothTime;

    public float RunSpeed { get { return runSpeed; } }
    public float SmoothTime { get { return smoothTime; } }
}
