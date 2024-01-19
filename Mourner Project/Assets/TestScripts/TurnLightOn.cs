using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TurnLightOn : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float distance;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        BellManager.OnBellRing += () => CheckLight();
    }

    void OnDisable()
    {
        BellManager.OnBellRing -= () => CheckLight();
    }

    private void CheckLight()
    {
        if(Vector3.Distance(transform.position, player.position) < distance)
            animator.SetBool("On", true);
    }
}
