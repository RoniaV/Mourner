using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunning : MovementState
{
    private RunSettings runSettings;
    private CharacterAim characterAim;

    public PlayerRunning(
        FSM fSM,
        RunSettings runSettings,
        PlayerControls playerControls,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        Transform camera,
        CharacterAim characterAim
        ) : base(fSM, runSettings, playerControls, player, characterFloorMovement, camera)
    {
        this.runSettings = runSettings;
        this.characterAim = characterAim;
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Enter Run State");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exit Run State");
    }

    public override void FixedUpdateState()
    {

    }

    public override void UpdateState()
    {
        base.UpdateState();

        characterAim.RotateCharacter(playerControls.Gameplay.Aim.ReadValue<Vector2>());


        //Check for Idle transition  
        if (inputValue.magnitude < 0.5f)
        {
            fSM.ChangeState((int)PlayerStates.Idle);
        }
        //Check for Walking transition
        else if (!playerControls.Gameplay.Run.IsPressed())
        {
            fSM.ChangeState((int)PlayerStates.Walking);
        }
        else if (playerControls.Gameplay.Jump.WasPressedThisFrame())
        {
            fSM.ChangeState((int)PlayerStates.Jump);
        }
    }
}
