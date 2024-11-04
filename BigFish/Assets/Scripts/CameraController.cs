using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float upperScreenThreshold = 0.75f;
    [SerializeField] private float lowerScreenThreshold = 0.25f;
    [SerializeField] private float cameraMoveSpeed = 2f;
    [SerializeField] private float smoothTime = 1f;
    [SerializeField] private float predictionFactor = 10f;

    private Vector3 velocity = Vector3.zero;
    private float cameraMinY;
    private Transform _cameraTransform;
    private float minX, maxX;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        cameraMinY = _cameraTransform.position.y;

        CalculateScreenBounds();
    }

    public void UpdateCameraPosition(Vector3 playerPosition)
    {
        Vector3 playerScreenPos = Camera.main.WorldToViewportPoint(playerPosition);
        Vector3 targetPosition = _cameraTransform.position;

        if (playerScreenPos.y >= upperScreenThreshold)
        {
            float predictedYPosition = playerPosition.y + (cameraMoveSpeed * predictionFactor);
            targetPosition = new Vector3(_cameraTransform.position.x, predictedYPosition, _cameraTransform.position.z);
        }
        else if (playerScreenPos.y <= lowerScreenThreshold && _cameraTransform.position.y > cameraMinY)
        {
            float predictedYPosition = playerPosition.y - (cameraMoveSpeed * predictionFactor);
            targetPosition = new Vector3(_cameraTransform.position.x, Mathf.Max(predictedYPosition, cameraMinY), _cameraTransform.position.z);
        }

        _cameraTransform.position = Vector3.SmoothDamp(_cameraTransform.position, targetPosition, ref velocity, smoothTime);
    }
    private void CalculateScreenBounds()
    {
        Camera mainCamera = Camera.main;

        Vector3 leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, mainCamera.nearClipPlane));
        Vector3 rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, mainCamera.nearClipPlane));

        float offset = Mathf.Abs(rightEdge.x - leftEdge.x) * 0.01f;
        minX = leftEdge.x + offset;
        maxX = rightEdge.x - offset;
    }
    public float MinX => minX;
    public float MaxX => maxX;
}
