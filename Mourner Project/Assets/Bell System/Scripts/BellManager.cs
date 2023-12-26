using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellManager : MonoBehaviour
{
    [SerializeField] HandIK handIK;
    [SerializeField] float movementSensitivity = 1f;

    private bool handOut = false;

    public void PutHandOut()
    {
        Debug.Log("PutHandOut");
        handOut = true;
        handIK.SetWeightGoal(1f);
    }

    public void PutHandIn()
    {
        Debug.Log("PutHandIn");
        handOut = false;
        handIK.SetWeightGoal(0f);
    }

    #region Test
    private PlayerControls playerControls;
    
    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void Update()
    {
        if(playerControls.Gameplay.BellOut.WasPressedThisFrame())
        {
            PutHandOut();
        }
        else if(playerControls.Gameplay.BellOut.WasReleasedThisFrame())
        {
            PutHandIn();
        }
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }
    #endregion
}
