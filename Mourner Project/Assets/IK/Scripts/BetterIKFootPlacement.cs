using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class BetterIKFootPlacement : MonoBehaviour
{
    [SerializeField] LayerMask walkableLayers;
    [SerializeField] Transform leftFoot;
    [SerializeField] Transform rightFoot;
    [Header("Feet Settings")]
    [Tooltip("Maximum distance can be between feets")]
    [SerializeField] float maxFootDistance = 0.5f;
    [Tooltip("Distance from where the foot transform is to the lowest possible position of the foot.")]
    [Range(0, 1f)]
    [SerializeField] float distanceToGround = 0.1f;
    [SerializeField] float footSmoothTime = 0.2f;
    [Header("Body Settings")]
    [Tooltip("Maximum local altitude the body can be")]
    [SerializeField] float maxBodyHeight = -0.1f;
    [SerializeField] float bodySmoothTime = 0.5f;

    private Animator anim;
    private float bodyCV = 0;

    private float leftFootRefVel = 0;
    private float rightFootRefVel = 0;
    private Vector3 leftDesiredPos = Vector3.zero;
    private Vector3 rightDesiredPos = Vector3.zero;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (anim == null) return;

        UpdateFootIK(AvatarIKGoal.LeftFoot, leftFoot, ref leftDesiredPos, ref leftFootRefVel);
        UpdateFootIK(AvatarIKGoal.RightFoot, rightFoot, ref rightDesiredPos, ref rightFootRefVel);
    }

    void Update()
    {
        // Convert maxBodyHeight to world space
        float worldMaxBodyHeight = transform.root.position.y + maxBodyHeight;

        // Determine the target body height based on the lowest foot desired position
        float lowestFootY = Mathf.Min(leftDesiredPos.y, rightDesiredPos.y);
        //Debug.Log("Lowest Foot Y: " + lowestFootY);

        bool isInRange = IsFootPositionInRange();
        //Debug.Log("Is In Range: " + isInRange);

        // Check if the lowest foot position is within the range
        float targetBodyHeight;
        if (isInRange)
        {
            targetBodyHeight = lowestFootY;
        }
        else
        {
            targetBodyHeight = worldMaxBodyHeight;
        }
        //Debug.Log("Target Body Height: " + targetBodyHeight);

        // Smoothly adjust the current body height towards the target body height
        float smoothHeight = 
            Mathf.SmoothDamp(transform.position.y, targetBodyHeight, ref bodyCV, bodySmoothTime);
        transform.position = new Vector3(transform.position.x, smoothHeight, transform.position.z);

        // Ensure local position is based on the foot positions
        Vector3 localPos = transform.localPosition;
        localPos.y = Mathf.Clamp(localPos.y, maxBodyHeight - maxFootDistance, maxBodyHeight);
        transform.localPosition = localPos;
    }

    private void UpdateFootIK(AvatarIKGoal goal, Transform foot, ref Vector3 desiredPos, ref float footVel)
    {
        float footWeight = anim.GetFloat($"IK{goal}Weight");
        RaycastHit hit;

        if (Physics.SphereCast(foot.position + Vector3.up * maxFootDistance, distanceToGround, Vector3.down, out hit, maxFootDistance + distanceToGround * 2, walkableLayers) &&
            (foot.position.y < hit.point.y || Mathf.Abs(foot.position.y - hit.point.y) <= distanceToGround))
        {
            anim.SetIKPositionWeight(goal, 1);
            anim.SetIKRotationWeight(goal, 1);
        }
        else
        {
            anim.SetIKPositionWeight(goal, footWeight);
            anim.SetIKRotationWeight(goal, footWeight);
        }

        Ray ray = new Ray(anim.GetIKPosition(goal) + Vector3.up * maxFootDistance, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, walkableLayers))
        {
            desiredPos = hit.point + Vector3.up * distanceToGround;
            Vector3 footSmoothPos = desiredPos;            
            footSmoothPos.y = Mathf.SmoothDamp(foot.position.y, desiredPos.y, ref footVel, footSmoothTime);
            anim.SetIKPosition(goal, desiredPos);
            //Debug.Log($"{goal} Smooth Pos: " + footSmoothPos);

            Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
            anim.SetIKRotation(goal, Quaternion.LookRotation(forward, hit.normal));
        }
    }

    private bool IsFootPositionInRange()
    {
        // Convert maxBodyHeight to world space
        float worldMaxBodyHeight = transform.root.position.y + maxBodyHeight;

        float worldMinBodyHeight = worldMaxBodyHeight - maxFootDistance;

        // Determine the lowest foot's world y-coordinate
        float lowestFootY = Mathf.Min(leftDesiredPos.y, rightDesiredPos.y);

        // Check if the lowest foot position is within the body's height range
        return lowestFootY >= worldMinBodyHeight && lowestFootY <= worldMaxBodyHeight;
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            return;

        // Convert maxBodyHeight and minBodyHeight (maxBodyHeight - maxFootDistance) to world space
        float worldMaxBodyHeight = transform.root.position.y + maxBodyHeight;
        float worldMinBodyHeight = worldMaxBodyHeight - maxFootDistance;

        // Start and end points for the line in world space
        Vector3 lineStart = new Vector3(transform.position.x, worldMaxBodyHeight, transform.position.z);
        Vector3 lineEnd = new Vector3(transform.position.x, worldMinBodyHeight, transform.position.z);

        // Set the color of the Gizmos line
        Gizmos.color = Color.green;

        // Draw the line
        Gizmos.DrawLine(lineStart, lineEnd);

        // Optionally, draw small spheres at the start and end points for better visibility
        Gizmos.DrawSphere(lineStart, 0.01f); // Size of the sphere can be adjusted
        Gizmos.DrawSphere(lineEnd, 0.01f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftDesiredPos, 0.05f);
        Gizmos.DrawSphere(rightDesiredPos, 0.05f);
    }
}
