using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterCrouch : MonoBehaviour
{
    public bool Crouched { get; protected set; }

    [SerializeField] float crouchedVelocity = 1;
    [SerializeField] float crouchHeight = 1;
    [SerializeField] float cooldown = 0.5f;
    [SerializeField] LayerMask obstacleLayers;

    protected CharacterController charController;

    protected float originalHeight;
    protected bool canCrouch = true;

    protected virtual void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    protected virtual void Start()
    {
        originalHeight = charController.height;
    }

    public bool Crouch()
    {
        if (canCrouch)
        {
            if(Crouched)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.up, out hit, originalHeight + charController.skinWidth, obstacleLayers))
                {
                    Debug.Log("Hit: " + hit.collider.name);
                    return false;
                }
            }

            ChangeCrouchState();
            return true;
        }
        else
            return false;
    }

    private IEnumerator CrouchCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canCrouch = true;
    }

    private void ChangeCrouchState()
    {
        canCrouch = false;
        Crouched = !Crouched;

        charController.height = Crouched ? crouchHeight : originalHeight;
        charController.center = new Vector3(0, charController.height / 2, 0);
        StartCoroutine(CrouchCooldown());
    }
}
