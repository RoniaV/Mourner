using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(CharacterGravitable))]
public class CharacterJump : MonoBehaviour
{
    public bool IsJumping { get; private set; }
    public event Action OnJump;


    [SerializeField] float jumpHeight = 4;
    [SerializeField] float jumpCooldown = 0.5f;

    CharacterGravitable characterGravitable;
    CharacterController characterController;

    protected bool doJump;
    private bool canJump = true;

    protected virtual void Awake()
    {
        characterGravitable = GetComponent<CharacterGravitable>();
        characterController = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        characterGravitable.OnLanded += Land;
    }

    void OnDisable()
    {
        characterGravitable.OnLanded -= Land;
    }

    void FixedUpdate()
    {
        if (doJump)
        {
            Debug.Log("Jump Input pressed. Grounded: " + characterGravitable.IsGrounded);
            doJump = false;

            if (characterGravitable.IsGrounded && !IsJumping && canJump)
            {
                Debug.Log("Jump");
                IsJumping = true;
                canJump = false;

                characterGravitable.AddVerticalVelocity(Mathf.Sqrt(jumpHeight * 9.8f));

                OnJump?.Invoke();
            }
        }
    }

    public bool Jump()
    {
        if(canJump)
        {
            doJump = true;
            return true;
        }
        else
            return false;
    }

    private void Land()
    {
        if (IsJumping)
        {
            IsJumping = false;
            StartCoroutine(JumpCooldown());
        }
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        Debug.Log("Can Jump");
        canJump = true;
    }
}
