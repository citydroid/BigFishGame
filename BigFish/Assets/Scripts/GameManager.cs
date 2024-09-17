using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
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
}


//Time.timeScale = 0;
//AudioListener.pause = false;