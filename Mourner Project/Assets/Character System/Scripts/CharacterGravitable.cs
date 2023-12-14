using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(CharacterMovement))]
public class CharacterGravitable : MonoBehaviour
{
    public bool IsGrounded { get; private set; }

    [SerializeField] LayerMask groundLayer;

    private CharacterController characterController;
    private CharacterMovement characterMovement;
    private CheckGrounded grounded;

    private float characterVelocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        characterMovement = GetComponent<CharacterMovement>();
        grounded = new CheckGrounded(transform, groundLayer, characterController.skinWidth + 0.05f, characterController.radius);
    }

    void FixedUpdate()
    {
        IsGrounded = grounded.IsGrounded();

        if (IsGrounded && characterVelocity < 0)
            characterVelocity = 0;
        
        characterVelocity += -9.8f * Time.fixedDeltaTime;
        characterMovement.SetVerticalVelocity(characterVelocity * Time.fixedDeltaTime);
    }

    void OnDrawGizmos()
    {
        grounded?.DrawSphereGizmos();
    }

    public void AddVerticalVelocity(float velocity)
    {
        characterVelocity += velocity;
        characterMovement.ResetVerticalVelocity();
    }

    public RaycastHit GetGroundHit()
    {
        return grounded.GetLastHit();
    }
}
