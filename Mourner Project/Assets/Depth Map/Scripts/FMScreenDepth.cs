using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent (typeof(Camera))]
public class FMScreenDepth : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Material mat;

    void Awake()
    {
        if (cam == null)
            cam = GetComponent<Camera>();

        cam.depthTextureMode = DepthTextureMode.DepthNormals;

        if(mat == null)
        {
            //Assign shader "Hidden/FMScreenDepthNormal" to Mat
            mat = new Material(Shader.Find("FMScreenDepthNormal"));
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Render source to screen
        //Graphics.Blit (source, destination);

        //Render source to screen with shader
        if (mat != null)
            Graphics.Blit(source, destination, mat);
    }
}
