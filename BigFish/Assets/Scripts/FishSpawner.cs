using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private float maxTime;
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private float height;
    [SerializeField] private int spawnCount = 0;
    [SerializeField] private bool isWhite = false;

    private float _timer = 0f;
    private GameObject _baseObject;

    void Start()
    {
        _baseObject = GameObject.Find("Base");
        SpawnFish();
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > maxTime)
        {
            SpawnFish();
            _timer = 0;
        }
    }

    private void SpawnFish()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 position = transform.position + new Vector3(0, Random.Range(0, height), 0);

            GameObject newFish = Instantiate(fishPrefab, position, Quaternion.identity);
            newFish.transform.SetParent(_baseObject.transform, false);

            if (isWhite)
            {
                Transform textTransform = newFish.transform.Find("Red");
                GameObject valueObject = textTransform.gameObject;
                if (valueObject != null)
                {
                    valueObject.SetActive(false);
                }
            }
        }
    }
    public void IsWhiteSwitch()
    {
        isWhite = true;
    }
    public void IncreaseSpawnRate(float newMaxTime, int newSpawnCount)
    {
        maxTime = newMaxTime;
        spawnCount = newSpawnCount;
    }
    public void IncreaseSpawnHight(float newHight)
    {
        height = newHight;
    }
}
