using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseStairs : MonoBehaviour
{
    [SerializeField] LayerMask walkableLayers;

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f, walkableLayers))
        {
            Debug.Log(hit.collider.bounds.max);
        }
    }
}
