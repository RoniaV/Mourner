using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementState : State
{
    protected MovementSettings movSettings;
    protected PlayerControls playerControls;
    protected Transform player;
    protected CharacterFloorMovement characterFloorMovement;
    protected Transform camera;

    protected Vector2 inputValue = Vector2.zero;
    protected Vector2 smoothInputValue = Vector2.zero;
    protected Vector2 smoothInputVelocity;
    protected Vector3 movDirection = Vector3.zero;

    public MovementState(FSM fSM,
        MovementSettings movSettings,
        PlayerControls playerControls,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        Transform camera
        ) : base(fSM)
    {
        this.movSettings = movSettings;
        this.playerControls = playerControls;
        this.player = player;
        this.characterFloorMovement = characterFloorMovement;
        this.camera = camera;
    }

    public override void EnterState()
    {
        characterFloorMovement.SetVelocity(movSettings.MovSpeed);
    }

    public override void ExitState()
    {
        smoothInputValue = Vector2.zero;
        smoothInputVelocity = Vector2.zero;
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        // Get and smooth input
        inputValue = playerControls.Gameplay.Move.ReadValue<Vector2>();
        smoothInputValue = Vector2.SmoothDamp(smoothInputValue, inputValue, ref smoothInputVelocity, movSettings.SmoothTime);

        // Transform input into movement direction
        movDirection.x = smoothInputValue.x;
        movDirection.z = smoothInputValue.y;
        Vector3 fixedDir = camera.TransformDirection(movDirection);

        // Set movement direction
        if (fixedDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(fixedDir, Vector3.up);

            // Smoothly rotate towards the target rotation
            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * movSettings.RotationSpeed);
        }

        characterFloorMovement.SetMovementDirection(fixedDir.normalized);
    }

    //Turn based on dot product
        ////Get input
        //inputValue = playerControls.Gameplay.Move.ReadValue<Vector2>();
        //Debug.Log("Input Value: " + inputValue);

        ////Transform input into movement diretion
        //movDirection.x = inputValue.x;
        //movDirection.z = inputValue.y;
        //Vector3 fixedDir = camera.TransformDirection(movDirection);
        //Debug.Log("Fixed Dir: " + fixedDir);

        //float dotProduct = Vector3.Dot(fixedDir, player.forward);

        //Debug.Log("Dot Product: " + dotProduct);
        //if(dotProduct < movSettings.DotProductThreshold)
        //{
        //    fixedDir = fixedDir + player.right;
        //}

        //smoothDirection = Vector2.SmoothDamp(smoothDirection, fixedDir, ref smoothInputVelocity, movSettings.SmoothTime);

        ////Set mov and aim directions
        //if (fixedDir != Vector3.zero)
        //    player.rotation = Quaternion.LookRotation(smoothDirection, Vector3.up);
        //characterFloorMovement.SetMovementDirection(smoothDirection.normalized);
}
