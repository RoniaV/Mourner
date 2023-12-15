using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpSettings", menuName = "PlayerStates Settings/Jump Settings")]
public class JumpSettings : ScriptableObject
{
    [SerializeField] float jumpSpeed;
    [SerializeField] float smoothTime;

    public float JumpSpeed { get { return jumpSpeed; } }
    public float SmoothTime { get { return smoothTime; } }
}
