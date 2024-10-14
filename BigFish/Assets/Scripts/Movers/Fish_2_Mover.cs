using UnityEngine;

namespace Movers
{
    public class Fish_2_Mover : FishMover
    {
        private float changeDirectionTime = 20f;
        private float currentTime = 0f;
        protected override void Mover()
        {
            currentTime += Time.deltaTime;

            // ������������� ����� ����������� ��������
            if (currentTime >= changeDirectionTime)
            {
                _horizontalSpeed = -_horizontalSpeed;
                _verticalSpeed = -_verticalSpeed;
                currentTime = 0f;
            }
        }
    }
}
