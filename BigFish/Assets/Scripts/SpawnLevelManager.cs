using UnityEngine;

public class SpawnLevelManager
{
    private float maxHeight;

    public SpawnLevelManager(float maxHeight)
    {
        this.maxHeight = maxHeight;
    }
    public void SetMaxHeight(float _maxHeight)
    {
        maxHeight = _maxHeight + 0.8f;
    }
    public (float heightMin, float heightMax) GetHeightRange(int level)
    {
        switch (level)
        {
            case 1:
                return (0f, 0.0001f);
            case 2:
                return (0f, Mathf.Min(2f, maxHeight));
            case 3:
                return (0.5f, Mathf.Min(2f, maxHeight));
            case 4:
                return (1f, maxHeight);
            case 5:
                return (0f, maxHeight);
            case 6:
                return (Mathf.Max(0f, maxHeight - 0.1f), maxHeight);
            default:
                return (0f, 0.1f); // Уровень по умолчанию
        }
    }
}
