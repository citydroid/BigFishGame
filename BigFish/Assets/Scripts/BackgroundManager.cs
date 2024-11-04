using UnityEngine;
using System.Collections;
public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Transform backgroundTransform;
    [SerializeField] private TerraWaveSpawner groundSpawner1;
    [SerializeField] private TerraWaveSpawner groundSpawner2;
    private TerraWaveSpawner colorGround1;
    private TerraWaveSpawner colorGround2;
    private float depthCoeff;

    [SerializeField] private Transform cameraTransform;
    private float maxPlayerHeight = -0.8f;

    public void Initialize(float initialDepthCoeff)
    {
        depthCoeff = initialDepthCoeff;
    }

    private float SetSpeedDependsCamera()
    {
        // Получаем ортографический размер камеры (половина высоты экрана в мировых координатах)
        float cameraSize = Camera.main.orthographicSize;

        // Соотношение сторон экрана (ширина / высота)
        float aspectRatio = (float)Screen.width / Screen.height;

        // Рассчитываем видимую ширину экрана в мировых координатах
        float worldScreenWidth = cameraSize * 2 * aspectRatio;

        // Нормализуем скорость перемещения фона на основе ширины экрана
        float normalizedSpeed = depthCoeff * worldScreenWidth / 10f; // 10f — базовая ширина, под которую адаптируемся

        return normalizedSpeed;
    }
    /*
    private float SetSpeedDependsScreen()
    {
        // Получаем текущее разрешение экрана
        float screenHeight = Screen.height;
        //float screenWidth = Screen.width;
        //float aspectRatio = screenWidth / screenHeight;// Соотношение сторон экрана
        float normalizedSpeed = depthCoeff * screenWidth / 1920f; // 1920 - базовое разрешение (например, для Full HD)

        return normalizedSpeed;
    }
    */

    public void MoveBackground(float distance)
    {

        StartCoroutine(MoveBackgroundRoutine(distance));
    }

    private IEnumerator MoveBackgroundRoutine(float distance)
    {
        float targetY = backgroundTransform.position.y + distance;

        while (Mathf.Abs(backgroundTransform.position.y - targetY) > 0.01f)
        {
            Vector3 newPosition = backgroundTransform.position;
            newPosition.y += depthCoeff * Time.deltaTime; // Используем инкремент для движения к цели
            backgroundTransform.position = newPosition;

            maxPlayerHeight += depthCoeff * Time.deltaTime; // Обновляем высоту игрока, если необходимо
           // UpdateTerraColor();

            yield return null;
        }
    }
    private void UpdateTerraColor()
    {
        groundSpawner1.ChangeGrayLevel(depthCoeff / 5);
        groundSpawner2.ChangeGrayLevel(depthCoeff / 5);
    }
    public float GetMaxPlayerHeight()
    {
        return maxPlayerHeight;
    }
    public void SetDepthCoefficient(float newDepthCoeff)
    {
        depthCoeff = newDepthCoeff;
    }
}
