using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movers
{
    public class Fish_5_Mover : FishMover
    {
        protected override void Mover()
        {
            _verticalSpeed = Mathf.Sin(Time.time);

            if (transform.position.y >= _maxVertical || transform.position.y <= _minVertical)
            {
                // ѕровер€ем, достигла ли рыба верхней границы и нижней границы
                if (transform.position.y >= _maxVertical)
                {
                    transform.position = new Vector3(transform.position.x, _maxVertical, transform.position.z);
                    _verticalSpeed = -Mathf.Abs(_verticalSpeed); // ƒвижение вниз после достижени€ верхней границы
                }
                else if (transform.position.y <= _minVertical)
                {
                    transform.position = new Vector3(transform.position.x, _minVertical, transform.position.z);
                    _verticalSpeed = Mathf.Abs(_verticalSpeed); // ƒвижение вверх после достижени€ нижней границы
                }
            }
        }
    }
}
