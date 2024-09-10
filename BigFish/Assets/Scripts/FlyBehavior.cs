using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.InputSystem;
public class FlyBehavior : MonoBehaviour
{
    [SerializeField] private float _velocity = 1f;

    private Rigidbody2D _rb;
    private Transform _tr;
    [SerializeField] public GameManager gameManager;
    public Animator playerAnimator;
    //  public GameObject playerObject;
    // Animator playerAction;

    public GameObject deadPrefab;
    public float destroyDelay = 1f;  // Время, через которое уничтожается объект 2

    private Vector2 moveVector;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tr = GetComponent<Transform>();
        playerAnimator = GetComponent<Animator>();
      //  playerAction = playerObject.GetComponent<Animator>();
    }


    void Update()
    { /*
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");
        _rb.MovePosition(_rb.position + moveVector * _velocity * Time.deltaTime);
        */
        Debug.Log("hjh " + _rb.velocity);
        if (Input.GetMouseButtonDown(0))
        {
            _rb.velocity = Vector2.up * _velocity;
            //Debug.Log("hjh " + _rb.velocity);
        }

        if (_tr.position.y <= -1.1)
        {
            _rb.velocity = Vector2.up;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.gameObject.CompareTag("FishAnimation"))
        {
            playerAnimator.Play("PlayerEating");
        }
        */


        playerAnimator.Play("PlayerEating");
        int scoreAdd = 0;
        int fishValue = 0;

        if (collision.gameObject.CompareTag("Fish_2"))   {
            fishValue = 2;
            scoreAdd = 1;
        }
        else if (collision.gameObject.CompareTag("Fish_10"))   {
            fishValue = 10;
            scoreAdd = 5;
        }
        else if (collision.gameObject.CompareTag("Fish_50"))   {
            fishValue = 50;
            scoreAdd = 10;
        }
        else if (collision.gameObject.CompareTag("Fish_150"))  {
            fishValue = 150;
            scoreAdd = 15;
        }
        else if (collision.gameObject.CompareTag("Fish_250"))  {
            fishValue = 250;
            scoreAdd = 22;
        }
        else if (collision.gameObject.CompareTag("Fish_375"))  {
            fishValue = 375;
            scoreAdd = 25;
        }
        else if (collision.gameObject.CompareTag("Fish_500"))  {
            fishValue = 500;
            scoreAdd = 30;
        }
        /*
        else if (collision.gameObject.CompareTag("Fish_750"))  {
            fishValue = 750;
            scoreAdd = 35;
        }
        else if (collision.gameObject.CompareTag("Fish_1000")) {
            fishValue = 1000;
            scoreAdd = 40;
        }
        else if (collision.gameObject.CompareTag("Fish_1500")) {
            fishValue = 1500;
            scoreAdd = 45;
        }
        else if (collision.gameObject.CompareTag("Fish_2250")) {
            fishValue = 2250;
            scoreAdd = 50;
        }
        else if (collision.gameObject.CompareTag("Fish_3000")) {
            fishValue = 3000;
            scoreAdd = 60;
        }
        else if (collision.gameObject.CompareTag("Fish_4000")) {
            fishValue = 4000;
            scoreAdd = 70;
        }       
        else if (collision.gameObject.CompareTag("Fish_5500")) {
             fishValue = 5500;
             scoreAdd = 80;
        }
        else if (collision.gameObject.CompareTag("Fish_7500")) {
             fishValue = 7500;
             scoreAdd = 90;
        }
        else if (collision.gameObject.CompareTag("Fish_10000")) {
             fishValue = 10000;
             scoreAdd = 100;
        }
        */

        if (Score.instance.GetScore() < fishValue) 
        {
            gameManager.GameOver();
        }
        else
        {
            Score.instance.UpdateScore(scoreAdd);
            Destroy(collision.gameObject);

            //playerAnimator.Play("PlayerFlash");

            Transform trans = collision.gameObject.GetComponent<Transform>();
            Vector3 spawnPosition = trans.position;
            GameObject newObject2 = Instantiate(deadPrefab, spawnPosition, Quaternion.identity);

            // Проигрываем анимацию (если требуется)
            /*Animator animator = newObject2.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("AnimationName"); // Замените "AnimationName" на название вашей анимации
            }*/

            // Уничтожаем Объект 2 через определённое время
            Destroy(newObject2, destroyDelay);
        }


    }


}
