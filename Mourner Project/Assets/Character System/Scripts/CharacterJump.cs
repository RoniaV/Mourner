using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(CharacterGravitable))]
public class CharacterJump : MonoBehaviour
{
    public bool IsJumping { get; private set; }
    public event Action OnJump;
    public event Action OnLanded;


    [SerializeField] float jumpHeight = 4;

    CharacterGravitable characterGravitable;
    CharacterController characterController;

    protected bool doJump;
    protected bool isFaling;

    protected virtual void Awake()
    {
        characterGravitable = GetComponent<CharacterGravitable>();
        characterController = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        if (doJump)
        {
            Debug.Log("Jump Input pressed. Grounded: " + characterGravitable.IsGrounded);
            doJump = false;

            if (characterGravitable.IsGrounded)
            {
                characterGravitable.AddVerticalVelocity(Mathf.Sqrt(jumpHeight * 9.8f));
                IsJumping = true;
                Debug.Log("Jump");
                OnJump?.Invoke();
            }
        }

        if(IsJumping && characterController.velocity.y < -0.5f)
        {
            Debug.Log("Vertical Velocity: " + characterController.velocity.y);
            isFaling = true;
        }

        if (isFaling && characterGravitable.IsGrounded)
        {
            IsJumping = false;
            isFaling = false;
            Debug.Log("Landed");
            OnLanded?.Invoke();
        }
    }

    public void Jump()
    {
        doJump = true;
    }
}
