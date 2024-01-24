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
    [SerializeField] Camera enviromentCamera;
    [SerializeField] string soulLayer = "Soul Light";
    [Header("Clip Space Settings")]
    [SerializeField] SetClipSpace clipSpace;
    [SerializeField] float lightOnRadious = 3.5f;
    [SerializeField] float lightOffRadious = 1.5f;

    void OnEnable()
    {
        BellManager.OnBellRing += SetVision;
        BellManager.OnHandIn += BackToIdle;
    }

    void OnDisable()
    {
        BellManager.OnBellRing -= SetVision;
        BellManager.OnHandIn -= BackToIdle;
    }

    private void SetVision()
    {
        if(!On)
        {
            On = true;
            lightAnimator.SetBool("Ring", true);
            clipSpace.SetRadius(lightOnRadious);
            //clipSpace.enabled = true;
            //enviromentCamera.cullingMask |= 1 << LayerMask.NameToLayer(soulLayer);
            OnTurnOn?.Invoke();
        }

        //StopAllCoroutines();
        //StartCoroutine(BackToIdleCoroutine());
    }

    private void BackToIdle()
    {
        if(On)
        {
            On = false;
            lightAnimator.SetBool("Ring", false);
            clipSpace.SetRadius(lightOffRadious);
            //clipSpace.enabled = false;
            //enviromentCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(soulLayer));
            OnTurnOff?.Invoke();
        }
    }

    private IEnumerator BackToIdleCoroutine()
    {
        yield return new WaitForSeconds(backTime);

        BackToIdle();
    }
}
