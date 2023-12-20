using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWalk : MovementState
{
    private WalkSettings walkSettings;
    private CharacterAim characterAim;

    public PlayerWalk( FSM fSM,
        WalkSettings walkSettings,
        PlayerControls playerControls,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        Transform camera,
        CharacterAim characterAim
        ) : base(fSM, walkSettings, playerControls, player, characterFloorMovement, camera) 
    {
        this.walkSettings = walkSettings;
        this.characterAim = characterAim;
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Enter Walk State");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exit Walk State");
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
        //Check for Running transition
        else if(playerControls.Gameplay.Run.IsPressed())
        {
            fSM.ChangeState((int)PlayerStates.Running);
        }
        else if (playerControls.Gameplay.Jump.WasPressedThisFrame())
        {
            fSM.ChangeState((int)PlayerStates.Jump);
        }
        else if (playerControls.Gameplay.Crouch.IsPressed())
        {
            fSM.ChangeState((int)PlayerStates.CrouchIdle);
        }
    }
}
