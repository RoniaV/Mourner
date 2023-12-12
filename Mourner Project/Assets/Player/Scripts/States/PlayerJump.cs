using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : State
{
    CharacterController characterController;
    CharacterJump characterJump;
    CharacterGravitable characterGravitable;

    public PlayerJump(FSM fSM,
        CharacterController characterController,
        CharacterJump characterJump,
        CharacterGravitable characterGravitable
        ) : base(fSM)
    {
        this.characterController = characterController;
        this.characterJump = characterJump;
        this.characterGravitable = characterGravitable;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Jump State");

        characterJump.OnLanded += CharacterLanded;
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
        
    }

    private void CharacterLanded()
    {
        fSM.ChangeState((int)PlayerStates.Idle);
    }
}
