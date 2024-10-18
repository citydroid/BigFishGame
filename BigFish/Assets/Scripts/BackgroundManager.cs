using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Transform backgroundTransform;
    [SerializeField] private TerraWaveSpawner groundSpawner1;
    [SerializeField] private TerraWaveSpawner groundSpawner2;
    private TerraWaveSpawner colorGround1;
    private TerraWaveSpawner colorGround2;
    private float depthCoeff;

    [SerializeField] private Transform cameraTransform;  // Ссылка на камеру
    private float maxPlayerHeight = -0.8f;

    public void Initialize(float initialDepthCoeff)
    {
        depthCoeff = initialDepthCoeff;
    }

    public void UpdateBackgroundPosition()
    {
        Vector3 newPosition = backgroundTransform.position;
        newPosition.y += depthCoeff;
        backgroundTransform.position = newPosition;

        maxPlayerHeight += depthCoeff;
        //Debug.Log("newPosition.y " + newPosition.y + " maxPlayerHeight " + maxPlayerHeight);
        UpdateTerraColor();
    }
        private void UpdateTerraColor() 
    {
        groundSpawner1.ChangeGrayLevel(depthCoeff / 5);
        groundSpawner2.ChangeGrayLevel(depthCoeff / 5);
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
    public float GetMaxPlayerHeight()
    {
        return maxPlayerHeight;
    }
    public void SetDepthCoefficient(float newDepthCoeff)
    {
        depthCoeff = newDepthCoeff;
    }
}
