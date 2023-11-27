using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformGravitable : GravitableObject
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        transform.position += velocity;
    }
}
