using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FallSettings", menuName = "PlayerStates Settings/Fall Settings")]
public class FallSettings : MovementSettings
{
    [SerializeField] private float inertia = 0.5f;

    public float Inertia { get { return inertia; } }
}
