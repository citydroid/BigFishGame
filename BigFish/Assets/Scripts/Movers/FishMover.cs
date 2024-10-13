using UnityEngine;

namespace Movers
{
    public class FishMover : MonoBehaviour
    {
        [SerializeField] private float minSpeed = 0.5f;
        [SerializeField] private float maxSpeed = 1.5f;
        protected float _horizontalSpeed;

        [SerializeField] private float minVertSpeed = 0f;
        [SerializeField] private float maxVertSpeed = 0f;
        protected float _verticalSpeed;

        protected readonly float targetX = -5f;
        protected float _maxVertical;
        protected float _minVertical = -1f;
        protected float depthCoeff = 0.0002f;

        private bool playGame = true;

        void Start()
        {
            _horizontalSpeed = Random.Range(minSpeed, maxSpeed);
            _verticalSpeed = Random.Range(minVertSpeed, maxVertSpeed);
        }

        void Update()
        {
            if (playGame)
            {
                _maxVertical += depthCoeff;

                Mover();

                // ��������� ������� ���� � ������ �������������� � ������������ ��������
                transform.position += new Vector3(-_horizontalSpeed, _verticalSpeed, 0) * Time.deltaTime;

                // ���������� ������, ���� �� ������� �� ������� ������
                if (transform.position.x <= targetX)
                {
                    Destroy(gameObject);
                }
            }
        }

        protected virtual void Mover()
        {
            _verticalSpeed = 0;
        }



        // ��������� ����� ��� ������� ������������ ������
        public void SetMaxVertical(float maxValue)
        {
            _maxVertical = maxValue - 0.2f;  // � ��������� �������� �� �������� ������
        }
        public void SetDepth(float maxVertical, float depthValue)
        {
            _maxVertical = maxVertical - 0.1f;
            depthCoeff = depthValue;  // � ��������� �������� �� �������� ������
        }
        // ��������� ����� ��� ��������� ������������ ��������
        public void SetVerticalSpeed(float verticalSpeed)
        {
            _verticalSpeed = verticalSpeed;
        }

        // ��������� ����� ��� ���������� �������������� �������� �� ��������� ��������
        public void IncreaseHorizontalSpeed(float increaseAmount)
        {
            _horizontalSpeed += increaseAmount;
        }
    }
}
