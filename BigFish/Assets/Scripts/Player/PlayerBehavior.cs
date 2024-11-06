using Movers;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DeadManager deadManager;
    [SerializeField] private PlayerAppearance playerAppearance;
    [SerializeField] private CameraController cameraController;

    private Transform _tr;
    private bool playGame = true;
    private float maxPlayerHeight;
    private Vector3 lastPosition;
    private readonly float lerpSpeed = 0.03f;

    private bool isBouncing = false;
    private bool isbounceHeight = false;
    private float bounceHeight = 0.2f; // Высота подъема над maxPlayerHeight
    private float bounceSpeed = 2.0f; // Скорость подъема и спуска

    private void Start()
    {
        _tr = GetComponent<Transform>();

        if (playerAppearance == null)
            playerAppearance = GetComponentInChildren<PlayerAppearance>();

        if (cameraController == null)
            cameraController = FindObjectOfType<CameraController>();

        maxPlayerHeight = gameManager.GetMaxPlayerHeight();
        lastPosition = _tr.position;
    }
    private void Update()
    {
        if (playGame)
        {
            maxPlayerHeight = gameManager.GetMaxPlayerHeight();

            cameraController.UpdateCameraPosition(_tr.position);
            playerAppearance.MirrorPlayer(lastPosition.x, _tr.position.x);
            lastPosition = _tr.position;

            if (isBouncing)
            {
             //   HandleBounceMovement();
            }
            else
            {
                HandleMouseInput();
            }
        }
    }
    private void HandleMouseInput()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 targetPosition = _tr.position;
        targetPosition.x = Mathf.Lerp(_tr.position.x, mousePosition.x, lerpSpeed);
        targetPosition.y = Mathf.Lerp(_tr.position.y, mousePosition.y, lerpSpeed);

        targetPosition.x = Mathf.Clamp(targetPosition.x, cameraController.MinX, cameraController.MaxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -1f, maxPlayerHeight);

        _tr.position = targetPosition;
        // Начать подъем, если достигли максимальной высоты
        /*
        if (_tr.position.y >= maxPlayerHeight)
        {
            isBouncing = true;
        }*/
    }
    /*
    private void HandleBounceMovement()
    {
        float targetY = _tr.position.y;

        // Поднимаемся вверх на bounceHeight над maxPlayerHeight
        if (_tr.position.y < (maxPlayerHeight + bounceHeight) && !isbounceHeight)
        {
            targetY = Mathf.MoveTowards(_tr.position.y, maxPlayerHeight + bounceHeight, bounceSpeed * Time.deltaTime);
        }
        else if (_tr.position.y >= (maxPlayerHeight + bounceHeight) && !isbounceHeight)
        {
            isbounceHeight = true;
        }
        else if (_tr.position.y > (maxPlayerHeight - 0.1f) && isbounceHeight)
        {
            targetY = Mathf.MoveTowards(_tr.position.y, maxPlayerHeight - 0.1f, 0.005f);
        }
        else if (_tr.position.y <= maxPlayerHeight - 0.1f && isbounceHeight)
        {
            // Завершаем подъем и спуск, возвращаем управление мышью
            isBouncing = false;
            isbounceHeight = false;
            return;
        }

        // Обновляем позицию игрока
        _tr.position = new Vector3(_tr.position.x, targetY, _tr.position.z);
    }
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int scoreAdd;
        int fishValue;

        if (playGame)
        {  
            if (collision.CompareTag("NetCollider"))
            {
                Debug.Log("Collision with NetCollider detected.");
            }

            if (!gameManager.TryGetFishValue(collision.gameObject.tag, out fishValue, out scoreAdd))
                return;

            if (Score.instance.GetScore() < fishValue)
            {
                playGame = false;
                playerAppearance.PlayAnimation("PlayerDead");
            }
            else
            {
                playerAppearance.PlayAnimation("PlayerEating");
                Score.instance.UpdateScore(scoreAdd);
                Destroy(collision.gameObject);
            /*    FishMover fishMover = collision.gameObject.GetComponent<FishMover>();
                if (fishMover != null)
                {
                    fishMover.IncreaseHorizontalSpeed(0.3f); 
                }*/
                deadManager.SpawnDeadFish(collision.gameObject.transform.position, scoreAdd);
            }
        }
    }
}
