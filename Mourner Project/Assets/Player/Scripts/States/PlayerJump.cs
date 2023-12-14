using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : State
{
    PlayerControls playerControls;
    CharacterController characterController;
    CharacterJump characterJump;
    CharacterGravitable characterGravitable;
    CharacterAim characterAim;
    Animator animator;

    public PlayerJump(FSM fSM,
        PlayerControls playerControls,
        CharacterController characterController,
        CharacterJump characterJump,
        CharacterGravitable characterGravitable,
        CharacterAim characterAim,
        Animator animator
        ) : base(fSM)
    {
        this.playerControls = playerControls;
        this.characterController = characterController;
        this.characterJump = characterJump;
        this.characterGravitable = characterGravitable;
        this.characterAim = characterAim;
        this.animator = animator;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Jump State");

        characterJump.OnLanded += CharacterLanded;
        animator.SetTrigger("Jump");
        characterJump.Jump();
    }

    public override void ExitState()
    {
        Debug.Log("Exit Jump State");

        characterJump.OnLanded -= CharacterLanded;
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        characterAim.RotateCharacter(playerControls.Gameplay.Aim.ReadValue<Vector2>());
    }

    private void CharacterLanded()
    {
        fSM.ChangeState((int)PlayerStates.Idle);
    }
}
