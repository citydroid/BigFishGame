using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowAdv();

    public GameObject gameOverCanvas;

    public void Start()
    {
        Time.timeScale = 1.0f;
    }
    public void GameOver()
    {
        SceneManager.LoadScene("2_GameOverScene");
        Time.timeScale = 0;
        /*
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        */
    }

    public void Replay()
    {
        SceneManager.LoadScene(1);
    }
    public void ShowAdvMethod()
    {
        ShowAdv();
    }
}
