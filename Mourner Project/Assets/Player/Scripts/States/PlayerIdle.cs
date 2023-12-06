using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : State
{
    private IdleSettings idleSettings;
    private PlayerControls playerControls;
    private CharacterFloorMovement characterFloorMovement;
    private CharacterAim characterAim;
    private Animator animator;


    public PlayerIdle( FSM fSM,
        IdleSettings idleSettings,
        PlayerControls playerControls,
        CharacterFloorMovement characterFloorMovement,
        CharacterAim characterAim,
        Animator animator) : base(fSM)
    {
        this.idleSettings = idleSettings;
        this.playerControls = playerControls;
        this.characterFloorMovement = characterFloorMovement;
        this.characterAim = characterAim;
        this.animator = animator;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Idle State");

        characterFloorMovement.SetVelocity(0);
    }

    public override void ExitState()
    {
        Debug.Log("Exit Idle State");
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        characterAim.RotateCharacter(playerControls.Gameplay.Aim.ReadValue<Vector2>());

        if (playerControls.Gameplay.Move.ReadValue<Vector2>().magnitude > 0.5f)
        {
            fSM.ChangeState((int)PlayerFSM.PlayerStates.Walking);
        }
    }
}
