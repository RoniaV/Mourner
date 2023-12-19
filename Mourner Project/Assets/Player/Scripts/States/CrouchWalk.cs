using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchWalk : MovementState
{
    CrouchWalkSettings crouchWalkSettings;
    CharacterAim characterAim;
    CharacterCrouch characterCrouch;

    public CrouchWalk(FSM fSM,
        CrouchWalkSettings crouchWalkSettings,
        PlayerControls playerControls,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        Transform camera,
        CharacterAim characterAim,
        CharacterCrouch characterCrouch) : base(fSM, crouchWalkSettings, playerControls, player, characterFloorMovement, camera)
    {
        this.crouchWalkSettings = crouchWalkSettings;
        this.characterAim = characterAim;
        this.characterCrouch = characterCrouch;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        characterAim.RotateCharacter(playerControls.Gameplay.Aim.ReadValue<Vector2>());

        if (!playerControls.Gameplay.Crouch.IsPressed())
        {
            if (characterCrouch.Crouch())
            {
                fSM.ChangeState((int)PlayerStates.Idle);
            }
        }
        else if (inputValue.magnitude < 0.5f)
        {
            fSM.ChangeState((int)PlayerStates.Idle);
        }
    }
}
