using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterFloorMovement))]
public class PlayerFSM : FSM
{
    public enum PlayerStates
    {
        Walking,
        Running
    }

    [Header("Components")]
    [SerializeField] CharacterAim characterAim;
    [SerializeField] Transform playerCamera;
    [SerializeField] Animator characterAnimator;
    [Header("State Settings")]
    [SerializeField] PlayerStates initialState = PlayerStates.Walking;
    [Header("Walk Settings")]
    [SerializeField] InputAction walkingAction;
    [SerializeField] InputAction aimAction;
    [SerializeField] float smoothTime = .5f;

    CharacterFloorMovement floorMovement;

    private State actualState;
    //States
    private PlayerWalking walkingState;


    void Awake()
    {
        floorMovement = GetComponent<CharacterFloorMovement>();
    }

    void Start()
    {
        walkingState = new PlayerWalking(
            this,
            transform,
            walkingAction,
            aimAction,
            smoothTime,
            floorMovement,
            characterAim,
            playerCamera,
            characterAnimator);

        ChangeState((int)initialState);
    }

    void FixedUpdate()
    {
        actualState?.FixedUpdateState();
    }

    void Update()
    {
        actualState?.UpdateState();
    }

    public override void ChangeState(int state)
    {
        if (actualState != null)
            actualState.ExitState();

        switch (state)
        {
            case (int)PlayerStates.Walking:
                actualState = walkingState;
                actualState.EnterState();
                break;
            case (int)PlayerStates.Running:
                break;
            default:
                Debug.Log("Non existent state");
                ChangeState((int)PlayerStates.Walking);
                break;
        }
    }
}
