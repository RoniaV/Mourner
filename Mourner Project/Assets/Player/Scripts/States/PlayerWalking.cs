using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalking : State
{
    private Transform player;
    private InputAction walkingAction;
    private InputAction aimAction;
    private float smoothTime;
    private CharacterFloorMovement characterFloorMovement;
    private CharacterAim characterAim;
    private Transform camera;
    private Animator animator;

    private Vector2 smoothInputValue = Vector2.zero;
    private Vector2 smoothInputVelocity;
    private Vector3 movDirection = Vector3.zero;

    public PlayerWalking(
        FSM fSM,
        Transform player,
        InputAction walkingAction,
        InputAction aimAction,
        float smoothTime,
        CharacterFloorMovement characterFloorMovement,
        CharacterAim characterAim,
        Transform camera,
        Animator animator
        ) : base(fSM) 
    {
        this.player = player;
        this.walkingAction = walkingAction;
        this.aimAction = aimAction;
        this.smoothTime = smoothTime;
        this.characterFloorMovement = characterFloorMovement;
        this.characterAim = characterAim;
        this.camera = camera;
        this.animator = animator;
    }

    public override void EnterState()
    {
        walkingAction.Enable(); 
        aimAction.Enable();
    }

    public override void ExitState()
    {
        walkingAction.Disable();
        aimAction.Disable();
    }

    public override void FixedUpdateState()
    {

    }

    public override void UpdateState()
    {
        //Get and smooth input
        Vector3 inputValue = walkingAction.ReadValue<Vector2>();
        smoothInputValue = Vector2.SmoothDamp(smoothInputValue, inputValue, ref smoothInputVelocity, smoothTime);

        //Transform input into movement diretion 
        movDirection.x = smoothInputValue.x;
        movDirection.z = smoothInputValue.y;
        Vector3 fixedDir = camera.TransformDirection(movDirection);

        //Set mov and aim directions
        if (fixedDir != Vector3.zero)
            player.rotation = Quaternion.LookRotation(fixedDir, Vector3.up);
        characterFloorMovement.SetMovementDirection(fixedDir);
        characterAim.RotateCharacter(aimAction.ReadValue<Vector2>());

        //Set animations
        animator.SetFloat("Vel", smoothInputValue.magnitude);
    }
}
