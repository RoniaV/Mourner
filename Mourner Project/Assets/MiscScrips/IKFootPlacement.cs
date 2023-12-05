using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootPlacement : MonoBehaviour 
{
    [SerializeField] LayerMask walkableLayers;
    [SerializeField] Transform leftFoot;
    [SerializeField] Transform rightFoot;
    [Tooltip("Maximum local altitude the body can be")]
    [SerializeField] float maxBodyHeight = -0.1f;
    [Tooltip("Maximum distance can be between feets")]
    [SerializeField] float maxFootDistance = 0.5f;
    [Tooltip("Distance from where the foot transform is to the lowest possible position of the foot.")]
    [Range (0, 1f)]
    [SerializeField] float distanceToGround = 0.1f;
    [SerializeField] float bodySmoothTime = 0.5f;

    Animator anim;

    private CheckGrounded leftFootGrounded;
    private CheckGrounded rightFootGrounded;
    private Vector3 bodyCV = Vector3.zero;

    void Awake() 
    {
        anim = GetComponent<Animator>();
        leftFootGrounded = new CheckGrounded(leftFoot, walkableLayers, distanceToGround * 2, distanceToGround);
        rightFootGrounded = new CheckGrounded(rightFoot, walkableLayers, distanceToGround * 2, distanceToGround);
    }

    private void OnAnimatorIK(int layerIndex) 
    { 
        // Only carry out the following code if there is an Animator set.
        if (anim) 
        {
            //Get weights
            float leftFootWeight = anim.GetFloat("IKLeftFootWeight");
            float rightFootWeight = anim.GetFloat("IKRightFootWeight");

            RaycastHit hit;

            //If feet are already touching the ground apply IK...
            Physics.SphereCast(
                leftFoot.position + Vector3.up * maxFootDistance,
                distanceToGround,
                Vector3.down,
                out hit,
                maxFootDistance + distanceToGround * 2,
                walkableLayers);
            if (leftFoot.position.y < hit.point.y || Mathf.Abs(leftFoot.position.y - hit.point.y) <= distanceToGround)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
            }
            //... else set the weights of left and right feet to the current value defined by the curve in animations.
            else
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            }

            Physics.SphereCast(
                rightFoot.position + Vector3.up * maxFootDistance,
                distanceToGround,
                Vector3.down,
                out hit,
                maxFootDistance + distanceToGround * 2,
                walkableLayers);
            if (rightFoot.position.y < hit.point.y || Mathf.Abs(rightFoot.position.y - hit.point.y) <= distanceToGround)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
            }
            else
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeight);
            }

            // Left Foot
            
            // We cast our ray from above the foot in case the current terrain/floor is above the foot position.
            Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up * maxFootDistance, Vector3.down);
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

            Vector3 leftFootDesiredPos = Vector3.zero;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, walkableLayers))
            {
                // The desired foot position is where the raycast hit a walkable object...
                leftFootDesiredPos = hit.point;
                // ... taking account the distance to the ground we added above.
                leftFootDesiredPos.y += distanceToGround;
                anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootDesiredPos);
                Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
                anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(forward, hit.normal));
            }

            // Right Foot
            ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up * maxFootDistance, Vector3.down);
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

            Vector3 rightFootDesiredPos = Vector3.zero;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, walkableLayers)) 
            {
                rightFootDesiredPos = hit.point;
                rightFootDesiredPos.y += distanceToGround;
                anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFootDesiredPos); 
                Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
                anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(forward, hit.normal));
            }


            //If any of the desired distances is inside of the body range:
            if (
                leftFootDesiredPos.y > transform.position.y - maxFootDistance ||
                rightFootDesiredPos.y > transform.position.y - maxFootDistance
                )
            {
                //If distance between feets are lower than the maximum distance...
                if (Mathf.Abs(leftFootDesiredPos.y - rightFootDesiredPos.y) < maxFootDistance)
                {
                    //... sets body position on the lowest foot.
                    Vector3 fixedPosition = transform.position;

                    if (leftFootDesiredPos.y <= rightFootDesiredPos.y)
                        fixedPosition.y = leftFootDesiredPos.y;
                    else
                        fixedPosition.y = rightFootDesiredPos.y;

                    transform.position = 
                        Vector3.SmoothDamp(transform.position, fixedPosition, ref bodyCV, bodySmoothTime);
                }
                //Else if the distance is higher...
                else
                {
                    //... sets body position on the highest foot.
                    Vector3 fixedPosition = transform.position;

                    if (leftFootDesiredPos.y >= rightFootDesiredPos.y)
                        fixedPosition.y = leftFootDesiredPos.y;
                    else
                        fixedPosition.y = rightFootDesiredPos.y;

                    transform.position = 
                        Vector3.SmoothDamp(transform.position, fixedPosition, ref bodyCV, bodySmoothTime);
                }
            }


            Vector3 localPos = transform.localPosition;
            //Body position can not be higher than its maximum defined height
            if (localPos.y > maxBodyHeight)
                localPos.y = maxBodyHeight;
            //Neither can be lower than it's maximum position less feet maximum distance
            //else if(localPos.y < maxBodyHeight - maxFootDistance)
            //    localPos.y = maxBodyHeight - maxFootDistance;
            transform.localPosition = localPos;
        }
    }

    void OnDrawGizmosSelected()
    {
        
    }

}
