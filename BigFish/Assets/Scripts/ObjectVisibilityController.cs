using UnityEngine;
public class ObjectVisibilityController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
 //   private Collider2D objectCollider;
    private const float visibilityPadding = 0.8f; // Зазор в 10%
    private Canvas objectCanvas;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main; // Получаем главную камеру
 //       objectCollider = GetComponent<Collider2D>(); // Попробуем получить коллайдер, если он есть
        objectCanvas = GetComponentInChildren<Canvas>();
    }

    private void Update()
    {
        CheckVisibility();
    }

    private void CheckVisibility()
    {
        bool isVisible = IsObjectVisible();

        // Включаем или отключаем SpriteRenderer
        spriteRenderer.enabled = isVisible;

        // Включаем или отключаем Canvas, если он есть
        if (objectCanvas != null)
        {
            objectCanvas.enabled = isVisible;
        }
    }

    private bool IsObjectVisible()
    {
        Bounds objectBounds;

        // Используем коллайдер, если он есть, иначе используем размеры спрайта
        /*
        if (objectCollider != null)
        {
            objectBounds = objectCollider.bounds;
        }
        else
        {*/
            objectBounds = spriteRenderer.bounds;
      //  }
        
        // Получаем все четыре угла объекта с учетом зазора
        Vector3 minBounds = objectBounds.min - new Vector3(objectBounds.size.x * visibilityPadding, objectBounds.size.y * visibilityPadding, 0);
        Vector3 maxBounds = objectBounds.max + new Vector3(objectBounds.size.x * visibilityPadding, objectBounds.size.y * visibilityPadding, 0);

        // Переводим мировые координаты границ объекта в экранные координаты камеры
        Vector3 screenPointMin = mainCamera.WorldToViewportPoint(minBounds);
        Vector3 screenPointMax = mainCamera.WorldToViewportPoint(maxBounds);

        // Проверяем, попадает ли хотя бы часть объекта в камеру
        bool isVisible = (screenPointMin.x < 1 && screenPointMax.x > 0) &&
                         (screenPointMin.y < 1 && screenPointMax.y > 0);

        // Лог для отладки
        Debug.Log("Object is visible: " + isVisible + " | Bounds min: " + screenPointMin + " | Bounds max: " + screenPointMax);

        return isVisible;
    }
}