using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] private GameObject baseObject;
    [SerializeField] private bool isTerra1 = false;

    public GameObject terra_1;
    public GameObject terra_2;
    public float MaxTime = 1.0f;
    public float height;

    private float _timer = 0;
    private GameObject _terra;
    //private int terra = 0;

    void Start()
    {
        if (isTerra1)
        {
            _terra = terra_1;
        }
        else
        {
            _terra = terra_2;
        }

        SpawnTube();
    }

    void Update()
    {
        if (_timer > MaxTime)
        {
            /*  Для случайного появления
            terra = Random.Range(1, 10);
            if (terra <= 5) { _terra = terra_2; }
            else if (terra > 5) { _terra = terra_1; }
        */
            SpawnTube();
            _timer = 0;
        }

        _timer += Time.deltaTime;
    }

    private void SpawnTube()
    {
        Vector3 position = transform.position + new Vector3(0, Random.Range(-height, height), 0);

        GameObject _newFish = Instantiate(_terra, position, Quaternion.identity);
        _newFish.transform.SetParent(baseObject.transform, false);
    }
}
