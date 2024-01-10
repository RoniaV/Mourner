using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRun : MovementState
{
    private RunSettings runSettings;
    private CharacterAim characterAim;

    public PlayerRun(
        FSM fSM,
        RunSettings runSettings,
        PlayerControls playerControls,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        Transform camera,
        CharacterAim characterAim,
        PlayerSoundManager soundManager
        ) : base(fSM, runSettings, playerControls, player, characterFloorMovement, camera, soundManager)
    {
        this.runSettings = runSettings;
        this.characterAim = characterAim;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Run State");
        base.EnterState();
    }

    public override void ExitState()
    {
        Debug.Log("Exit Run State");
        base.ExitState();
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
