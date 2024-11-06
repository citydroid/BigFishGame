using UnityEngine;
public class ObjectVisibilityController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private const float visibilityPadding = 0.8f; 
    private Canvas objectCanvas;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        objectCanvas = GetComponentInChildren<Canvas>();
    }

    private void Update()
    {
        CheckVisibility();
    }

    private void CheckVisibility()
    {
        bool isVisible = IsObjectVisible();

        spriteRenderer.enabled = isVisible;

        if (objectCanvas != null)
        {
            objectCanvas.enabled = isVisible;
        }
    }

    private bool IsObjectVisible()
    {
        Bounds objectBounds;
        objectBounds = spriteRenderer.bounds;

        Vector3 minBounds = objectBounds.min - new Vector3(objectBounds.size.x * visibilityPadding, objectBounds.size.y * visibilityPadding, 0);
        Vector3 maxBounds = objectBounds.max + new Vector3(objectBounds.size.x * visibilityPadding, objectBounds.size.y * visibilityPadding, 0);

        Vector3 screenPointMin = mainCamera.WorldToViewportPoint(minBounds);
        Vector3 screenPointMax = mainCamera.WorldToViewportPoint(maxBounds);

        bool isVisible = (screenPointMin.x < 1 && screenPointMax.x > 0) &&
                         (screenPointMin.y < 1 && screenPointMax.y > 0);

        return isVisible;
    }
}