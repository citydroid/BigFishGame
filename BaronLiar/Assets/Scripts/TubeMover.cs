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
        // Уничтожаем объект, если он выходит за пределы экрана
        if (transform.position.x < -10) // Предположим, что -10 это предел экрана
        {
            Destroy(gameObject);
        }
    }
}
