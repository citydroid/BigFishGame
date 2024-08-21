using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TubeSpawner : MonoBehaviour
{
    public float MaxTime = 1.0f;
    private float _timer = 0;
    public GameObject _tube;
    public float height;

    void Start()
    {
        SpawnTube();
    }

    void Update()
    {
        if (_timer > MaxTime)
        {
            GameObject _newTube = Instantiate(_tube);
            _newTube.transform.position = transform.position + new Vector3(0, Random.Range(-height, height), 0);
            Destroy(_newTube,10);
            _timer = 0;
        }

        _timer += Time.deltaTime;
    }

    private void SpawnTube()
    {
        GameObject newTube = Instantiate(_tube);
        newTube.transform.position = transform.position + new Vector3(0, Random.Range(-height, height), 0);
    }
}
