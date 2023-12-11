using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterFloorMovement))]
public class PlayerFSM : FSM
{
    public enum PlayerStates
    {
        Idle,
        Walking,
        Running
    }

    [Header("Components")]
    [SerializeField] CharacterAim characterAim;
    [SerializeField] Transform playerCamera;
    [SerializeField] Animator characterAnimator;
    [Header("State Settings")]
    [SerializeField] PlayerStates initialState = PlayerStates.Idle;
    [SerializeField] IdleSettings idleSettings;
    [SerializeField] WalkSettings walkSettings;
    [SerializeField] RunSettings runSettings;

    CharacterController characterController;
    CharacterFloorMovement floorMovement;
    PlayerControls playerControls;

    private State actualState;
    //States
    private PlayerIdle idleState;
    private PlayerWalking walkingState;
    private PlayerRunning runningState;


    void Awake()
    {
        floorMovement = GetComponent<CharacterFloorMovement>();
        characterController = GetComponent<CharacterController>();

        playerControls = new PlayerControls();
    }

    void Start()
    {
        idleState = new PlayerIdle(
            this,
            idleSettings,
            playerControls,
            floorMovement,
            characterAim,
            characterAnimator
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
        characterAnimator.SetFloat("Vel", floorMovement.ActualVelocity.magnitude);
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
            default:
                Debug.Log("Non existent state");
                ChangeState((int)PlayerStates.Idle);
                break;
        }

        actualState.EnterState();
    }
}
