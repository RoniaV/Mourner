using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputSettings;

public class BellIdle : State
{
    private BellIdleSettings bellSettings;
    private PlayerControls playerControls;
    private CharacterFloorMovement characterFloorMovement;
    private CharacterAim characterAim;
    private BellManager bellManager;
    private GameObject bellCamera;
    private Animator animator;
    private PlayerSoundManager soundManager;
    private Transform player;

    private bool turned = false;
    private Vector3 startCameraDir = Vector3.zero;
    private Vector3 smoothDirVel = Vector3.zero;

    public BellIdle(FSM fSM,
        BellIdleSettings bellSettings,
        PlayerControls playerControls,
        CharacterFloorMovement characterFloorMovement,
        CharacterAim characterAim,
        BellManager bellManager,
        GameObject bellCamera,
        Animator animator,
        PlayerSoundManager soundManager,
        Transform player
        ) : base(fSM)
    {
        this.bellSettings = bellSettings;
        this.playerControls = playerControls;
        this.characterFloorMovement = characterFloorMovement;
        this.characterAim = characterAim;
        this.bellManager = bellManager;
        this.bellCamera = bellCamera;
        this.animator = animator;
        this.soundManager = soundManager;
        this.player = player;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Bell Idle State");

        startCameraDir = Camera.main.transform.forward;
        startCameraDir.y = 0;
        startCameraDir = startCameraDir.normalized;

        characterFloorMovement.SetVelocity(GetPlayerVel());
        characterFloorMovement.SetMovementDirection(startCameraDir);
        characterAim.RotateCharacter(Vector2.zero);
        bellCamera.SetActive(true);

        //Do after turn
        fSM.StartCoroutine(TurnCoroutine());
    }

    public override void ExitState()
    {
        Debug.Log("Exit Bell Idle State");

        //bellManager.PutHandIn();
        bellManager.SetBellActive(false);
        bellCamera.SetActive(false);

        turned = false;
        startCameraDir = Vector3.zero;
        smoothDirVel = Vector3.zero;

        fSM.StopAllCoroutines();
    }

    public override void FixedUpdateState()
    {

    }

    public override void UpdateState()
    {
        if (turned)
        {
            bellManager.MoveHand(playerControls.Gameplay.Aim.ReadValue<Vector2>());

            if (!playerControls.Gameplay.MoveBell.IsPressed())
            {
                fSM.ChangeState((int)PlayerStates.Idle);
            }
            else if (!playerControls.Gameplay.BellOut.IsPressed())
            {
                fSM.ChangeState((int)PlayerStates.Idle);
            }
        }
    }

    private IEnumerator TurnCoroutine()
    {
        //Rotate towards the main camera start position
        Quaternion targetRotation = Quaternion.LookRotation(startCameraDir, Vector3.up);
        Vector3 movDirection = startCameraDir;

        do
        {
            // Smoothly rotate towards the target rotation
            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * bellSettings.TurnSpeed);
            movDirection = Vector3.SmoothDamp(movDirection, Vector3.zero, ref smoothDirVel, bellSettings.SmoothTime);

            characterFloorMovement.SetMovementDirection(movDirection);

            yield return null;
        } while (Quaternion.Angle(player.rotation, targetRotation) > 1f);

        Debug.Log("End turning");
        turned = true;
        player.rotation = targetRotation;
        characterFloorMovement.SetVelocity(0);

        soundManager.StopFootstepSound();
        //bellManager.PutHandOut();
        bellManager.SetBellActive(true);
    }

    private float GetPlayerVel()
    {
        float desiredVel = 0;

        float dot = Vector3.Dot(startCameraDir, player.forward);
        float t = Mathf.InverseLerp(1, -1, dot);
        desiredVel = Mathf.Lerp(0, bellSettings.StepVel, t);
        Debug.Log(desiredVel);

        return desiredVel;
    }
}
