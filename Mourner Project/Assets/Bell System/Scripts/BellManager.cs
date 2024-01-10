using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BellManager : MonoBehaviour
{
    public static event Action OnBellRing;

    [SerializeField] HandIK handIK;
    [SerializeField] CheckCollisionForce checkCollision;
    [SerializeField] float movementSensitivity = 1f;
    [Header("Hand Settings")]
    [SerializeField] Transform handGoal;
    [SerializeField] float maxHandMovement = 0.75f;
    [SerializeField] float forwardMovMultiplier = 0.5f;
    [Header("Bell Settings")]
    [SerializeField] Transform bell;
    [SerializeField] Rigidbody bellCyllinderRB;
    [Header("Ring Settings")]
    [SerializeField] AudioClip[] bellSounds;

    AudioSource audioSource;

    private bool handOut = false;
    private Vector3 originalHandPosition;
    private Vector3 handDirection;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        originalHandPosition = handGoal.localPosition;

        bellCyllinderRB.centerOfMass = new Vector3(0, -0.26f, 0);
        bellCyllinderRB.isKinematic = true;
        bellCyllinderRB.transform.parent = bell;
    }

    void OnEnable()
    {
        checkCollision.OnCollisionDone += BellCollision;
    }

    void OnDisable()
    {
        checkCollision.OnCollisionDone -= BellCollision;
    }

    void Update()
    {
        if (handOut)
        {
            //Get local position
            Vector3 handLocalPosition = handGoal.localPosition;
            handLocalPosition.x += handDirection.x * movementSensitivity * Time.deltaTime;
            handLocalPosition.y += handDirection.y * movementSensitivity * Time.deltaTime;
            handLocalPosition.z += handDirection.z * movementSensitivity * Time.deltaTime;

            //Clamp the positon
            handLocalPosition.x = 
                Mathf.Clamp(handLocalPosition.x, originalHandPosition.x - maxHandMovement, originalHandPosition.x + maxHandMovement);

            handLocalPosition.y =
                Mathf.Clamp(handLocalPosition.y,
                originalHandPosition.y - maxHandMovement * forwardMovMultiplier,
                originalHandPosition.y + maxHandMovement * forwardMovMultiplier);

            handLocalPosition.z =
                Mathf.Clamp(handLocalPosition.z,
                originalHandPosition.z - maxHandMovement * forwardMovMultiplier,
                originalHandPosition.z + maxHandMovement * forwardMovMultiplier);

            handGoal.localPosition = handLocalPosition;
        }
    }

    public void PutHandOut()
    {
        Debug.Log("PutHandOut");

        handOut = true;
        handIK.SetWeightGoal(1f);

        bellCyllinderRB.isKinematic = false;
        bellCyllinderRB.transform.parent = null;
    }

    public void PutHandIn()
    {
        Debug.Log("PutHandIn");

        handOut = false;
        handIK.SetWeightGoal(0f);

        bellCyllinderRB.isKinematic = true;
        bellCyllinderRB.transform.parent = bell;
    }

    public void MoveHand(Vector2 direction)
    {
        if(!handOut) return;

        handDirection.x = direction.x;
        handDirection.y = direction.y * forwardMovMultiplier;
        handDirection.z = direction.y * forwardMovMultiplier;
    }

    private void BellCollision()
    {
        int randomIndex = UnityEngine.Random.Range(0, bellSounds.Length);
        audioSource.clip = bellSounds[randomIndex];
        audioSource.Play();

        OnBellRing?.Invoke();
    }

    #region Test
    //private PlayerControls playerControls;
    
    //void Awake()
    //{
    //    playerControls = new PlayerControls();
    //}

    //void Update()
    //{
    //    if(playerControls.Gameplay.BellOut.WasPressedThisFrame())
    //    {
    //        PutHandOut();
    //    }
    //    else if(playerControls.Gameplay.BellOut.WasReleasedThisFrame())
    //    {
    //        PutHandIn();
    //    }
    //}

    //void OnEnable()
    //{
    //    playerControls.Enable();
    //}

    //void OnDisable()
    //{
    //    playerControls.Disable();
    //}
    #endregion
}
