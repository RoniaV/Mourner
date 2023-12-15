using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FallSettings", menuName = "PlayerStates Settings/Fall Settings")]
public class FallSettings : ScriptableObject
{
    [SerializeField] float fallSpeed;
    [SerializeField] float smoothTime;

    public float FallSpeed { get { return fallSpeed; } }
    public float SmoothTime { get { return smoothTime; } }
}
