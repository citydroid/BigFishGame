using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class FishSpawner : MonoBehaviour
{
    public float MaxTime = 1.0f;
    private float _timer = 0;
    private GameObject baseObject;
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
        GameObject newFish = Instantiate(_fish);
         newFish.transform.position = transform.position + new Vector3(0, Random.Range(-height, height), 0);
        */

         Destroy(_newFish, 10);
    }
}
