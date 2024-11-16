using TMPro;
using UnityEngine;

namespace Movers
{
    public interface IMovable
    {
        void Move();
    }

    public interface IDirectionChanger
    {
        void ChangeDirection();
    }

    public class FishMover : MonoBehaviour
    {
        private readonly bool playGame = true;

        [SerializeField] private float minSpeed = 0.5f;
        [SerializeField] private float maxSpeed = 1.5f;
        protected float _horizontalSpeed;

        [SerializeField] private float minVertSpeed = 0f;
        [SerializeField] private float maxVertSpeed = 0f;
        protected float _verticalSpeed;

        [SerializeField] private Transform fishTransform;
        protected bool mirrorFish = false;

        protected GameManager gameManager;
        protected readonly float targetLeftX = -5f;
        protected readonly float targetRightX = 5f;
        protected float _maxVertical;
        protected float _minVertical = -1f;

        private void Awake()
        {
            GameObject gameManagerObject = GameObject.Find("GameManager"); 

            if (gameManagerObject != null)
                gameManager = gameManagerObject.GetComponent<GameManager>();
        }
        void Start()
        {
            _horizontalSpeed = Random.Range(minSpeed, maxSpeed);
            _verticalSpeed = Random.Range(minVertSpeed, maxVertSpeed);
        }

        void Update()
        {
            if (playGame)
            {
                _maxVertical = gameManager.GetMaxPlayerHeight();
// Debug.Log("_maxVertical " + _maxVertical);
                Mover();

                MirrorFish(_horizontalSpeed);

                transform.position += new Vector3(-_horizontalSpeed, _verticalSpeed, 0) * Time.deltaTime;

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
            bool shouldMirror = (horizontalSpeed > 0 && mirrorFish) || (horizontalSpeed < 0 && !mirrorFish);

            if (shouldMirror)
            {
                Vector3 currentScale = fishTransform.localScale;
                currentScale.x = -currentScale.x;
                fishTransform.localScale = currentScale;
                mirrorFish = !mirrorFish;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("NetCollider"))
            {
                Debug.Log("Collision with NetCollider detected.");
                _horizontalSpeed = 0;
                _verticalSpeed = 0;
            }
        }
            // Публичный метод для изменения вертикальной скорости
        public void SetVerticalSpeed(float verticalSpeed)
        {
            _verticalSpeed = verticalSpeed;
        }

        // Публичный метод для увеличения горизонтальной скорости на указанное значение
        public void IncreaseHorizontalSpeed(float increaseAmount)
        {
            _horizontalSpeed += increaseAmount;
        }
    }
}
