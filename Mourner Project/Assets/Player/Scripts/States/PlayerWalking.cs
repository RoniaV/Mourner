using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalking : State
{
    private InputAction walkingAction;
    private CharacterFloorMovement characterFloorMovement;

    private Vector2 inputValue = Vector2.zero;
    private Vector3 movDirection = Vector3.zero;

    public PlayerWalking(FSM fSM, InputAction walkingAction, CharacterFloorMovement characterFloorMovement) : base(fSM) 
    {
        this.walkingAction = walkingAction;
        this.characterFloorMovement = characterFloorMovement;
    }

    public override void EnterState()
    {
        walkingAction.Enable(); 
    }

    public override void ExitState()
    {
        walkingAction.Disable();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        Debug.Log("Input Value: " + walkingAction.ReadValue<Vector2>());

        inputValue = walkingAction.ReadValue<Vector2>();
        movDirection.x = inputValue.x;
        movDirection.z = inputValue.y;

        characterFloorMovement.SetMovementDirection(movDirection);
    }
}
