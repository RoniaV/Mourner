using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAim : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] Transform body;
    [Header("Rotation Settings")]
    [SerializeField] float maxHeadAngle = 90;
    [SerializeField] float sensitivity = 1f;
    [SerializeField] bool flipY;

    protected Vector2 aim;

    protected virtual void FixedUpdate()
    {
        body.Rotate(
            0,
            aim.x * sensitivity * Time.fixedDeltaTime,
            0, Space.Self);
        head.Rotate(
            aim.y * sensitivity * Time.fixedDeltaTime,
            0,
            0, Space.Self);

        ClampHeadRotation();
    }

    public void RotateCharacter(Vector2 direction)
    {
        aim = direction;

        aim.y = flipY ? -aim.y : aim.y;
    }

    private void ClampHeadRotation()
    {
        Quaternion clampedHeadRotation = head.localRotation;
        clampedHeadRotation = Quaternion.Euler(
            ClampAngle(clampedHeadRotation.eulerAngles.x, -maxHeadAngle, maxHeadAngle),
            0, 0);

        head.localRotation = clampedHeadRotation;
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < 0f) { angle = 360 + angle; }
        if (angle > 180f) { return Mathf.Max(angle, 360 + min); }
        return Mathf.Min(angle, max);
    }
}
