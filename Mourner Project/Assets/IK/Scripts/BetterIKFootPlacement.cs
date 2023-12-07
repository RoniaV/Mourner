using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterIKFootPlacement : MonoBehaviour
{
    [SerializeField] LayerMask walkableLayers;
    [SerializeField] Transform leftFoot;
    [SerializeField] Transform rightFoot;
    [Tooltip("Maximum local altitude the body can be")]
    [SerializeField] float maxBodyHeight = -0.1f;
    [Tooltip("Maximum distance can be between feets")]
    [SerializeField] float maxFootDistance = 0.5f;
    [Tooltip("Distance from where the foot transform is to the lowest possible position of the foot.")]
    [Range(0, 1f)]
    [SerializeField] float distanceToGround = 0.1f;
    [SerializeField] float bodySmoothTime = 0.5f;

    private Animator anim;
    private float bodyCV = 0;
    private float lastBodyHeight = 0;
    private Vector3 lastLeftRPos = Vector3.zero;

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

        UpdateFootIK(AvatarIKGoal.LeftFoot, leftFoot);
        UpdateFootIK(AvatarIKGoal.RightFoot, rightFoot);
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

    private void UpdateFootIK(AvatarIKGoal goal, Transform foot)
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
            anim.SetIKPosition(goal, footDesiredPos);
            Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
            anim.SetIKRotation(goal, Quaternion.LookRotation(forward, hit.normal));
        }
    }

    void OnDrawGizmosSelected()
    {
        // Your Gizmos drawing code (if needed)
    }
}
