using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private float maxTime = 1.0f;
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private float height;

    private float _timer = 0f;
    private GameObject _baseObject;
    void Start()
    {
        _baseObject = GameObject.Find("Base");
        SpawnTube();
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > maxTime)
        {
            SpawnTube();
            _timer = 0;
        }
    }

    private void SpawnTube()
    {
        Vector3 position = transform.position + new Vector3(0, Random.Range(-height, height), 0);

        GameObject newFish = Instantiate(fishPrefab, position, Quaternion.identity);
        newFish.transform.SetParent(_baseObject.transform, false);
    }
}
