using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterFloorMovement), typeof(CharacterAim))]
public class NavMeshCharacter : MonoBehaviour
{
    public event Action OnEndOfPathReached;

    [Tooltip("Minimum distance to the current corner, before choosing the next one")]
    [SerializeField] float nextCornerDistance = 0.2f;
    [SerializeField] float closestPointMaxDistance = 5;
    [Header("Avoid Settings")]
    [SerializeField] bool avoidOtherCharacters = true;
    [SerializeField] float avoidRadius = 1;
    [SerializeField] LayerMask charactersLayers;
    
    //[Header("Debug Settings")]
    //[SerializeField] Transform testTarget;
    //[SerializeField] bool setTarget;
    

    CharacterFloorMovement characterFloorMovement;
    CharacterAim characterAim;
    CharacterController characterController;

    private NavMeshPath actualPath;
    private int cornerIndex;
    private Transform aimTarget;

    void Awake()
    {
        characterFloorMovement = GetComponent<CharacterFloorMovement>();
        characterAim = GetComponent<CharacterAim>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        //if(setTarget)
        //{
        //    SetNewTarget(testTarget);
        //    setTarget = false;
        //}

        characterFloorMovement.SetMovementDirection(GetMovementDirection());
        characterAim.RotateCharacter(GetAimInput());
    }

    public void SetNewTarget(Transform target, bool findClosest = false)
    {
        NavMeshPath newPath = new NavMeshPath();

        if (target != null && NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, newPath))
            actualPath = newPath;
        else if (findClosest && target != null)
        {
            actualPath = null;

            NavMeshHit startHit;
            NavMeshHit endHit;

            if (NavMesh.SamplePosition(transform.position, out startHit, closestPointMaxDistance, NavMesh.AllAreas) &&
                NavMesh.SamplePosition(target.position, out endHit, closestPointMaxDistance, NavMesh.AllAreas))
            {
                if (NavMesh.CalculatePath(startHit.position, endHit.position, NavMesh.AllAreas, newPath))
                    actualPath = newPath;
            }
        }
        else
            actualPath = null;

        cornerIndex = 0;
    }

    public void SetNewTarget(Vector3 targetPoint, bool findClosest)
    {
        NavMeshPath newPath = new NavMeshPath();

        if (targetPoint != Vector3.zero && NavMesh.CalculatePath(transform.position, targetPoint, NavMesh.AllAreas, newPath))
        {
            Debug.Log("Path calculated");
            actualPath = newPath;
        }
        else if (findClosest && targetPoint != Vector3.zero)
        {

            Debug.Log("Find closest points");
            actualPath = null;

            NavMeshHit startHit;
            NavMeshHit endHit;

            if (NavMesh.SamplePosition(transform.position, out startHit, closestPointMaxDistance, NavMesh.AllAreas) &&
                NavMesh.SamplePosition(targetPoint, out endHit, closestPointMaxDistance, NavMesh.AllAreas))
            {
                if (NavMesh.CalculatePath(startHit.position, endHit.position, NavMesh.AllAreas, newPath))
                    actualPath = newPath;
            }
        }
        else
            actualPath = null;

        //if (actualPath != null)
        //    Debug.Log(actualPath.corners.Length);

        cornerIndex = 0;
    }

    public void SetAimTarget(Transform target)
    {
        aimTarget = target;
    }

    public Vector2 GetAimInput()
    {
        Quaternion objetiveRotation;

        if(aimTarget != null)
        {
            Vector3 aimDirection = (aimTarget.position - transform.position).normalized;
            objetiveRotation = Quaternion.LookRotation(aimDirection);
        }
        else if (GetMovementDirection() != Vector3.zero)
            objetiveRotation = Quaternion.LookRotation(GetMovementDirection());
        else
            objetiveRotation = transform.rotation;

        float ownRot = transform.eulerAngles.y;
        if (ownRot > 180)
            ownRot = ownRot - 360;
        
        float angle = objetiveRotation.eulerAngles.y - transform.eulerAngles.y;
        if (angle > 180)
            angle = angle - 360f;

        return new Vector2(angle, 0);
    }

    public Vector3 GetMovementDirection()
    {
        Vector3 nextCornerDirection = Vector3.zero;

        if (cornerIndex < actualPath?.corners.Length)
        {
            nextCornerDirection = (actualPath.corners[cornerIndex] - transform.position).normalized;

            if (avoidOtherCharacters)
                nextCornerDirection = GetAvoidDirection(nextCornerDirection);

            if (CheckNextCornerCloseness())
            {
                SetNextCorner();
            }
        }

        return nextCornerDirection;
    }

    private Vector3 GetAvoidDirection(Vector3 desiredDirection)
    {
        Vector3 capsulePointA = new Vector3(
            transform.position.x,
            transform.position.y + characterController.height,
            transform.position.z
            );
        Vector3 capsulePointB = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z
            );

        Collider[] charactersInRange =
        Physics.OverlapCapsule( capsulePointA, capsulePointB,  avoidRadius, charactersLayers);        

        if (charactersInRange != null)
        {
            Vector3 avoidDirection = desiredDirection;

            foreach (Collider c in charactersInRange)
            {
                Vector3 dir = (transform.position - c.transform.position);
                dir.y = 0;
                dir = dir.normalized;
                Debug.DrawRay(transform.position, dir, Color.yellow);

                avoidDirection += dir / charactersInRange.Length;
            }

            avoidDirection = avoidDirection.normalized;

            return avoidDirection;
        }
        else
            return desiredDirection;
    }

    private bool CheckNextCornerCloseness()
    {
        return Vector3.Distance(transform.position, actualPath.corners[cornerIndex]) < nextCornerDistance;
    }

    private void SetNextCorner()
    {
        cornerIndex++;

        if (cornerIndex >= actualPath.corners.Length && OnEndOfPathReached != null)
            OnEndOfPathReached();
    }

    private void OnDrawGizmosSelected()
    {
        if(actualPath != null)
        {
            for(int i = 0; i < actualPath.corners.Length; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(actualPath.corners[i], 0.1f);

                if (i + 1 < actualPath.corners.Length)
                    Gizmos.DrawLine(actualPath.corners[i], actualPath.corners[i + 1]);
                Gizmos.color = Color.white;
            }
        }
    }
}
