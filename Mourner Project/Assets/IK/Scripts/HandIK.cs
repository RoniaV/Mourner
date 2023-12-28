using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HandIK : MonoBehaviour
{
    [SerializeField] Transform handGoal;
    [SerializeField] float armSpeed = 1f;

    Animator animator;

    private float weightGoal = 0f;
    private float weight = 0f;
    private float weightCurrentVel = 0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        animator.SetIKPosition(AvatarIKGoal.RightHand, handGoal.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, handGoal.rotation);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);

        //Debug.Log("Hand IK Weight: " + weight);
    }

    public void SetWeightGoal(float goal)
    {
        weightGoal = goal;

        StopAllCoroutines();
        StartCoroutine(MoveHand());
    }

    private IEnumerator MoveHand()
    {
        while (Mathf.Abs(weightGoal - weight) > 0.001f)
        {
            weight = Mathf.SmoothDamp(weight, weightGoal, ref weightCurrentVel, armSpeed);

            yield return null;
        }

        weight = weightGoal;
    }
}
