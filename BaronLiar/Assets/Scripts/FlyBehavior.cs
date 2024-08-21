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
        /*
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _rb.velocity = Vector2.up * _velocity;
        }
        */
    }
/*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameManager.GameOver();
    }
*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
           
            Debug.Log("Collectible");
            // Добавляем очко
            Score.instance.UpdateScore();

            // Уничтожаем объект
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("FishAnimation"))
        {
            playerAnimator.Play("PlayerEating");
        }
        else
        {
            // Если столкновение с другими объектами
            gameManager.GameOver();
        }
    }
}
