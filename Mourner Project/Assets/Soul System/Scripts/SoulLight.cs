using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SoulLight : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        VisionChanger.OnTurnOn += () => animator.SetBool("On", true);
        VisionChanger.OnTurnOff += () => animator.SetBool("On", false);
    }

    void OnDisable()
    {
        VisionChanger.OnTurnOn -= () => animator.SetBool("On", true);
        VisionChanger.OnTurnOff -= () => animator.SetBool("On", false);
    }
}
