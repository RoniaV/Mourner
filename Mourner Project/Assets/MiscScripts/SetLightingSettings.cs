using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightingSettings : MonoBehaviour
{
    [SerializeField] GameObject groundDirectionalLight;

    void Awake()
    {
        groundDirectionalLight.SetActive(false);
        RenderSettings.ambientSkyColor = Color.black;
    }
}
