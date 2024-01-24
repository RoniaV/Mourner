using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SetClipSpace : MonoBehaviour
{
    [SerializeField] float radius = 3;

    private bool on = false;

    void OnEnable()
    {
        on = true;
        Shader.SetGlobalFloat("_toggle", 1);
    }

    void Start()
    {
        Shader.SetGlobalFloat("_radius", radius);
    }

    void OnDisable()
    {
        on = false;
        Shader.SetGlobalFloat("_toggle", 0);
    }

    void Update()
    {
        Shader.SetGlobalVector("_worldCenter", (Vector4)transform.position);

#if UNITY_EDITOR
        Shader.SetGlobalFloat("_radius", radius);
#endif
    }

    public void SetRadius(float radius)
    {
        this.radius = radius;
        Shader.SetGlobalFloat("_radius", radius);
    }

    void OnDrawGizmosSelected()
    {
        if (on)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
