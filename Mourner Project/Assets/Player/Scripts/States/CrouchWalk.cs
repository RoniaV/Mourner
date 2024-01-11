using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchWalk : MovementState
{
    CrouchWalkSettings crouchWalkSettings;
    CharacterAim characterAim;
    CharacterCrouch characterCrouch;
    Animator animator;

    public CrouchWalk(FSM fSM,
        CrouchWalkSettings crouchWalkSettings,
        PlayerControls playerControls,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        Transform camera,
        CharacterAim characterAim,
        CharacterCrouch characterCrouch,
        Animator animator,
        PlayerSoundManager soundManager
        ) : base(fSM, crouchWalkSettings, playerControls, player, characterFloorMovement, camera, soundManager)
    {
        this.crouchWalkSettings = crouchWalkSettings;
        this.characterAim = characterAim;
        this.characterCrouch = characterCrouch;
        this.animator = animator;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Crouch Walk State");
        base.EnterState();
    }

    public override void ExitState()
    {
        Debug.Log("Exit Crouch Walk State");
        base.ExitState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        soundManager.SetActualVelocity(characterFloorMovement.ActualVelocity.magnitude * 2.2f);

        characterAim.RotateCharacter(playerControls.Gameplay.Aim.ReadValue<Vector2>());

        if (!playerControls.Gameplay.Crouch.IsPressed() && characterCrouch.Crouch())
        {
            animator.SetBool("Crouch", false);
            fSM.ChangeState((int)PlayerStates.Idle);
        }
        else if (inputValue.magnitude < 0.5f)
        {
            fSM.ChangeState((int)PlayerStates.CrouchIdle);
        }
    }
}
