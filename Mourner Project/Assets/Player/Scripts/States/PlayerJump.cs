using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerJump : State
{
    JumpSettings jumpSettings;
    PlayerControls playerControls;
    CharacterController characterController;
    CharacterFloorMovement characterFloorMovement;
    CharacterJump characterJump;
    CharacterGravitable characterGravitable;
    CharacterAim characterAim;
    Animator animator;
    Transform camera;
    Transform player;

    private Vector2 smoothInputValue = Vector2.zero;
    private Vector2 smoothInputVelocity;
    private Vector3 movDirection = Vector3.zero;

    public PlayerJump(FSM fSM,
        JumpSettings jumpSettings,
        PlayerControls playerControls,
        CharacterController characterController,
        CharacterFloorMovement characterFloorMovement,
        CharacterJump characterJump,
        CharacterGravitable characterGravitable,
        CharacterAim characterAim,
        Animator animator,
        Transform camera,
        Transform player
        ) : base(fSM)
    {
        this.jumpSettings = jumpSettings;
        this.playerControls = playerControls;
        this.characterController = characterController;
        this.characterFloorMovement = characterFloorMovement;
        this.characterJump = characterJump;
        this.characterGravitable = characterGravitable;
        this.characterAim = characterAim;
        this.animator = animator;
        this.camera = camera;
        this.player = player;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Jump State");

        characterGravitable.OnLanded += CharacterLanded;

        if(!characterJump.Jump())
        {
            fSM.ChangeState((int)PlayerStates.Idle);
            return;
        }

        animator.SetBool("Jump", true);
    }

    public override void ExitState()
    {
        Debug.Log("Exit Jump State");

        smoothInputValue = Vector2.zero;
        smoothInputVelocity = Vector2.zero;

        animator.SetBool("Jump", false);
        characterGravitable.OnLanded -= CharacterLanded;
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        //Get and smooth input
        Vector3 inputValue = playerControls.Gameplay.Move.ReadValue<Vector2>();
        smoothInputValue = Vector2.SmoothDamp(smoothInputValue, inputValue, ref smoothInputVelocity, jumpSettings.SmoothTime);

        //Transform input into movement diretion
        movDirection.x = smoothInputValue.x;
        movDirection.z = smoothInputValue.y;
        Vector3 fixedDir = camera.TransformDirection(movDirection);

        //Set mov and aim directions
        if (fixedDir != Vector3.zero)
            player.rotation = Quaternion.LookRotation(fixedDir, Vector3.up);
        characterFloorMovement.SetMovementDirection(fixedDir.normalized);
        
        characterAim.RotateCharacter(playerControls.Gameplay.Aim.ReadValue<Vector2>());
    }

    private void CharacterLanded()
    {
        animator.SetBool("Jump", false);
        animator.SetBool("Fall", false);
        fSM.ChangeState((int)PlayerStates.Idle);
    }
}
