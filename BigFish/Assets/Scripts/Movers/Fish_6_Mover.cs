using Movers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_6_Mover : FishMover
{
    private float changeDirectionTime = 20f;
    private float currentTime = 0f;
    protected override void Mover()
    {
        currentTime += Time.deltaTime;

        // ѕериодическа€ смена направлени€ движени€
        if (currentTime >= changeDirectionTime)
        {
            _horizontalSpeed = -_horizontalSpeed;
            _verticalSpeed = -_verticalSpeed;
            currentTime = 0f;
        }
    }
}
