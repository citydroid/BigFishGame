using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bubbleObject;
    [SerializeField] private float spawnInterval = 0.1f; // Интервал появления пузырьков
    [SerializeField] private float bubbleLifetime = 2f; // Время жизни пузырька


    private Vector3 lastPlayerPosition;
    private bool isSpawning = false;
    float randCoef;


    private void Start()
    {
        StartCoroutine(SpawnBubbles());
    }

    private IEnumerator SpawnBubbles()
    {
        while (true)
        {
            if (isSpawning)
            {
                SpawnBubbleAt(lastPlayerPosition);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnBubbleAt(Vector3 position)
    {
        if (bubbleObject == null) return;

        Vector3 spawnPosition = new Vector3(position.x, position.y, position.z);
        GameObject bubbleInstance = Instantiate(bubbleObject, spawnPosition, Quaternion.identity, transform);

        Transform scaleTransform = bubbleInstance.transform;
        if (scaleTransform != null)
        {
            float randCoef = Random.Range(0.6f, 1f);
            Vector3 currentScale = scaleTransform.localScale;
            scaleTransform.localScale = new Vector3(currentScale.x * randCoef, currentScale.y * randCoef, currentScale.z);
        }
        Destroy(bubbleInstance, bubbleLifetime);
    }

    public void UpdatePlayerPosition(Vector3 playerPosition)
    {
        lastPlayerPosition = playerPosition;
        isSpawning = true;
    }

    public void StopBubbles()
    {
        isSpawning = false;
    }
}
