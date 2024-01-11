using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionChanger : MonoBehaviour
{
    [SerializeField] Animator lightAnimator;
    [SerializeField] float backTime = 3f;

    void OnEnable()
    {
        BellManager.OnBellRing += SetVision;
    }

    void OnDisable()
    {
        BellManager.OnBellRing -= SetVision;
    }

    private void SetVision()
    {
        lightAnimator.SetBool("Ring", true);

        StopAllCoroutines();
        StartCoroutine(BackToIdleCoroutine());
    }

    private IEnumerator BackToIdleCoroutine()
    {
        yield return new WaitForSeconds(backTime);
        lightAnimator.SetBool("Ring", false);
    }
}
