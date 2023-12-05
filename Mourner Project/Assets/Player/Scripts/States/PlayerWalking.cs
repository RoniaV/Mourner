using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalking : State
{
    private Transform player;
    private WalkSettings walkSettings;
    private CharacterFloorMovement characterFloorMovement;
    private CharacterAim characterAim;
    private Transform camera;
    private Animator animator;

    private Vector2 smoothInputValue = Vector2.zero;
    private Vector2 smoothInputVelocity;
    private Vector3 movDirection = Vector3.zero;

    public PlayerWalking(
        FSM fSM,
        WalkSettings walkSettings,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        CharacterAim characterAim,
        Transform camera,
        Animator animator
        ) : base(fSM) 
    {
        this.player = player;
        this.walkSettings = walkSettings;
        this.characterFloorMovement = characterFloorMovement;
        this.characterAim = characterAim;
        this.camera = camera;
        this.animator = animator;
    }

    public override void EnterState()
    {
        walkSettings.WalkingAction.Enable(); 
        walkSettings.AimAction.Enable();
        walkSettings.RunAction.Enable();
    }

    public override void ExitState()
    {
        walkSettings.WalkingAction.Disable();
        walkSettings.AimAction.Disable();
        walkSettings.RunAction.Disable();
    }

    public override void FixedUpdateState()
    {

    }

    public override void UpdateState()
    {
        //Get and smooth input
        Vector3 inputValue = walkSettings.WalkingAction.ReadValue<Vector2>();
        smoothInputValue = Vector2.SmoothDamp(smoothInputValue, inputValue, ref smoothInputVelocity, walkSettings.SmoothTime);

        //Transform input into movement diretion
        movDirection.x = smoothInputValue.x;
        movDirection.z = smoothInputValue.y;
        Vector3 fixedDir = camera.TransformDirection(movDirection);

        //Set mov and aim directions
        if (fixedDir != Vector3.zero)
            player.rotation = Quaternion.LookRotation(fixedDir, Vector3.up);
        characterFloorMovement.SetMovementDirection(fixedDir);
        characterAim.RotateCharacter(walkSettings.AimAction.ReadValue<Vector2>());

        //Check and set run speed
        bool runInput = walkSettings.RunAction.IsPressed();
        float movSpeed = 
            runInput ? walkSettings.RunSpeed : walkSettings.WalkSpeed;
        characterFloorMovement.ModifyVelocity(movSpeed * smoothInputValue.magnitude);

        //Set animations
        float animValue = smoothInputValue.magnitude * (runInput ? 1 : 0.5f);
        animator.SetFloat("Vel", animValue);
    }
}
