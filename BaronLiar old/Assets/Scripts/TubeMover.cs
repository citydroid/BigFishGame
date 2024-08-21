using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeMover : MonoBehaviour
{
    public float minSpeed = 0.5f; 
    public float maxSpeed = 1.5f; 
    private float _speed;

    void Start()
    {
        _speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        transform.position += Vector3.left * _speed * Time.deltaTime;
    }
}
