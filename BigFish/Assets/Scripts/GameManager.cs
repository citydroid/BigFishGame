using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowAdv();

    [DllImport("__Internal")]

    private static extern void RewardedAdv();
    public GameObject gameOverCanvas;


    [SerializeField] private int _level = 1;

    public void Start()
    {
        Time.timeScale = 1.0f;
        transform.parent = null;
    }
    public void GameOver()
    {
        Score.instance.UpdateGold();
        SceneManager.LoadScene("2_GameOverScene");
        Time.timeScale = 0;
        /*
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        */
    }

    public void NextLevel()
    {
        _level++;
        Progress.Instance.PlayerInfo.Level = _level;
    }



    public void Replay()
    {
        SceneManager.LoadScene(1);
    }



    //Для показа банера
    public void ShowAdvMethod()
    {
        ShowAdv();
    }

    //Запуск после вознаграждения
    public void AfterAdvAction()
    {
        SceneManager.LoadScene(1);
    }
}


//Time.timeScale = 0;
//AudioListener.pause = false;