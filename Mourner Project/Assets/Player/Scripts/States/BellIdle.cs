using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellIdle : State
{
    private IdleSettings idleSettings;
    private PlayerControls playerControls;
    private CharacterFloorMovement characterFloorMovement;
    private CharacterAim characterAim;
    private BellManager bellManager;
    private GameObject bellCamera;
    private Animator animator;


    public BellIdle(FSM fSM,
        IdleSettings idleSettings,
        PlayerControls playerControls,
        CharacterFloorMovement characterFloorMovement,
        CharacterAim characterAim,
        BellManager bellManager,
        GameObject bellCamera,
        Animator animator) : base(fSM)
    {
        this.idleSettings = idleSettings;
        this.playerControls = playerControls;
        this.characterFloorMovement = characterFloorMovement;
        this.characterAim = characterAim;
        this.bellManager = bellManager;
        this.bellCamera = bellCamera;
        this.animator = animator;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Bell Idle State");

        bellManager.PutHandOut();
        characterFloorMovement.SetVelocity(0);
        characterAim.RotateCharacter(Vector2.zero);
        bellCamera.SetActive(true);
    }

    public override void ExitState()
    {
        Debug.Log("Exit Bell Idle State");

        bellManager.PutHandIn();
        bellCamera.SetActive(false);
    }

    public override void FixedUpdateState()
    {

    }

    public override void UpdateState()
    {
        bellManager.MoveHand(playerControls.Gameplay.Aim.ReadValue<Vector2>());

        if(playerControls.Gameplay.BellOut.WasReleasedThisFrame())
        {
            fSM.ChangeState((int)PlayerStates.Idle);
        }
    }
}
