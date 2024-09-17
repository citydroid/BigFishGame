using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressSceneButton : MonoBehaviour
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
        ///TO DO Обнуляет все очки. Перенести!!
        Score.instance.Obnulit();
        /////////////////////////////////////

        //Progress.Instance.SavePlayerInfo();

        SceneManager.LoadScene(3);
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
