using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float lastBodyHeight = 0;

    private Vector3 leftFootRefVel = Vector3.zero;
    private Vector3 rightFootRefVel = Vector3.zero;
    private Vector3 leftDesiredPos = Vector3.zero;
    private Vector3 rightDesiredPos = Vector3.zero;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        lastBodyHeight = transform.position.y;
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (anim == null) return;

        transform.position = new Vector3(transform.position.x, lastBodyHeight, transform.position.z); ;

        UpdateFootIK(AvatarIKGoal.LeftFoot, leftFoot, true);
        UpdateFootIK(AvatarIKGoal.RightFoot, rightFoot, false);
    }

    void LateUpdate()
    {
        // Calculate target body height based on the lowest foot position
        float targetBodyHeight = Mathf.Min(leftFoot.position.y, rightFoot.position.y);

        // Smoothly adjust the current body height towards the target body height
        float smoothHeight = 
            Mathf.SmoothDamp(transform.position.y, targetBodyHeight - distanceToGround, ref bodyCV, bodySmoothTime);
        transform.position = new Vector3(transform.position.x, smoothHeight, transform.position.z);

        // Ensure local position is based on the foot positions
        Vector3 localPos = transform.localPosition;
        localPos.y = Mathf.Clamp(localPos.y, maxBodyHeight - maxFootDistance, maxBodyHeight);
        transform.localPosition = localPos;

        lastBodyHeight = transform.position.y;
    }

    private void UpdateFootIK(AvatarIKGoal goal, Transform foot, bool left)
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
            Vector3 footDesiredPos = hit.point + Vector3.up * distanceToGround;
            if (left)
                leftDesiredPos = footDesiredPos;
            else
                rightDesiredPos = footDesiredPos;

            Vector3 refVel = left ? leftFootRefVel : rightFootRefVel;
            Vector3 footSmoothPos = Vector3.SmoothDamp(foot.position, footDesiredPos, ref refVel, footSmoothTime);
            anim.SetIKPosition(goal, footSmoothPos);
            Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
            anim.SetIKRotation(goal, Quaternion.LookRotation(forward, hit.normal));
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftDesiredPos, 0.05f);
        Gizmos.DrawSphere(rightDesiredPos, 0.05f);
    }
}
