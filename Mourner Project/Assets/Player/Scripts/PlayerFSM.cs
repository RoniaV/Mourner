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
    CrouchWalk,
    BellIdle
}

[RequireComponent(typeof(CharacterFloorMovement))]
public class PlayerFSM : FSM
{
    [Header("Components")]
    [SerializeField] CharacterAim characterAim;
    [SerializeField] Transform playerCamera;
    [SerializeField] GameObject bellCamera;
    [SerializeField] Animator animator;
    [SerializeField] BellManager bellManager;
    [SerializeField] PlayerSoundManager soundManager;
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
    private BellIdle bellIdleState;

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
            animator,
            soundManager
            );

        walkingState = new PlayerWalk(
            this,
            walkSettings,
            playerControls,
            transform,
            floorMovement,
            playerCamera,
            characterAim,
            soundManager
            );

        runningState = new PlayerRun(
            this,
            runSettings,
            playerControls,
            transform,
            floorMovement,
            playerCamera,
            characterAim,
            soundManager
            );

        jumpState = new PlayerJump(
            this,
            jumpSettings,
            playerControls,
            transform,
            floorMovement,
            playerCamera,
            characterGravitable,
            characterAim,
            animator,
            characterJump,
            soundManager
            );

        fallState = new PlayerFall(
            this,
            fallSettings,
            playerControls,
            transform,
            floorMovement,
            playerCamera,
            characterGravitable,
            characterAim,
            animator,
            soundManager
            );

        crouchIdleState = new CrouchIdle(
            this,
            crouchIdleSettings,
            playerControls,
            floorMovement,
            characterAim,
            animator,
            characterCrouch,
            soundManager
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
            animator,
            soundManager
            );

        bellIdleState = new BellIdle(
            this,
            idleSettings,
            playerControls,
            floorMovement,
            characterAim,
            bellManager,
            bellCamera,
            animator,
            soundManager
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
            case (int)PlayerStates.BellIdle:
                actualState = bellIdleState;
                break;
            default:
                Debug.Log("Non existent state");
                ChangeState((int)PlayerStates.Idle);
                break;
        }

        actualState.EnterState();
    }
}
