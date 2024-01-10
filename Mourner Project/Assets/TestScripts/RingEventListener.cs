using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEventListener : MonoBehaviour
{
    void OnEnable()
    {
        BellManager.OnBellRing += ListenRing;
    }

    void OnDisable()
    {
        BellManager.OnBellRing -= ListenRing;
    }

    private void ListenRing()
    {
        Debug.Log("OwO I heard it!");
    }
}
