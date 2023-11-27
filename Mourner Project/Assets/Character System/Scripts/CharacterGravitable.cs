using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterGravitable : MonoBehaviour
{
    public bool isGrounded { get; private set; }

    [SerializeField] LayerMask groundLayer;

    private CharacterController characterController;
    private CheckGrounded grounded;

    private Vector3 characterVelocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        grounded = new CheckGrounded(transform, groundLayer);
    }

    void FixedUpdate()
    {
        isGrounded = grounded.IsGrounded();

        if (isGrounded && characterVelocity.y < 0)
            characterVelocity = Vector3.zero;
        
        characterVelocity.y += -9.8f * Time.fixedDeltaTime;
        characterController.Move(characterVelocity * Time.fixedDeltaTime);
    }

    void OnDrawGizmos()
    {
        grounded?.DrawSphereGizmos();
    }

    public void AddVerticalVelocity(float velocity)
    {
        characterVelocity.y += velocity;
    }

    public RaycastHit GetGroundHit()
    {
        return grounded.GetLastHit();
    }
}
