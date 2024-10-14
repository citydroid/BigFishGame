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
        protected bool mirrorFish = false;

        protected readonly float targetLeftX = -5f;
        protected readonly float targetRightX = 5f;
        protected float _maxVertical;
        protected float _minVertical = -1f;
        protected float depthCoeff = 0.0002f;

        [SerializeField] private Transform fishTransform;

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

                MirrorFish(_horizontalSpeed);

                // ќбновл€ем позицию рыбы с учетом горизонтальной и вертикальной скорости
                transform.position += new Vector3(-_horizontalSpeed, _verticalSpeed, 0) * Time.deltaTime;

                // ”ничтожаем объект, если он выходит за пределы экрана
                if (transform.position.x <= targetLeftX || transform.position.x >= targetRightX)
                {
                    Destroy(gameObject);
                }
            }
        }

        protected virtual void Mover()
        {
            _verticalSpeed = 0;
        }
        private void MirrorFish(float horizontalSpeed)
        {
            if (horizontalSpeed > 0 && mirrorFish)
            {
                Vector3 currentScale = fishTransform.localScale;
                currentScale.x = -currentScale.x;
                fishTransform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);
                mirrorFish = false;
            }
            else if (horizontalSpeed < 0 && !mirrorFish)
            {
                Vector3 currentScale = fishTransform.localScale;
                currentScale.x = -currentScale.x;
                fishTransform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);
                mirrorFish = true;
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

        // ѕубличный метод дл€ увеличени€ горизонтальной скорости на указанное значение
        public void IncreaseHorizontalSpeed(float increaseAmount)
        {
            _horizontalSpeed += increaseAmount;
        }
    }
}
