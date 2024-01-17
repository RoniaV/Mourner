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
            clipSpace.enabled = true;
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
            clipSpace.enabled = false;
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
