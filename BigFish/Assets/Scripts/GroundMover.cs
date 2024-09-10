using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMover : MonoBehaviour
{
    public float _speed;

    void Start()
    {

    }

    void Update()
    {
        transform.position += Vector3.left * _speed * Time.deltaTime;
    }
}
