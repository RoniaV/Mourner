using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = Vector3.zero;
    [SerializeField] bool localOffset = true;

    void LateUpdate()
    {
        Vector3 followPos = localOffset ? target.TransformPoint(offset) : target.position + offset;
        transform.position = followPos;
    }
}
