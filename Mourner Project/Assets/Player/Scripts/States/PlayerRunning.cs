using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunning : State
{
    private RunSettings runSettings;
    private PlayerControls playerControls;
    private Transform player;
    private CharacterFloorMovement characterFloorMovement;
    private CharacterAim characterAim;
    private Transform camera;

    private Vector2 smoothInputValue = Vector2.zero;
    private Vector2 smoothInputVelocity;
    private Vector3 movDirection = Vector3.zero;

    public PlayerRunning(
        FSM fSM,
        RunSettings walkSettings,
        PlayerControls playerControls,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        CharacterAim characterAim,
        Transform camera
        ) : base(fSM)
    {
        this.runSettings = walkSettings;
        this.playerControls = playerControls;
        this.player = player;
        this.characterFloorMovement = characterFloorMovement;
        this.characterAim = characterAim;
        this.camera = camera;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Run State");

        characterFloorMovement.SetVelocity(runSettings.RunSpeed);
    }

    public override void ExitState()
    {
        Debug.Log("Exit Run State");

        smoothInputValue = Vector2.zero;
        smoothInputVelocity = Vector2.zero;
    }

    public override void FixedUpdateState()
    {

    }

    public override void UpdateState()
    {
        //Get and smooth input
        Vector3 inputValue = playerControls.Gameplay.Move.ReadValue<Vector2>();
        smoothInputValue = Vector2.SmoothDamp(smoothInputValue, inputValue, ref smoothInputVelocity, runSettings.SmoothTime);

        //Transform input into movement diretion
        movDirection.x = smoothInputValue.x;
        movDirection.z = smoothInputValue.y;
        Vector3 fixedDir = camera.TransformDirection(movDirection);

        //Set mov and aim directions
        if (fixedDir != Vector3.zero)
            player.rotation = Quaternion.LookRotation(fixedDir, Vector3.up);
        characterFloorMovement.SetMovementDirection(fixedDir.normalized);
        characterAim.RotateCharacter(playerControls.Gameplay.Aim.ReadValue<Vector2>());


        //Check for Idle transition  
        if (inputValue.magnitude < 0.5f)
        {
            fSM.ChangeState((int)PlayerStates.Idle);
        }
        //Check for Walking transition
        else if (!playerControls.Gameplay.Run.IsPressed())
        {
            fSM.ChangeState((int)PlayerStates.Walking);
        }
        else if (playerControls.Gameplay.Jump.WasPressedThisFrame())
        {
            fSM.ChangeState((int)PlayerStates.Jump);
        }
    }
}
