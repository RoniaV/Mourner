using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CheckCollisionForce : MonoBehaviour
{
    [SerializeField] float collisionForceThreshold = 0.1f;
    [SerializeField] AudioClip[] bellSounds;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > collisionForceThreshold)
        {
            Debug.Log("OnCollisionEnter");
            Debug.Log(collision.relativeVelocity.magnitude);

            int randomIndex = Random.Range(0, bellSounds.Length);
            audioSource.clip = bellSounds[randomIndex];
            audioSource.Play();
        }
    }
}
