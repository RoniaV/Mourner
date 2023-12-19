using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : MovementState
{
    private FallSettings fallSettings;
    private CharacterAim characterAim;
    private CharacterGravitable characterGravitable;
    private Animator animator;

    public PlayerFall(FSM fSM,
        FallSettings fallSettings,
        PlayerControls playerControls,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        Transform camera,
        CharacterAim characterAim,
        CharacterGravitable characterGravitable,
        Animator animator
        ) : base(fSM, fallSettings, playerControls, player, characterFloorMovement, camera)
    {
        this.fallSettings = fallSettings;
        this.characterAim = characterAim;
        this.characterGravitable = characterGravitable;
        this.animator = animator;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Fall State");

        animator.SetBool("Fall", true);
        characterGravitable.OnLanded += Land;
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exit Fall State");

        characterGravitable.OnLanded -= Land;
    }

    public override void FixedUpdateState()
    {

    }

    public override void UpdateState()
    {
        base.UpdateState();

        characterAim.RotateCharacter(playerControls.Gameplay.Aim.ReadValue<Vector2>());
    }

    private void Land()
    {
        animator.SetBool("Fall", false);
        fSM.ChangeState((int)PlayerStates.Idle);
    }
}
