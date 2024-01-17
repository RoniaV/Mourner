using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentAtAwake : MonoBehaviour
{
    void Awake()
    {
        transform.parent = null;
    }
}
