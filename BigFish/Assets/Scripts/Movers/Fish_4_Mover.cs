using UnityEngine;

namespace Movers
{
    public class Fish_4_Mover : FishMover
    {
        protected override void Mover()
        {
            _verticalSpeed = Mathf.Sin(Time.time);

            if (transform.position.y >= _maxVertical || transform.position.y <= _minVertical)
            {
                // ���������, �������� �� ���� ������� ������� � ������ �������
                if (transform.position.y >= _maxVertical)
                {
                    transform.position = new Vector3(transform.position.x, _maxVertical, transform.position.z);
                    _verticalSpeed = -Mathf.Abs(_verticalSpeed); // �������� ���� ����� ���������� ������� �������
                }
                else if (transform.position.y <= _minVertical)
                {
                    transform.position = new Vector3(transform.position.x, _minVertical, transform.position.z);
                    _verticalSpeed = Mathf.Abs(_verticalSpeed); // �������� ����� ����� ���������� ������ �������
                }
            }
        }
    }
}
