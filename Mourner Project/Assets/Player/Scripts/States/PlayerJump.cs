using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerJump : MovementState
{
    JumpSettings jumpSettings;
    CharacterController characterController;
    CharacterJump characterJump;
    CharacterGravitable characterGravitable;
    CharacterAim characterAim;
    Animator animator;

    public PlayerJump(FSM fSM,
        JumpSettings jumpSettings,
        PlayerControls playerControls,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        Transform camera,
        CharacterJump characterJump,
        CharacterGravitable characterGravitable,
        CharacterAim characterAim,
        CharacterController characterController,
        Animator animator
        ) : base(fSM, jumpSettings, playerControls, player, characterFloorMovement, camera)
    {
        this.jumpSettings = jumpSettings;
        this.characterController = characterController;
        this.characterJump = characterJump;
        this.characterGravitable = characterGravitable;
        this.characterAim = characterAim;
        this.animator = animator;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Jump State");

        characterGravitable.OnLanded += CharacterLanded;

        if(!characterJump.Jump())
        {
            fSM.ChangeState((int)PlayerStates.Idle);
            return;
        }

        animator.SetBool("Jump", true);
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exit Jump State");

        animator.SetBool("Jump", false);
        characterGravitable.OnLanded -= CharacterLanded;
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        characterAim.RotateCharacter(playerControls.Gameplay.Aim.ReadValue<Vector2>());
    }

    private void CharacterLanded()
    {
        animator.SetBool("Jump", false);
        animator.SetBool("Fall", false);
        fSM.ChangeState((int)PlayerStates.Idle);
    }
}
