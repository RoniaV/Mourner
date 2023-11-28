using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalking : State
{
    private InputAction walkingAction;
    private InputAction aimAction;
    private CharacterFloorMovement characterFloorMovement;
    private CharacterAim characterAim;
    private Transform camera;
    private Animator animator;

    private Vector2 inputValue = Vector2.zero;
    private Vector3 movDirection = Vector3.zero;

    public PlayerWalking(
        FSM fSM,
        InputAction walkingAction,
        InputAction aimAction,
        CharacterFloorMovement characterFloorMovement,
        CharacterAim characterAim,
        Transform camera,
        Animator animator
        ) : base(fSM) 
    {
        this.walkingAction = walkingAction;
        this.aimAction = aimAction;
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
        //Debug.Log("Input Value: " + walkingAction.ReadValue<Vector2>());

        inputValue = walkingAction.ReadValue<Vector2>();
        movDirection.x = inputValue.x;
        movDirection.z = inputValue.y;

        Vector3 fixedDir = camera.TransformDirection(movDirection);

        characterFloorMovement.SetMovementDirection(fixedDir);
        characterAim.RotateCharacter(aimAction.ReadValue<Vector2>());

        animator.SetFloat("XDir", inputValue.x);
        animator.SetFloat("YDir", inputValue.y);
    }
}
