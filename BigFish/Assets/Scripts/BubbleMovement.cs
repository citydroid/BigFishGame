using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
    private float _speed = 0.2f;
    protected GameManager gameManager;
    protected float _maxVertical;

    void Start()
    {
        GameObject gameManagerObject = GameObject.Find("GameManager");

        if (gameManagerObject != null)
            gameManager = gameManagerObject.GetComponent<GameManager>();

    }
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    void Update()
    {
        // �������� ������������ ������������ ������� (������ �����)
        if (gameManager != null)
            _maxVertical = gameManager.GetMaxPlayerHeight();

        // ���� ������ ���� ������ �����, ������� ��� �����
        if (transform.position.y < _maxVertical)
        {
            transform.position += Vector3.up * _speed * Time.deltaTime;
        }
        else
        {
            // ������������� �������� ��� ��������� ��������� ��� ���������� ������ �����
            transform.position = new Vector3(transform.position.x, _maxVertical, transform.position.z);
        }
    }
}
