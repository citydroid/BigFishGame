using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMover : MonoBehaviour
{
    public float minSpeed = 0.5f; 
    public float maxSpeed = 1.5f; 
    private float _speed;
    public float targetX = -5f;

    void Start()
    {
        _speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        transform.position += Vector3.left * _speed * Time.deltaTime;
        // ”ничтожаем объект, если он выходит за пределы экрана
        if (transform.position.x <= targetX) 
        {
            Destroy(gameObject);
        }
    }
}
