using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalking : State
{
    private InputAction walkingAction;
    private CharacterFloorMovement characterFloorMovement;

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
        characterFloorMovement.SetMovementDirection(walkingAction.ReadValue<Vector2>());
    }
}
