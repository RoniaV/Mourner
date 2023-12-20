using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpSettings", menuName = "PlayerStates Settings/Jump Settings")]
public class JumpSettings : MovementSettings
{
    [SerializeField] private float inertia = 0.5f;

    public float Inertia { get { return inertia; } }
}
