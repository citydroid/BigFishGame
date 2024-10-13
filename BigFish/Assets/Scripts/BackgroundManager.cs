using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private Transform backgroundTransform;
    private TerraWaveSpawner groundSpawner1;
    private TerraWaveSpawner groundSpawner2;
    private TerraWaveSpawner colorGround1;
    private TerraWaveSpawner colorGround2;
    private float depthCoeff;
    private float maxPlayerHeight;

    public void Initialize(Transform background, TerraWaveSpawner spawner1, TerraWaveSpawner spawner2, float initialDepthCoeff, float initialMaxHeight)
    {
        backgroundTransform = background;
        groundSpawner1 = spawner1;
        groundSpawner2 = spawner2;
        depthCoeff = initialDepthCoeff;
        maxPlayerHeight = initialMaxHeight;
    }

    public void UpdateBackgroundPosition()
    {
        Vector3 newPosition = backgroundTransform.position;
        newPosition.y += depthCoeff;
        backgroundTransform.position = newPosition;
        maxPlayerHeight += depthCoeff;

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
