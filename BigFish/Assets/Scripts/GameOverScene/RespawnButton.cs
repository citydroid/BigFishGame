using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnButton : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowAdv();
    void OnMouseDown()
    {
        // Действие при клике на спрайт
        Debug.Log("Sprite button clicked!");

        // Вы можете вызвать любой метод, аналогичный кнопке в UI
        PerformAction();
    }

    void PerformAction()
    {
        // ShowAdv();
        SceneManager.LoadScene(1);
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

    void AdYandex(int value)
    {
        // Возвращение к исходному цвету
        GetComponent<SpriteRenderer>().color = Color.black;
    }
}
