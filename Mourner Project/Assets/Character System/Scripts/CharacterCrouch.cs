using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterFloorMovement), typeof(CharacterController))]
public class CharacterCrouch : MonoBehaviour
{
    public bool Crouched { get; protected set; }

    [SerializeField] float crouchedVelocity = 1;

    protected CharacterFloorMovement charMovement;
    protected CharacterController charController;

    protected float originalHeight;

    protected virtual void Awake()
    {
        charMovement = GetComponent<CharacterFloorMovement>();
        charController = GetComponent<CharacterController>();
    }

    protected virtual void Start()
    {
        originalHeight = charController.height;
    }

    public void ChangeCrouchState()
    {
        Crouched = !Crouched;

        charController.height = Crouched ? originalHeight / 2 : originalHeight;
        charMovement.ModifyVelocity(Crouched ? crouchedVelocity : 0);
    }
}
