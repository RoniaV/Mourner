using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterFloorMovement))]
public class PlayerFSM : FSM
{
    public enum PlayerStates
    {
        Walking,
        Running
    }


    [SerializeField] PlayerStates initialState = PlayerStates.Walking;
    [Header("Input Settings")]
    [SerializeField] InputAction walkingAction;

    CharacterFloorMovement floorMovement;

    private State actualState;
    //States
    private PlayerWalking walkingState;


    void Awake()
    {
        floorMovement = GetComponent<CharacterFloorMovement>();
    }

    void Start()
    {
        walkingState = new PlayerWalking((FSM)this, walkingAction, floorMovement);


        switch(initialState)
        {
            case PlayerStates.Walking:
                
                break;
            case PlayerStates.Running:
                break;
        }
    }

    public override void ChangeState(int state)
    {
        
    }
}
