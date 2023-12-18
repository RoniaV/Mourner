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
    Fall
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
    [Header("Variables")]
    [SerializeField] float fallTime = 1.5f;

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
    private PlayerFall fallState;

    private float fallTimer = 0;

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
            playerCamera,
            characterAim
            );

        runningState = new PlayerRunning(
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
            characterController,
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
            default:
                Debug.Log("Non existent state");
                ChangeState((int)PlayerStates.Idle);
                break;
        }

        actualState.EnterState();
    }
}
