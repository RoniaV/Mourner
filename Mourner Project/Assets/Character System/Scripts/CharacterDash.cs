using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterDash : MonoBehaviour
{
    public bool isDashing;

    [SerializeField] float braking = -3;
    [SerializeField] float dashLenght = 4;
    [SerializeField] float dashMinimumSpeed = 2;

    private CharacterController characterController;

    private float acceleration;
    private Vector3 dashDirection;

    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    protected virtual void FixedUpdate()
    {
        if(isDashing)
        {
            acceleration += braking * Time.fixedDeltaTime;

            if (acceleration <= dashMinimumSpeed)
                StopDash();

            characterController.Move(dashDirection * acceleration * Time.fixedDeltaTime);
        }

    }

    public void Dash(Vector3 direction)
    {
        if (!isDashing)
        {
            acceleration += Mathf.Sqrt(dashLenght * -braking);
            dashDirection = direction;

            isDashing = true;

            Debug.Log("Start Dashing");
        }
    }

    public void StopDash()
    {
        acceleration = 0;
        isDashing = false;

        Debug.Log("Stop Dashing");
    }
}
