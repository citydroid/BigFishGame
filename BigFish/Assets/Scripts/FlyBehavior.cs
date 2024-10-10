using Movers;
using TMPro;
using UnityEngine;

public class FlyBehavior : MonoBehaviour
{
    private float moveSpeed = 3f;  // Скорость перемещения
    private float lerpSpeed = 0.03f;  // Коэффициент для плавного движения
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject deadPrefab;
    [SerializeField] private float destroyDelay = 1f;  // Время, через которое уничтожается объект

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
    private float extraHeight = 0f; // Дополнительная высота, которую Плеер может преодолеть над maxPlayerHeight
    private bool isJump = false; // Флаг, указывающий, находится ли Плеер выше уровня воды
    private bool isFalling = true; // Флаг, указывающий, падает ли Плеер вниз

    private Vector3 velocity = Vector3.zero; // Переменная для плавного движения камеры
    private float smoothTime = 1f; // Время, за которое камера достигает целевой позиции
    private float predictionFactor = 10f; // Коэффициент предсказания позиции

    private Vector3 lastPosition; // Переменная для отслеживания предыдущей позиции Плеера
    private bool isMovingRight = true; // Переменная для отслеживания направления движения

    private float minX, maxX; // Границы по оси X

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tr = GetComponent<Transform>();
        playerAnimator = GetComponent<Animator>();

        // Задаем минимальное положение камеры на старте
        cameraMinY = cameraTransform.position.y;

        // Получаем максимальную высоту из GameManager
        maxPlayerHeight = gameManager.GetMaxPlayerHeight();

        // Сохраняем начальную позицию Плеера
        lastPosition = _tr.position;

        // Рассчитываем границы экрана по оси X в мировых координатах
        CalculateScreenBounds();
    }
    private void Update()
    {
        if (playGame)
        {
            // Постоянно обновляем максимальную высоту из GameManager
            maxPlayerHeight = gameManager.GetMaxPlayerHeight();

            // Проверяем положение игрока относительно экрана
           HandleCameraMovement();  
            

                // Управление мышью
                HandleMouseInput();

               // HandleVerticalBoundary();
 
        }
    }
    private void HandleMouseInput()
    {
        // Получаем позицию мыши в мировых координатах
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Вычисляем новое положение Плеера
        Vector3 targetPosition = _tr.position;

        // Интерполируем позицию с замедлением с помощью lerpSpeed
        targetPosition.x = Mathf.Lerp(_tr.position.x, mousePosition.x, lerpSpeed);
        targetPosition.y = Mathf.Lerp(_tr.position.y, mousePosition.y, lerpSpeed);

        // Ограничиваем движение по оси X границами экрана
        targetPosition.x = Mathf.Clamp(targetPosition.x, -10f, 10f); // Примерные границы по X, можно подстроить
        // Ограничиваем движение по высоте в зависимости от maxPlayerHeight
        targetPosition.y = Mathf.Clamp(targetPosition.y, -1f, maxPlayerHeight);

        _tr.position = targetPosition;
    }
/*
    private void HandleVerticalBoundary()
    {
        float playerY = _tr.position.y;
        Vector3 targetPosition = _tr.position;

        // Если Плеер поднимается выше maxPlayerHeight
        if (playerY > maxPlayerHeight)
        {

            Debug.Log("Игрок выше максимальной высоты!");

            // Если Плеер ещё не начал падать
            if (!isFalling)
            {
                // Рассчитываем, насколько выше maxPlayerHeight Плеер может подняться
             //   float overshootHeight = Mathf.Clamp((playerY - maxPlayerHeight) * 0.4f, 0, 1f); // Максимум 2 единицы выше
                extraHeight = maxPlayerHeight + 0.5f; // Дополнительная высота
                targetPosition.y = Mathf.Clamp(targetPosition.y, -1f, extraHeight + 2f);
                // Если Плеер поднимается выше разрешённой высоты, он начинает падать
                if (playerY >= extraHeight)
                {
                    isFalling = true;
                }
                _tr.position = targetPosition;
            }
            else
            {
                // Если Плеер начал падать, плавно замедляем его движение
                _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Lerp(_rb.velocity.y, -1f, Time.deltaTime * 2f));

                // Если Плеер падает ниже maxPlayerHeight, останавливаем падение
                if (playerY <= maxPlayerHeight)
                {
                    _tr.position = new Vector3(_tr.position.x, maxPlayerHeight, _tr.position.z);
                    _rb.velocity = Vector2.zero; // Останавливаем вертикальную скорость
                    isFalling = false; // Останавливаем падение
                    isJump = false; // Переходим обратно в состояние, когда можно двигаться
                }
            }
        }
    }
*/
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

    private void HandleAnimationBasedOnDirection()
    {
        // Определяем направление движения Плеера по оси X
        float currentX = _tr.position.x;
        float lastX = lastPosition.x;

        // Если Плеер движется вправо
        if (currentX > lastX && !isMovingRight)
        {
            isMovingRight = true;
            // Включаем анимацию №1 (движение вправо)
            playerAnimator.Play("MoveRightAnimation");
        }
        // Если Плеер движется влево
        else if (currentX < lastX && isMovingRight)
        {
            isMovingRight = false;
            // Включаем анимацию №2 (движение влево)
            playerAnimator.Play("MoveLeftAnimation");
        }

        // Обновляем lastPosition для сравнения на следующем кадре
        lastPosition = _tr.position;
    }

    // Рассчитываем границы экрана по оси X
    private void CalculateScreenBounds()
    {
        Camera mainCamera = Camera.main;

        // Левая граница экрана
        Vector3 leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        minX = leftEdge.x + 0.2f;

        // Правая граница экрана
        Vector3 rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane));
        maxX = rightEdge.x - 0.2f;
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
                return; // Если это не рыба, выходим из метода
            }

            if (Score.instance.GetScore() < fishValue)
            {
                playGame = false; // Выключаем возможность столкновений с другими объектами
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
