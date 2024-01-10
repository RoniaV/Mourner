using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : AirMovementState
{
    FallSettings fallSettings;


    public PlayerFall(
        FSM fSM,
        FallSettings fallSettings,
        PlayerControls playerControls,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        Transform camera,
        CharacterGravitable characterGravitable,
        CharacterAim characterAim,
        Animator animator,
        PlayerSoundManager soundManager 
        ) : base(fSM, fallSettings, playerControls, player, characterFloorMovement, camera, characterGravitable, characterAim, animator, soundManager)
    {
        this.fallSettings = fallSettings;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Fall State");

        base.EnterState();
        animator.SetBool("Fall", true);
    }

    public override void ExitState()
    {
        Debug.Log("Exit Fall State");
        base.ExitState();
    }

    public override void FixedUpdateState()
    {

    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
