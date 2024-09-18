using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FishSpawner fishSpawner_2;
    [SerializeField] private FishSpawner fishSpawner_10;
    [SerializeField] private FishSpawner fishSpawner_50;
 /*
    [SerializeField] private FishSpawner fishSpawner_2;
    [SerializeField] private FishSpawner fishSpawner_2;
    [SerializeField] private FishSpawner fishSpawner_2;
    [SerializeField] private FishSpawner fishSpawner_2;
    [SerializeField] private FishSpawner fishSpawner_2;
 */
    [SerializeField] private int _level = 1; 
    [SerializeField] private int scoreThreshold_1 = 10;
    [SerializeField] private int scoreThreshold_2 = 20;
    [SerializeField] private int scoreThreshold_3 = 40;

    private bool[] hasIncreasedSpawn =  new bool[] { false, false, false, false, false };
    public void Start()
    {
        Time.timeScale = 1.0f;
        transform.parent = null;
    }
    private void Update()
    {
        int currentScore = Score.instance.GetScore();

        if (currentScore >= scoreThreshold_1 && !hasIncreasedSpawn[1])
        {
            Level_1();
        }
        if (currentScore >= scoreThreshold_2 && !hasIncreasedSpawn[2])
        {
            Level_2();
        }
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

    private void Level_1()
    {
        // Увеличиваем количество спавнящихся объектов
        fishSpawner_2.IncreaseSpawnRate(0.5f, 3);  // Уменьшаем время спавна и увеличиваем количество объектов
        hasIncreasedSpawn[1] = true;  // Ставим флаг, чтобы увеличить спавн только один раз
    }
    private void Level_2()
    {
        // Увеличиваем количество спавнящихся объектов
        fishSpawner_10.IncreaseSpawnRate(0.5f, 3);  // Уменьшаем время спавна и увеличиваем количество объектов
        hasIncreasedSpawn[2] = true;  // Ставим флаг, чтобы увеличить спавн только один раз
    }
    private void Level_3()
    {
        // Увеличиваем количество спавнящихся объектов
        fishSpawner_50.IncreaseSpawnRate(0.5f, 3);  // Уменьшаем время спавна и увеличиваем количество объектов
        hasIncreasedSpawn[3] = true;  // Ставим флаг, чтобы увеличить спавн только один раз
    }
}


//Time.timeScale = 0;
//AudioListener.pause = false;