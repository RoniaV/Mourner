using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(CharacterGravitable))]
public class CharacterJump : MonoBehaviour
{
    [SerializeField] float jumpHeight = 4;

    private CharacterGravitable characterGravitable;

    protected bool doJump;

    protected virtual void Awake()
    {
        characterGravitable = GetComponent<CharacterGravitable>();
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
                Debug.Log("Jump");
            }
        }
    }

    public void Jump()
    {
        doJump = true;
    }
}
