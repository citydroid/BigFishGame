using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.InputSystem;
public class FlyBehavior : MonoBehaviour
{
    [SerializeField] private float _velocity = 1f;

    private Rigidbody2D _rb;
    [SerializeField] public GameManager gameManager;
    public Animator playerAnimator;
  //  public GameObject playerObject;
  //  Animator playerAction;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
      //  playerAction = playerObject.GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _rb.velocity = Vector2.up * _velocity;
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

        if (collision.gameObject.CompareTag("Fish_2")) {
            fishValue = 2;
            scoreAdd = 1;
        } else if (collision.gameObject.CompareTag("Fish_10")) {
            fishValue = 10;
            scoreAdd = 5;
        } else if (collision.gameObject.CompareTag("Fish_50")) {
            fishValue = 50;
            scoreAdd = 10;
        } else if (collision.gameObject.CompareTag("Fish_150")) {
            fishValue = 150;
            scoreAdd = 15;
        } else if (collision.gameObject.CompareTag("Fish_250")) {
            fishValue = 250;
            scoreAdd = 22;
        } else if (collision.gameObject.CompareTag("Fish_375")) {
            fishValue = 375;
            scoreAdd = 25;     
         } else if (collision.gameObject.CompareTag("Fish_500")) {
                    fishValue = 500;
                    scoreAdd = 30;
         } /*   else if (collision.gameObject.CompareTag("Fish_750")) {
                    fishValue = 750;
                    scoreAdd = 35;
            } else if (collision.gameObject.CompareTag("Fish_1000")) {
                    fishValue = 1000;
                    scoreAdd = 40;
            } else if (collision.gameObject.CompareTag("Fish_1500")) {
                    fishValue = 1500;
                    scoreAdd = 45;
            } else if (collision.gameObject.CompareTag("Fish_2250")) {
                    fishValue = 2250;
                    scoreAdd = 60;


           } else if (collision.gameObject.CompareTag("Fish_3000")) {
                    fishValue = 3000;
                    scoreAdd = 60;
           } else if (collision.gameObject.CompareTag("Fish_4000")) {
                    fishValue = 4000;
                    scoreAdd = ;
           } else if (collision.gameObject.CompareTag("Fish_5500")) {
                    fishValue = 5500;
                    scoreAdd = ;
           } else if (collision.gameObject.CompareTag("Fish_7500")) {
                    fishValue = 2250;
                    scoreAdd = ;
           } else if (collision.gameObject.CompareTag("Fish_10000")) {
                    fishValue = 2250;
                    scoreAdd = ;
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
        }
    }
}
