using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerStates
{
    Idle,
    Walking,
    Running,
    Jump,
    Fall,
    CrouchIdle,
    CrouchWalk
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
    [SerializeField] JumpSettings jumpSettings;
    [SerializeField] FallSettings fallSettings;
    [SerializeField] CrouchIdleSettings crouchIdleSettings;
    [SerializeField] CrouchWalkSettings crouchWalkSettings;
    [Header("Variables")]
    [SerializeField] float fallTime = 1.5f;

    CharacterController characterController;
    CharacterFloorMovement floorMovement;
    CharacterJump characterJump;
    CharacterGravitable characterGravitable;
    CharacterCrouch characterCrouch;
    PlayerControls playerControls;

    private State actualState;
    //States
    private PlayerIdle idleState;
    private PlayerWalk walkingState;
    private PlayerRun runningState;
    private PlayerJump jumpState;
    private PlayerFall fallState;
    private CrouchIdle crouchIdleState;
    private CrouchWalk crouchWalkState;

    private float fallTimer = 0;

    void Awake()
    {
        floorMovement = GetComponent<CharacterFloorMovement>();
        characterController = GetComponent<CharacterController>();
        characterJump = GetComponent<CharacterJump>();
        characterGravitable = GetComponent<CharacterGravitable>();
        characterCrouch = GetComponent<CharacterCrouch>();

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

        walkingState = new PlayerWalk(
            this,
            walkSettings,
            playerControls,
            transform,
            floorMovement,
            playerCamera,
            characterAim
            );

        runningState = new PlayerRun(
            this,
            runSettings,
            playerControls,
            transform,
            floorMovement,
            playerCamera,
            characterAim
            );

        jumpState = new PlayerJump(
            this,
            jumpSettings,
            playerControls,
            transform,
            floorMovement,
            playerCamera,
            characterJump,
            characterGravitable,
            characterAim,
            animator
            );

        fallState = new PlayerFall(
            this,
            fallSettings,
            playerControls,
            transform,
            floorMovement,
            playerCamera,
            characterAim,
            characterGravitable,
            animator);

        crouchIdleState = new CrouchIdle(
            this,
            crouchIdleSettings,
            playerControls,
            floorMovement,
            characterAim,
            animator,
            characterCrouch
            );

        crouchWalkState = new CrouchWalk(
            this,
            crouchWalkSettings,
            playerControls,
            transform,
            floorMovement,
            playerCamera,
            characterAim,
            characterCrouch,
            animator);
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

        animator.SetFloat("Vel", floorMovement.ActualVelocity.magnitude);

        if(!characterGravitable.IsGrounded && actualState != fallState)
        {
            fallTimer += Time.deltaTime;

            if(fallTimer >= fallTime && characterController.velocity.y < 0)
                ChangeState((int)PlayerStates.Fall);
        }
        else
            fallTimer = 0;
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
            case (int)PlayerStates.Fall:
                actualState = fallState;
                break;
            case (int)PlayerStates.CrouchIdle:
                actualState = crouchIdleState;
                break;
            case (int)PlayerStates.CrouchWalk:
                actualState = crouchWalkState;
                break;
            default:
                Debug.Log("Non existent state");
                ChangeState((int)PlayerStates.Idle);
                break;
        }

        actualState.EnterState();
    }
}
