using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartNewGame : MonoBehaviour
{
    void OnMouseDown()
    {
        // Действие при клике на спрайт
        Debug.Log("Sprite button clicked!");

        // Вы можете вызвать любой метод, аналогичный кнопке в UI
        PerformAction();
    }

    void PerformAction()
    {
        SceneManager.LoadScene(0);
    }

    void OnMouseEnter()
    {
        // Изменение цвета спрайта при наведении курсора
        GetComponent<SpriteRenderer>().color = Color.gray;
    }

    void OnMouseExit()
    {
        // Возвращение к исходному цвету
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
