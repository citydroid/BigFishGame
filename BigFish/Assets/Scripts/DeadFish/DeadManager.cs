using UnityEngine;

public class DeadManager : MonoBehaviour
{
    [SerializeField] private GameObject deadPrefab;
    [SerializeField] private float destroyDelay = 2.0f;

    public void SpawnDeadFish(Vector3 spawnPosition, int scoreAdd)
    {
        GameObject deadFish = Instantiate(deadPrefab, spawnPosition, Quaternion.identity);
        UpdateDeadText(deadFish, scoreAdd);
        Destroy(deadFish, destroyDelay);
    }

    private void UpdateDeadText(GameObject deadObject, int scoreAdd)
    {
        DeadScore deadScore = deadObject.GetComponent<DeadScore>();

        if (deadScore != null)
        {
            deadScore.ScoreValue(scoreAdd);
        }
    }
}