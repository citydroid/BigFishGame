using UnityEngine;

namespace Movers
{
    public class FishMover : MonoBehaviour
    {
        [SerializeField] private float minSpeed = 0.5f;
        [SerializeField] private float maxSpeed = 1.5f;
        private float _horizontalSpeed;
        private float _verticalSpeed = 0f;
        private readonly float targetX = -5f;

        void Start()
        {
            _horizontalSpeed = Random.Range(minSpeed, maxSpeed);
        }

        void Update()
        {
            transform.position += new Vector3(-_horizontalSpeed, _verticalSpeed, 0) * Time.deltaTime;
            // Уничтожаем объект, если он выходит за пределы экрана
            if (transform.position.x <= targetX)
            {
                Destroy(gameObject);
            }
        }
        // Публичный метод для изменения вертикальной скорости
        public void SetVerticalSpeed(float verticalSpeed)
        {
            _verticalSpeed = verticalSpeed;
        }

        // Публичный метод для изменения горизонтальной скорости
        public void SetHorizontalSpeed(float horizontalSpeed)
        {
            _horizontalSpeed = horizontalSpeed;
        }

        // Публичный метод для увеличения горизонтальной скорости на указанное значение
        public void IncreaseHorizontalSpeed(float increaseAmount)
        {
            _horizontalSpeed += increaseAmount;
        }
    }
}
