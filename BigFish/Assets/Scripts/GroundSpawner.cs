using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public float MaxTime = 1.0f;
    private float _timer = 0;
    private GameObject baseObject;
    private GameObject _fish;
    public float height;
    private int terra = 0;
    public GameObject terra_1;
    public GameObject terra_2;
    void Start()
    {
        _fish = terra_2;
        baseObject = GameObject.Find("Base");
        SpawnTube();
    }

    void Update()
    {
        if (_timer > MaxTime)
        {
            terra = Random.Range(1, 10);
            if (terra <= 5) { _fish = terra_2; }
            else if (terra > 5) { _fish = terra_1; }

            SpawnTube();
            _timer = 0;
        }

        _timer += Time.deltaTime;
    }

    private void SpawnTube()
    {
        Vector3 position = transform.position + new Vector3(0, Random.Range(-height, height), 0);

        GameObject _newFish = Instantiate(_fish, position, Quaternion.identity);
        _newFish.transform.SetParent(baseObject.transform, false);
        /*
        GameObject newFish = Instantiate(_fish);
        newFish.transform.position = transform.position + new Vector3(0, Random.Range(-height, height), 0);
        */

        Destroy(_newFish, 10);
    }
}
