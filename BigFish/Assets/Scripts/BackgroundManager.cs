using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Transform backgroundTransform;
    [SerializeField] private TerraWaveSpawner groundSpawner1;
    [SerializeField] private TerraWaveSpawner groundSpawner2;
    private TerraWaveSpawner colorGround1;
    private TerraWaveSpawner colorGround2;
    private float depthCoeff;
    private float maxPlayerHeight = -0.5f;

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
