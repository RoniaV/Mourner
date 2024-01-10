using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerJump : AirMovementState
{
    JumpSettings jumpSettings;
    CharacterJump characterJump;


    public PlayerJump(
        FSM fSM,
        JumpSettings jumpSettings,
        PlayerControls playerControls,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        Transform camera,
        CharacterGravitable characterGravitable,
        CharacterAim characterAim,
        Animator animator,
        CharacterJump characterJump,
        PlayerSoundManager soundManager
        ) : base(fSM, jumpSettings, playerControls, player, characterFloorMovement, camera, characterGravitable, characterAim, animator, soundManager)
    {
        this.jumpSettings = jumpSettings;
        this.characterJump = characterJump;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Jump State");

        if(!characterJump.Jump())
        {
            Debug.Log("Jump Failed");
            fSM.ChangeState((int)PlayerStates.Idle);
            return;
        }

        base.EnterState();
        animator.SetBool("Jump", true);
        soundManager.PlaySound(PlayerSounds.Jump);
    }

    public override void ExitState()
    {
        Debug.Log("Exit Jump State");
        base.ExitState();

        animator.SetBool("Jump", false);
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    protected override void CharacterLanded()
    {
        animator.SetBool("Jump", false);
        base.CharacterLanded();
    }
}
