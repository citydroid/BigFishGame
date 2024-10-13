using UnityEngine;

namespace Movers
{
    public class Fish_3_Mover : FishMover
    {
        protected override void Mover()
        {
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

