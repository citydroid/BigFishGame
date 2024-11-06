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

        //����� ������� �� �������������� ������ 
        Yandex.Instance.RevardAdv();
        //����� ������� ���������� Yandex.Instance.AfterAdvAction();
    }
    void OnMouseEnter()
    {
        // ��������� ����� ������� ��� ��������� �������
        GetComponent<SpriteRenderer>().color = Color.gray;
    }

    void OnMouseExit()
    {
        // ����������� � ��������� �����
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
