using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movers
{
    public class Terra1Mover : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float targetX = -5f;

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
}