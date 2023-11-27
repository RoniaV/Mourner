using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterKickable : MonoBehaviour
{
    [HideInInspector]
    public bool isBeingKicked;

    [SerializeField] float braking = -3;
    [SerializeField] float pushLenght = 4;
    [SerializeField] float pushMinimumSpeed = 2;

    private CharacterController characterController;

    private float acceleration;
    private Vector3 kickDirection;

    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    protected virtual void FixedUpdate()
    {
        if (isBeingKicked)
        {
            acceleration += braking * Time.fixedDeltaTime;

            if (acceleration <= pushMinimumSpeed)
            {
                acceleration = 0;
                isBeingKicked = false;

                Debug.Log("Stop Push");
            }

            characterController.Move(kickDirection * acceleration * Time.fixedDeltaTime);
        }

    }

    public void Kick(Vector3 direction)
    {
        if (!isBeingKicked)
        {
            acceleration += Mathf.Sqrt(pushLenght * -braking);
            kickDirection = direction;

            isBeingKicked = true;

            Debug.Log("Start Push");
        }
    }
}
