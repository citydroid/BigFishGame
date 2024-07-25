using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeMover : MonoBehaviour
{
    public float _speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * _speed * Time.deltaTime;
    }
}
