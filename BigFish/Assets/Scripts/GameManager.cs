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
    [SerializeField] private int[] scoreThreshold = new int[]
    {
        0,
        10,
        50,
        100
    };

    private readonly bool[] isWhiteDo = new bool[] { false, false, false, false };
    private readonly bool[] hasIncreasedSpawn =  new bool[] { false, false, false, false };

    private bool _isSceneChanging = false;
    public void Start()
    {
        Time.timeScale = 1.0f;
        transform.parent = null;
        // Подписываемся на событие загрузки новой сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Сбрасываем флаг при загрузке новой сцены
        _isSceneChanging = false;
    }
    private void Update()
    {
        if (_isSceneChanging)
        {
            return;  // Останавливаем выполнение, если сцена меняется
        }

        int currentScore = Score.instance.GetScore();

        IsWhiteChanger(currentScore);

        if (currentScore >= scoreThreshold[1] && !hasIncreasedSpawn[1])
        {
            Level_1();
        }
        if (currentScore >= scoreThreshold[2] && !hasIncreasedSpawn[2])
        {
            Level_2();
        }
        if (currentScore >= scoreThreshold[3] && !hasIncreasedSpawn[3])
        {
            Level_3();
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

    private void IsWhiteChanger(int _score)
    {
        if (_score >= 10 && !isWhiteDo[2]) {
            fishSpawner_10.IsWhiteSwitch();
            isWhiteDo[2] = true;
        }
        else if (_score >= 50 && !isWhiteDo[3]) {
            fishSpawner_50.IsWhiteSwitch();
            isWhiteDo[3] = true;
        }
    }

    private void Level_1()
    {
        // Увеличиваем количество спавнящихся объектов
        fishSpawner_2.IncreaseSpawnRate(0.5f, 3);  // Уменьшаем время спавна и увеличиваем количество объектов
        hasIncreasedSpawn[1] = true;  // Ставим флаг, чтобы увеличить спавн только один раз
    }
    private void Level_2()
    {
        fishSpawner_2.IncreaseSpawnRate(3f, 1);
        // Увеличиваем количество спавнящихся объектов
        fishSpawner_10.IncreaseSpawnRate(1f, 3);  // Уменьшаем время спавна и увеличиваем количество объектов
        hasIncreasedSpawn[2] = true;  // Ставим флаг, чтобы увеличить спавн только один раз
    }
    private void Level_3()
    {
        fishSpawner_10.IncreaseSpawnRate(2f, 2);
        // Увеличиваем количество спавнящихся объектов
        fishSpawner_50.IncreaseSpawnRate(0.5f, 3);  // Уменьшаем время спавна и увеличиваем количество объектов
        hasIncreasedSpawn[3] = true;  // Ставим флаг, чтобы увеличить спавн только один раз
    }
}


//Time.timeScale = 0;
//AudioListener.pause = false;