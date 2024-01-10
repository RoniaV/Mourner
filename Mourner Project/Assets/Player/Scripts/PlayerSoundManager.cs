using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSounds
{
    Jump,
    Land
}

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landSound;
    [Header("Footstep Settings")]
    [SerializeField] AudioClip[] footstepSounds;
    [SerializeField] float ratio = 0.25f;
    [SerializeField] float originalVel = 1.8f;

    AudioSource audioSource;

    private float normalizedRatio { get { return (ratio * 1000) / 60; } }

    private bool footstepPlaying = false;
    private float actualRatio;
    private float actualVel;
    private float stepTimer;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(PlayerSounds soundType)
    {
        switch(soundType)
        {
            case PlayerSounds.Jump:
                audioSource.PlayOneShot(jumpSound);
                break;
            case PlayerSounds.Land:
                audioSource.PlayOneShot(landSound);
                break;
            default:
                Debug.Log("Non existent Player Sound");
                break;
        }
    }

    public void PlayFootstepSound()
    {
        if (!footstepPlaying)
            StartCoroutine(FootstepCoroutine());

        footstepPlaying = true;
    }

    public void StopFootstepSound()
    {
        StopAllCoroutines();
        footstepPlaying = false;
        stepTimer = 0;
    }

    public void SetActualVelocity(float actualVel)
    {
        this.actualVel = actualVel;
    }

    private IEnumerator FootstepCoroutine()
    {
        while (true)
        {
            Debug.Log("Play footstep sound");
            int randomIndex = Random.Range(0, footstepSounds.Length);
            audioSource.PlayOneShot(footstepSounds[randomIndex]);

            do
            {
                stepTimer += Time.deltaTime;

                actualRatio = (actualVel * originalVel) / ratio;
                Debug.Log("Actual ratio: " + actualRatio);

                yield return null;
            } while (stepTimer < actualRatio);

            stepTimer = 0;
        }
    }
}
