using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMovementState : State
{
    protected AirMovementSettings movSettings;
    protected PlayerControls playerControls;
    protected Transform player;
    protected CharacterFloorMovement characterFloorMovement;
    protected Transform camera;
    protected CharacterGravitable characterGravitable;
    protected CharacterAim characterAim;
    protected Animator animator;
    protected PlayerSoundManager soundManager;

    protected Vector2 inputValue = Vector2.zero;
    protected Vector2 smoothInputValue = Vector2.zero;
    protected Vector2 smoothInputVelocity;
    protected Vector3 movDirection = Vector3.zero;
    protected Vector3 smoothDirVel = Vector3.zero;
    private float smoothVelocity;

    public AirMovementState(FSM fSM,
        AirMovementSettings movSettings,
        PlayerControls playerControls,
        Transform player,
        CharacterFloorMovement characterFloorMovement,
        Transform camera,
        CharacterGravitable characterGravitable,
        CharacterAim characterAim,
        Animator animator,
        PlayerSoundManager soundManager
        ) : base(fSM)
    {
        this.movSettings = movSettings;
        this.playerControls = playerControls;
        this.player = player;
        this.characterFloorMovement = characterFloorMovement;
        this.camera = camera;
        this.characterGravitable = characterGravitable;
        this.characterAim = characterAim;
        this.animator = animator;
        this.soundManager = soundManager;
    }

    public override void EnterState()
    {
        characterGravitable.OnLanded += CharacterLanded;

        smoothInputValue = playerControls.Gameplay.Move.ReadValue<Vector2>();
        movDirection = characterFloorMovement.ActualVelocity.normalized;

        soundManager.StopFootstepSound();

        //if (characterFloorMovement.ActualVelocity.magnitude <= 0.1f)
        //    characterFloorMovement.SetVelocity(movSettings.MovSpeed);
    }

    public override void ExitState()
    {
        characterGravitable.OnLanded -= CharacterLanded;

        smoothInputValue = Vector2.zero;
        smoothInputVelocity = Vector2.zero;
    }

    public override void FixedUpdateState()
    {

    }

    public override void UpdateState()
    {
        //// Get and smooth input
        //inputValue = playerControls.Gameplay.Move.ReadValue<Vector2>();
        //smoothInputValue = Vector2.SmoothDamp(smoothInputValue, inputValue, ref smoothInputVelocity, movSettings.SmoothTime);

        ////Transform input into movement direction
        //Vector3 fixedDir = camera.TransformDirection(new Vector3(smoothInputValue.x, 0, smoothInputValue.y));
        //movDirection = Vector3.SmoothDamp(movDirection, fixedDir, ref smoothDirVel, movSettings.Inertia);

        movDirection = Vector3.SmoothDamp(movDirection, Vector3.zero, ref smoothDirVel, movSettings.Inertia);

        // Set movement direction
        if (movDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movDirection, Vector3.up);

            // Smoothly rotate towards the target rotation
            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * movSettings.RotationSpeed);
        }

        characterFloorMovement.SetMovementDirection(movDirection);

        #region Old Logic
        //// Get input
        //inputValue = playerControls.Gameplay.Move.ReadValue<Vector2>();

        //// Transform input into movement direction
        //movDirection.x = inputValue.x;
        //movDirection.z = inputValue.y;
        //Vector3 fixedDir = camera.TransformDirection(movDirection);

        ////Smooth input
        //smoothInputValue = Vector2.SmoothDamp(smoothInputValue, fixedDir, ref smoothInputVelocity, movSettings.SmoothTime);

        //// Set movement direction
        //if (smoothInputValue != Vector2.zero)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(smoothInputValue, Vector3.up);

        //    // Smoothly rotate towards the target rotation
        //    player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * movSettings.RotationSpeed);
        //}

        //characterFloorMovement.SetMovementDirection(smoothInputValue);

        //float smoothVel = characterFloorMovement.ActualVelocity.magnitude * smoothInputValue.magnitude;

        //characterFloorMovement.SetVelocity(
        //    Mathf.SmoothDamp(characterFloorMovement.ActualVelocity.magnitude, smoothVel, ref smoothVelocity, movSettings.Inertia)
        //    );
        #endregion

        characterAim.RotateCharacter(playerControls.Gameplay.Aim.ReadValue<Vector2>());
    }

    protected virtual void CharacterLanded()
    {
        if (characterFloorMovement.VerticalVelocity <= 0)
            soundManager.PlaySound(PlayerSounds.Land);

        animator.SetBool("Fall", false);
        fSM.ChangeState((int)PlayerStates.Idle);
    }
}
