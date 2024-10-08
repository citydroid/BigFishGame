using UnityEngine;

namespace Movers
{
    public class FishMover : MonoBehaviour
    {
        [SerializeField] private int fishNumber = 0;

        [SerializeField] private float minSpeed = 0.5f;
        [SerializeField] private float maxSpeed = 1.5f;
        private float _horizontalSpeed;

        [SerializeField] private float minVertSpeed = 0f;
        [SerializeField] private float maxVertSpeed = 0f;
        private float _verticalSpeed;

        private readonly float targetX = -5f;
        private float _maxVertical;
        private float _minVertical = -1f;  // ћинимальна€ допустима€ высота
        private float depthCoeff = 0.0002f;

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
                // ѕровер€ем, достигла ли рыба верхней границы
                if (transform.position.y >= _maxVertical)
                {
                    transform.position = new Vector3(transform.position.x, _maxVertical, transform.position.z);
                    _verticalSpeed = -Mathf.Abs(_verticalSpeed); // ƒвижение вниз после достижени€ верхней границы
                }
                // ѕровер€ем, достигла ли рыба нижней границы
                else if (transform.position.y <= _minVertical)
                {
                    transform.position = new Vector3(transform.position.x, _minVertical, transform.position.z);
                    _verticalSpeed = Mathf.Abs(_verticalSpeed); // ƒвижение вверх после достижени€ нижней границы
                }

                // Ћогика дл€ конкретных рыб по их номерам
                if (fishNumber == 10)
                {
                    _verticalSpeed = 0; // ƒл€ рыбы с номером 10 вертикальное движение останавливаетс€
                }
                else if (fishNumber == 50 || fishNumber == 150)
                {
                    // –ыбы с номерами 50 и 150 продолжают движение вверх-вниз
                    if (transform.position.y >= _maxVertical || transform.position.y <= _minVertical)
                    {
                        _verticalSpeed = -_verticalSpeed;  // ћен€ем направление движени€
                    }
                }

                // ќбновл€ем позицию рыбы с учетом горизонтальной и вертикальной скорости
                transform.position += new Vector3(-_horizontalSpeed, _verticalSpeed, 0) * Time.deltaTime;

                // ”ничтожаем объект, если он выходит за пределы экрана
                if (transform.position.x <= targetX)
                {
                    Destroy(gameObject);
                }
            }
        }

        // ѕубличный метод дл€ задани€ максимальной высоты
        public void SetMaxVertical(float maxValue)
        {
            _maxVertical = maxValue - 0.2f;  // — небольшим отступом от заданной высоты
        }
        public void SetDepth(float maxVertical, float depthValue)
        {
            _maxVertical = maxVertical - 0.1f;
            depthCoeff = depthValue;  // — небольшим отступом от заданной высоты
        }
        // ѕубличный метод дл€ изменени€ вертикальной скорости
        public void SetVerticalSpeed(float verticalSpeed)
        {
            _verticalSpeed = verticalSpeed;
        }

        // ѕубличный метод дл€ изменени€ горизонтальной скорости
        public void SetHorizontalSpeed(float horizontalSpeed)
        {
            _horizontalSpeed = horizontalSpeed;
        }

        // ѕубличный метод дл€ увеличени€ горизонтальной скорости на указанное значение
        public void IncreaseHorizontalSpeed(float increaseAmount)
        {
            _horizontalSpeed += increaseAmount;
        }
    }
}
