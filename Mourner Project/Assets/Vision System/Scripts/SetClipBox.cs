using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SetClipBox : MonoBehaviour
{
    void Update()
    {
        Shader.SetGlobalVector("_WorldToBox", (Vector4)transform.position);
    }
}
