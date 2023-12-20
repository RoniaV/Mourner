using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterCrouch : MonoBehaviour
{
    public bool Crouched { get; protected set; }

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

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || !Crouched)
            return;

        if (charController == null)
            return;

        // Define the ray's origin and direction
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector3.up;

        // Calculate the ray length (original height plus skin width)
        float rayLength = originalHeight + charController.skinWidth;

        // Perform the raycast
        RaycastHit hit;
        bool isHit = Physics.Raycast(rayOrigin, rayDirection, out hit, rayLength, obstacleLayers);

        // Choose the color based on whether the raycast hits an obstacle
        Gizmos.color = isHit ? Color.red : Color.green;

        // Draw the ray
        Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection * rayLength);

        // If there's a hit, draw a sphere at the hit point
        if (isHit)
        {
            Gizmos.DrawWireSphere(hit.point, 0.1f);
            Gizmos.DrawLine(hit.point, hit.point + hit.normal * 0.5f); // Draw normal
        }
    }
}
