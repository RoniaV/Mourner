using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLookAtIK : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] float lookAtDistance = 100;
    [SerializeField] Transform body;
    [SerializeField] float weightMultiplier = 1;
    [SerializeField] float weightOffset = 0.2f;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (anim == null) return;

        Transform head = anim.GetBoneTransform(HumanBodyBones.Head);
        if (head == null) return;

        //Set head look at position
        Vector3 lookAt = playerCamera.position + playerCamera.forward * lookAtDistance;
        anim.SetLookAtPosition(lookAt);

        //Set head look at weight
        float dot = Vector3.Dot(body.forward, playerCamera.forward);
        float weight = Mathf.Clamp01((dot + weightOffset) * weightMultiplier);
        Debug.Log(weight);
        anim.SetLookAtWeight(weight);
    }
}
