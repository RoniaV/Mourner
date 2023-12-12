using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(CharacterGravitable))]
public class CharacterJump : MonoBehaviour
{
    public bool isJumping { get; private set; }
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
            Debug.Log("Jump Input pressed. Grounded: " + characterGravitable.isGrounded);
            doJump = false;

            if (characterGravitable.isGrounded)
            {
                characterGravitable.AddVerticalVelocity(Mathf.Sqrt(jumpHeight * 9.8f));
                isJumping = true;
                Debug.Log("Jump");
                OnJump?.Invoke();
            }
        }

        if(isJumping && characterController.velocity.y < 0)
        {
            isFaling = true;
        }

        if (isFaling && characterGravitable.isGrounded)
        {
            isJumping = false;
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
