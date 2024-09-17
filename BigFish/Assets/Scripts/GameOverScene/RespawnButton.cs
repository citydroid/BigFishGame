using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnButton : MonoBehaviour
{

    void OnMouseDown()
    {
        //Вызов рекламы за вознаграждение Яндекс 
        Yandex.Instance.RevardAdv();
        //После рекламы вызывается Yandex.Instance.AfterAdvAction();
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
