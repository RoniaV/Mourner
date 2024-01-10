using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CheckCollisionForce : MonoBehaviour
{
    public event Action OnCollisionDone;

    [SerializeField] float collisionForceThreshold = 0.1f;


    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > collisionForceThreshold)
        {
            //Debug.Log("OnCollisionEnter");
            //Debug.Log(collision.relativeVelocity.magnitude);
            OnCollisionDone?.Invoke();
        }
    }
}
