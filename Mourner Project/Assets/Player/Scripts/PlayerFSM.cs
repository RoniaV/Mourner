using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerStates
{
    Idle,
    Walking,
    Running,
    Jump
}

[RequireComponent(typeof(CharacterFloorMovement))]
public class PlayerFSM : FSM
{
    [Header("Components")]
    [SerializeField] CharacterAim characterAim;
    [SerializeField] Transform playerCamera;
    [SerializeField] Animator animator;
    [Header("State Settings")]
    [SerializeField] PlayerStates initialState = PlayerStates.Idle;
    [SerializeField] IdleSettings idleSettings;
    [SerializeField] WalkSettings walkSettings;
    [SerializeField] RunSettings runSettings;

    CharacterController characterController;
    CharacterFloorMovement floorMovement;
    CharacterJump characterJump;
    CharacterGravitable characterGravitable;
    PlayerControls playerControls;

    private State actualState;
    //States
    private PlayerIdle idleState;
    private PlayerWalking walkingState;
    private PlayerRunning runningState;
    private PlayerJump jumpState;


    void Awake()
    {
        floorMovement = GetComponent<CharacterFloorMovement>();
        characterController = GetComponent<CharacterController>();
        characterJump = GetComponent<CharacterJump>();
        characterGravitable = GetComponent<CharacterGravitable>();

        playerControls = new PlayerControls();
    }

    void Start()
    {
        #region Initialize States
        idleState = new PlayerIdle(
            this,
            idleSettings,
            playerControls,
            floorMovement,
            characterAim,
            animator
            );

        walkingState = new PlayerWalking(
            this,
            walkSettings,
            playerControls,
            transform,
            floorMovement,
            characterAim,
            playerCamera
            );

        runningState = new PlayerRunning(
            this,
            runSettings,
            playerControls,
            transform,
            floorMovement,
            characterAim,
            playerCamera
            );

        jumpState = new PlayerJump(
            this,
            playerControls,
            characterController,
            characterJump,
            characterGravitable,
            characterAim,
            animator
            );
        #endregion

        ChangeState((int)initialState);
    }

    void FixedUpdate()
    {
        actualState?.FixedUpdateState();
    }

    void Update()
    {
        actualState?.UpdateState();

        Vector3 fixedVel = characterController.velocity;
        fixedVel.y = 0;
        animator.SetFloat("Vel", floorMovement.ActualVelocity.magnitude);

        if (characterController.velocity.y < -3 && !characterGravitable.IsGrounded)
            animator.SetBool("Fall", true);
        else if (characterGravitable.IsGrounded)
            animator.SetBool("Fall", false);
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    public override void ChangeState(int state)
    {
        if (actualState != null)
            actualState.ExitState();

        switch (state)
        {
            case (int)PlayerStates.Idle:
                actualState = idleState;
                break;
            case (int)PlayerStates.Walking:
                actualState = walkingState;
                break;
            case (int)PlayerStates.Running:
                actualState = runningState;
                break;
            case (int)PlayerStates.Jump:
                actualState = jumpState;
                break;
            default:
                Debug.Log("Non existent state");
                ChangeState((int)PlayerStates.Idle);
                break;
        }

        actualState.EnterState();
    }
}
