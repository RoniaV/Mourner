using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : State
{
    private IdleSettings idleSettings;
    private CharacterFloorMovement characterFloorMovement;
    private CharacterAim characterAim;
    private Animator animator;


    public PlayerIdle(FSM fSM,
        IdleSettings idleSettings,
        CharacterFloorMovement characterFloorMovement,
        CharacterAim characterAim,
        Animator animator) : base(fSM)
    {
        this.idleSettings = idleSettings;
        this.characterFloorMovement = characterFloorMovement;
        this.characterAim = characterAim;
        this.animator = animator;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Idle State");

        characterFloorMovement.SetVelocity(0);

        idleSettings.WalkingAction.Enable();
        idleSettings.AimAction.Enable();
    }

    public override void ExitState()
    {
        Debug.Log("Exit Idle State");

        idleSettings.WalkingAction.Disable();
        idleSettings.AimAction.Disable();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        characterAim.RotateCharacter(idleSettings.AimAction.ReadValue<Vector2>());

        if (idleSettings.WalkingAction.ReadValue<Vector2>().magnitude > 0.5f)
        {
            fSM.ChangeState((int)PlayerFSM.PlayerStates.Walking);
        }
    }
}
