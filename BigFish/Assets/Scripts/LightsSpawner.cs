using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LightsSpawner : MonoBehaviour
{
    public float MaxTime = 1.0f;
    private float _timer = 0;
    GameObject baseObject;
    public GameObject _fish;
    public float height;

    void Start()
    {
        baseObject = GameObject.Find("Base");
        SpawnTube();
    }

    void Update()
    {
        if (_timer > MaxTime)
        {
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
        GameObject newTube = Instantiate(_tube);
        newTube.transform.position = transform.position + new Vector3(0, Random.Range(-height, height), 0);
        */
        Destroy(_newFish, 10);
    }
}
