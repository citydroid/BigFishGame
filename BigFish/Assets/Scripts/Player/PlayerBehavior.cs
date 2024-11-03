using Movers;
using TMPro;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private readonly float lerpSpeed = 0.03f;  // ����������� ��� �������� ��������

    [SerializeField] private GameManager gameManager;
    [SerializeField] private DeadManager deadManager;
    [SerializeField] private PlayerAppearance playerAppearance;
    [SerializeField] private Transform cameraTransform;

    private readonly float upperScreenThreshold = 0.75f; // ����� �� ������ ��� ������ ���� (75% ������)
    private readonly float lowerScreenThreshold = 0.25f; // ����� �� ������ ��� �������� ���� (25% ������)
    private readonly float cameraMoveSpeed = 2f;  // �������� �������� ������

    private Rigidbody2D _rb;
    private Transform _tr;

    private bool playGame = true;
    private float cameraMinY; // ����������� �������� �� ��� Y ��� ������
    private float maxPlayerHeight; // ������������ ������ ������
    private float extraHeight = 0f; // �������������� ������, ������� ����� ����� ���������� ��� maxPlayerHeight
    private bool isJump = false; // ����, �����������, ��������� �� ����� ���� ������ ����
    private bool isFalling = true; // ����, �����������, ������ �� ����� ����

    private Vector3 velocity = Vector3.zero; // ���������� ��� �������� �������� ������
    private readonly float smoothTime = 1f; // �����, �� ������� ������ ��������� ������� �������
    private readonly float predictionFactor = 10f; // ����������� ������������ �������

    private Vector3 lastPosition; // ���������� ��� ������������ ���������� ������� ������
    private float minX, maxX; // ������� �� ��� X

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tr = GetComponent<Transform>();

        if (playerAppearance == null)
        {
            playerAppearance = GetComponentInChildren<PlayerAppearance>();
        }
        // ������ ����������� ��������� ������ �� ������
        cameraMinY = cameraTransform.position.y;

        // �������� ������������ ������ �� GameManager
        maxPlayerHeight = gameManager.GetMaxPlayerHeight();

        // ��������� ��������� ������� ������
        lastPosition = _tr.position;

        // ������������ ������� ������ �� ��� X � ������� �����������
        CalculateScreenBounds();
    }
    private void Update()
    {
        if (playGame)
        {
            // ��������� ��������� ������������ ������ �� GameManager
            maxPlayerHeight = gameManager.GetMaxPlayerHeight();

            // ��������� ��������� ������ ������������ ������
            HandleCameraMovement();
            playerAppearance.MirrorPlayer(lastPosition.x, _tr.position.x);
            lastPosition = _tr.position;
            // ���������� �����
            HandleMouseInput();

         // HandleVerticalBoundary();
        }
    }
    private void HandleMouseInput()
    {
        // �������� ������� ���� � ������� �����������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ��������� ����� ��������� ������
        Vector3 targetPosition = _tr.position;

        // ������������� ������� � ����������� � ������� lerpSpeed
        targetPosition.x = Mathf.Lerp(_tr.position.x, mousePosition.x, lerpSpeed);
        targetPosition.y = Mathf.Lerp(_tr.position.y, mousePosition.y, lerpSpeed);

        // ������������ �������� �� ��� X ��������� ������
        targetPosition.x = Mathf.Clamp(targetPosition.x, -2.2f, 2.2f); // ��������� ������� �� X, ����� ����������
        // ������������ �������� �� ������ � ����������� �� maxPlayerHeight
        targetPosition.y = Mathf.Clamp(targetPosition.y, -1f, maxPlayerHeight);

        _tr.position = targetPosition;
    }
/*
    private void HandleVerticalBoundary()
    {
        float playerY = _tr.position.y;
        Vector3 targetPosition = _tr.position;

        // ���� ����� ����������� ���� maxPlayerHeight
        if (playerY > maxPlayerHeight)
        {

            Debug.Log("����� ���� ������������ ������!");

            // ���� ����� ��� �� ����� ������
            if (!isFalling)
            {
                // ������������, ��������� ���� maxPlayerHeight ����� ����� ���������
             //   float overshootHeight = Mathf.Clamp((playerY - maxPlayerHeight) * 0.4f, 0, 1f); // �������� 2 ������� ����
                extraHeight = maxPlayerHeight + 0.5f; // �������������� ������
                targetPosition.y = Mathf.Clamp(targetPosition.y, -1f, extraHeight + 2f);
                // ���� ����� ����������� ���� ����������� ������, �� �������� ������
                if (playerY >= extraHeight)
                {
                    isFalling = true;
                }
                _tr.position = targetPosition;
            }
            else
            {
                // ���� ����� ����� ������, ������ ��������� ��� ��������
                _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Lerp(_rb.velocity.y, -1f, Time.deltaTime * 2f));

                // ���� ����� ������ ���� maxPlayerHeight, ������������� �������
                if (playerY <= maxPlayerHeight)
                {
                    _tr.position = new Vector3(_tr.position.x, maxPlayerHeight, _tr.position.z);
                    _rb.velocity = Vector2.zero; // ������������� ������������ ��������
                    isFalling = false; // ������������� �������
                    isJump = false; // ��������� ������� � ���������, ����� ����� ���������
                }
            }
        }
    }
*/
    private void HandleCameraMovement()
    {
        // ��������� ������� ������ �� ������� ��������� � ���������� ������ (0-1)
        Vector3 playerScreenPos = Camera.main.WorldToViewportPoint(_tr.position);

        // ������������ ������� ������� ������
        Vector3 targetPosition = cameraTransform.position;

        // ���� ����� ��������� ������� �������� ������
        if (playerScreenPos.y >= upperScreenThreshold)
        {
            // ������������� ���������� �������� ������, ������ ������ ���� ��� ���������
            float predictedYPosition = _tr.position.y + (cameraMoveSpeed * predictionFactor);
            targetPosition = new Vector3(cameraTransform.position.x, predictedYPosition, cameraTransform.position.z);
        }
        // ���� ����� ���������� ���� ������ �������� ������
        else if (playerScreenPos.y <= lowerScreenThreshold)
        {
            if (cameraTransform.position.y > cameraMinY)
            {
                // ������������� ���������� �������� ������ ����, �� �� �������� ������ ���� cameraMinY
                float predictedYPosition = _tr.position.y - (cameraMoveSpeed * predictionFactor);
                targetPosition = new Vector3(cameraTransform.position.x, Mathf.Max(predictedYPosition, cameraMinY), cameraTransform.position.z);
            }
        }

        // ������ ���������� ������ � ������������� �������
        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, targetPosition, ref velocity, smoothTime);
    }
    // ������������ ������� ������ �� ��� X
    private void CalculateScreenBounds()
    {
        Camera mainCamera = Camera.main;

        // ����� ������� ������
        Vector3 leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        minX = leftEdge.x + 0.2f;

        // ������ ������� ������
        Vector3 rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane));
        maxX = rightEdge.x - 0.2f;
    }
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
                deadManager.SpawnDeadFish(collision.gameObject.transform.position, scoreAdd);
            }
        }
    }
}
