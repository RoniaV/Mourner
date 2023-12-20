using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerJump : MovementState
{
    JumpSettings jumpSettings;
    CharacterJump characterJump;
    CharacterGravitable characterGravitable;
    CharacterAim characterAim;
    Animator animator;

    private float smoothVelocity;

    public PlayerJump(FSM fSM,
        JumpSettings jumpSettings,
        PlayerControls playerControls,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        Transform camera,
        CharacterJump characterJump,
        CharacterGravitable characterGravitable,
        CharacterAim characterAim,
        Animator animator
        ) : base(fSM, jumpSettings, playerControls, player, characterFloorMovement, camera)
    {
        this.jumpSettings = jumpSettings;
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

        smoothInputValue = playerControls.Gameplay.Move.ReadValue<Vector2>();
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

        characterFloorMovement.SetVelocity(
            Mathf.SmoothDamp(characterFloorMovement.ActualVelocity.magnitude, 0, ref smoothVelocity, jumpSettings.Inertia)
            );

        characterAim.RotateCharacter(playerControls.Gameplay.Aim.ReadValue<Vector2>());
    }

    private void CharacterLanded()
    {
        animator.SetBool("Jump", false);
        animator.SetBool("Fall", false);
        fSM.ChangeState((int)PlayerStates.Idle);
    }
}
