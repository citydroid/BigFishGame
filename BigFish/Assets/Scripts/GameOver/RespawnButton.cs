using UnityEngine;

public class RespawnButton : MonoBehaviour
{
    protected GameManager gameManager;
    private void Awake()
    {
        GameObject gameManagerObject = GameObject.Find("GameManager");

        if (gameManagerObject != null)
            gameManager = gameManagerObject.GetComponent<GameManager>();
    }
    void OnMouseDown()
    {
        gameManager.ResumeGame();

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
