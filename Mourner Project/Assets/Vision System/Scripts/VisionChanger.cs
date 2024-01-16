using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionChanger : MonoBehaviour
{
    public static event Action OnTurnOn;
    public static event Action OnTurnOff;

    public bool On { get; private set; }

    [SerializeField] Animator lightAnimator;
    [SerializeField] float backTime = 3f;
    [SerializeField] SetClipSpace clipSpace;
    [SerializeField] Camera enviromentCamera;
    [SerializeField] string soulLayer = "Soul Light";

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
        if(!On)
        {
            lightAnimator.SetBool("Ring", true);
            clipSpace.enabled = true;
            //enviromentCamera.cullingMask |= 1 << LayerMask.NameToLayer(soulLayer);
            OnTurnOn?.Invoke();
        }

        StopAllCoroutines();
        StartCoroutine(BackToIdleCoroutine());
    }

    private IEnumerator BackToIdleCoroutine()
    {
        yield return new WaitForSeconds(backTime);

        lightAnimator.SetBool("Ring", false);
        clipSpace.enabled = false;
        //enviromentCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(soulLayer));
        OnTurnOff?.Invoke();
    }
}
