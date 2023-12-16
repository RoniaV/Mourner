using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : State
{
    private FallSettings fallSettings;
    private PlayerControls playerControls;
    private Transform player;
    private CharacterFloorMovement characterFloorMovement;
    private CharacterAim characterAim;
    private Transform camera;
    private CharacterGravitable characterGravitable;
    private Animator animator;

    private Vector2 smoothInputValue = Vector2.zero;
    private Vector2 smoothInputVelocity;
    private Vector3 movDirection = Vector3.zero;

    public PlayerFall(FSM fSM,
        FallSettings fallSettings,
        PlayerControls playerControls,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        CharacterAim characterAim,
        Transform camera,
        CharacterGravitable characterGravitable,
        Animator animator
        ) : base(fSM)
    {
        this.fallSettings = fallSettings;
        this.playerControls = playerControls;
        this.player = player;
        this.characterFloorMovement = characterFloorMovement;
        this.characterAim = characterAim;
        this.camera = camera;
        this.characterGravitable = characterGravitable;
        this.animator = animator;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Fall State");

        characterFloorMovement.SetVelocity(fallSettings.FallSpeed);
        animator.SetBool("Fall", true);

        characterGravitable.OnLanded += Land;
    }

    public override void ExitState()
    {
        Debug.Log("Exit Fall State");

        smoothInputValue = Vector2.zero;
        smoothInputVelocity = Vector2.zero;

        characterGravitable.OnLanded -= Land;
    }

    public override void FixedUpdateState()
    {

    }

    public override void UpdateState()
    {
        //Get and smooth input
        Vector3 inputValue = playerControls.Gameplay.Move.ReadValue<Vector2>();
        smoothInputValue = Vector2.SmoothDamp(smoothInputValue, inputValue, ref smoothInputVelocity, fallSettings.SmoothTime);

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

    private void Land()
    {
        animator.SetBool("Fall", false);
        fSM.ChangeState((int)PlayerStates.Idle);
    }
}
