using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class FlyBehavior : MonoBehaviour
{
    [SerializeField] private float _velocity = 1f;

    private Rigidbody2D _rb;
    public GameManager gameManager;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _rb.velocity = Vector2.up * _velocity;
        }
        /*
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _rb.velocity = Vector2.up * _velocity;
        }
        */
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameManager.GameOver();
    }
}
