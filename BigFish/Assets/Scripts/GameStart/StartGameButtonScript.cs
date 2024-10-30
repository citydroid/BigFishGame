using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButtonScript : MonoBehaviour
{
    void OnMouseDown()
    {
        PerformAction();
    }

    void PerformAction()
    {
        SceneManager.LoadScene(1);
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
