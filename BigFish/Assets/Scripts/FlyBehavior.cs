using Movers;
using UnityEngine;

public class FlyBehavior : MonoBehaviour
{
    [SerializeField] private float _velocity = 1f;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject deadPrefab;
    [SerializeField] private float destroyDelay = 1f;  // Время, через которое уничтожается объект 2

    [SerializeField] private Transform cameraTransform;  // Ссылка на камеру
    [SerializeField] private float upperScreenThreshold = 0.75f; // Порог по высоте для сдвига фона (75% экрана)
    [SerializeField] private float lowerScreenThreshold = 0.25f; // Порог по высоте для возврата фона (25% экрана)
    [SerializeField] private float cameraMoveSpeed = 2f;  // Скорость движения фона

    private Rigidbody2D _rb;
    private Transform _tr;
    private Animator playerAnimator;

    private bool playGame = true;
    private float cameraMinY; // Минимальное значение по оси Y для камеры
    private float maxPlayerHeight; // Максимальная высота игрока

    private Vector3 velocity = Vector3.zero; // Переменная для плавного движения камеры
    private float smoothTime = 1f; // Время, за которое камера достигает целевой позиции
    private float predictionFactor = 10f; // Коэффициент предсказания позиции

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tr = GetComponent<Transform>();
        playerAnimator = GetComponent<Animator>();
        // Задаем минимальное положение камеры на старте
        cameraMinY = cameraTransform.position.y;
        // Получаем максимальную высоту из GameManager
        maxPlayerHeight = gameManager.GetMaxPlayerHeight();
    }

    void Update()
    {
        if (playGame)
        {
            maxPlayerHeight = gameManager.GetMaxPlayerHeight();
            if (Input.GetMouseButtonDown(0) && _tr.position.y < maxPlayerHeight)
            {
                _rb.velocity = Vector2.up * _velocity;
            }

            if (_tr.position.y <= -1f)
            {
                _rb.velocity = Vector2.up * 1.5f;
            }
            // Проверяем положение игрока относительно экрана
            HandleCameraMovement();
        }
    }
    private void HandleCameraMovement()
    {
        // Переводим позицию игрока из мировых координат в координаты экрана (0-1)
        Vector3 playerScreenPos = Camera.main.WorldToViewportPoint(_tr.position);

        // Рассчитываем целевую позицию камеры
        Vector3 targetPosition = cameraTransform.position;

        // Если игрок достигает верхней четверти экрана
        if (playerScreenPos.y >= upperScreenThreshold)
        {
            // Предсказываем дальнейшее движение игрока, смещая камеру выше его положения
            float predictedYPosition = _tr.position.y + (cameraMoveSpeed * predictionFactor);
            targetPosition = new Vector3(cameraTransform.position.x, predictedYPosition, cameraTransform.position.z);
        }
        // Если игрок опускается ниже нижней четверти экрана
        else if (playerScreenPos.y <= lowerScreenThreshold)
        {
            if (cameraTransform.position.y > cameraMinY)
            {
                // Предсказываем дальнейшее движение игрока вниз, но не опускаем камеру ниже cameraMinY
                float predictedYPosition = _tr.position.y - (cameraMoveSpeed * predictionFactor);
                targetPosition = new Vector3(cameraTransform.position.x, Mathf.Max(predictedYPosition, cameraMinY), cameraTransform.position.z);
            }
        }

        // Плавно перемещаем камеру в предсказанную позицию
        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, targetPosition, ref velocity, smoothTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int scoreAdd;
        int fishValue;

        if (playGame)
        {
            // Используем GameManager для проверки в библиотеке рыб
            if (!gameManager.TryGetFishValue(collision.gameObject.tag, out fishValue, out scoreAdd))
            {
                return;// Если это не рыба, выходим из метода
            }

            if (Score.instance.GetScore() < fishValue)
            {
                playGame = false; // Выключаем возможность collision с другими объектами
                playerAnimator.Play("PlayerDead");
            }
            else
            {
                playerAnimator.Play("PlayerEating");
                Score.instance.UpdateScore(scoreAdd);
                // Уничтожаем столкнувшийся объект
                Destroy(collision.gameObject);

                // Создаем "мертвый" объект (deadPrefab) на месте уничтоженной рыбы
                Vector3 spawnPosition = collision.gameObject.transform.position;
                GameObject newObject2 = Instantiate(deadPrefab, spawnPosition, Quaternion.identity);
                // Добавляем на объект (deadPrefab) количество заработанных очков
                UpdateDeadText(newObject2, scoreAdd);
                // Уничтожаем новый объект через определенное время
                Destroy(newObject2, destroyDelay);
            }
        }
    }

    private void UpdateDeadText(GameObject deadObject, int scoreAdd)
    {
        DeadScore deadScore = deadObject.GetComponent<DeadScore>();

        if (deadScore != null)
        {
            deadScore.ScoreValue(scoreAdd);
        }
    }

    public void GameOverPlayer() // Включается в конце анимации PlayerDead
    {
        gameManager.GameOver();
    }
}
