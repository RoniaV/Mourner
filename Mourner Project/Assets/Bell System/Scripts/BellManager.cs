using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellManager : MonoBehaviour
{
    [SerializeField] HandIK handIK;
    [SerializeField] float movementSensitivity = 1f;
    [Header("Hand Settings")]
    [SerializeField] Transform handGoal;
    [SerializeField] float maxHandMovement = 0.75f;
    [Header("Bell Settings")]
    [SerializeField] Transform bell;
    [SerializeField] Rigidbody bellCyllinderRB;

    private bool handOut = false;
    private Vector3 originalHandPosition;
    private float handDirection;

    void Start()
    {
        originalHandPosition = handGoal.localPosition;

        bellCyllinderRB.centerOfMass = new Vector3(0, -0.26f, 0);
        bellCyllinderRB.isKinematic = true;
        bellCyllinderRB.transform.parent = bell;
    }

    void Update()
    {
        if (handOut)
        {
            //Get local position
            Vector3 handLocalPosition = handGoal.localPosition;
            handLocalPosition.x += handDirection * movementSensitivity * Time.deltaTime;

            //Clamp the positon
            handLocalPosition.x = 
                Mathf.Clamp(handLocalPosition.x, originalHandPosition.x - maxHandMovement, originalHandPosition.x + maxHandMovement);

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

        handDirection = direction.x;
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
