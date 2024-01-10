using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchIdle : State
{
    private CrouchIdleSettings crouchIdleSettings;
    private PlayerControls playerControls;
    private CharacterFloorMovement characterFloorMovement;
    private CharacterAim characterAim;
    private Animator animator;
    private CharacterCrouch characterCrouch;
    private PlayerSoundManager soundManager;


    public CrouchIdle(FSM fSM,
        CrouchIdleSettings crouchIdleSettings,
        PlayerControls playerControls,
        CharacterFloorMovement characterFloorMovement,
        CharacterAim characterAim,
        Animator animator,
        CharacterCrouch characterCrouch,
        PlayerSoundManager soundManager) : base(fSM)
    {
        this.crouchIdleSettings = crouchIdleSettings;
        this.playerControls = playerControls;
        this.characterFloorMovement = characterFloorMovement;
        this.characterAim = characterAim;
        this.animator = animator;
        this.characterCrouch = characterCrouch;
        this.soundManager = soundManager;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Crouch Idle State");

        if (!characterCrouch.Crouched)
        {
            if (!characterCrouch.Crouch())
            {
                fSM.ChangeState((int)PlayerStates.Idle);
                return;
            }
        }

        animator.SetBool("Crouch", true);
        soundManager.StopFootstepSound();
        characterFloorMovement.SetVelocity(0);
    }

    public override void ExitState()
    {
        Debug.Log("Exit Crouch Idle State");
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        characterAim.RotateCharacter(playerControls.Gameplay.Aim.ReadValue<Vector2>());

        if (!playerControls.Gameplay.Crouch.IsPressed() && characterCrouch.Crouch())
        {
            animator.SetBool("Crouch", false);
            fSM.ChangeState((int)PlayerStates.Idle);
        }
        else if (playerControls.Gameplay.Move.ReadValue<Vector2>().magnitude > 0.5f)
        {
            fSM.ChangeState((int)PlayerStates.CrouchWalk);
        }
    }
}
