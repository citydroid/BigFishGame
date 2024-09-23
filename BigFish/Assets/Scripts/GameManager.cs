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



    private readonly Dictionary<string, (int fishValue, int scoreAdd)> fishValues = new Dictionary<string, (int fishValue, int scoreAdd)>
    {
        { "Fish_2", (2, 1) },
        { "Fish_10", (10, 5) },
        { "Fish_50", (50, 10) },
        { "Fish_150", (150, 15) },
        { "Fish_250", (250, 22) },
        { "Fish_375", (375, 25) },
        { "Fish_500", (500, 30) }
        // Можно добавить другие рыбы по аналогии
    };


    // Fish Value и ScoreAdd для каждого уровня
    private Dictionary<int, int> fishValueForLevel = new Dictionary<int, int>
    {
        { 1, 2 },
        { 2, 10 },
        { 3, 50 }
    };

    private Dictionary<int, int> scoreAddForLevel = new Dictionary<int, int>
    {
        { 1, 1 },
        { 2, 5 },
        { 3, 10 }
    };
    private int currentScore;
    private int currentLevel = 1;

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

        currentScore = Score.instance.GetScore();

        IsWhiteChanger(currentScore);

        // Проверяем, если очки игрока достигли следующего fishValue для текущего уровня
        if (currentScore >= scoreThreshold[currentLevel])
        {
            // Скрываем объекты "Red" для всех рыб текущего уровня
            HideRedObjectsIfScoreMet(scoreThreshold[currentLevel]);
            // Переходим на следующий уровень
            NextLevel();
        }


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
    private void HideRedObjectsIfScoreMet(int fishValue)
    {
        // Логика для скрытия объектов "Red"
        GameObject[] allFishObjects = GameObject.FindGameObjectsWithTag($"Fish_{fishValue}");

        foreach (GameObject fish in allFishObjects)
        {
            if (Score.instance.GetScore() >= fishValue)
            {
                Transform redTransform = fish.transform.Find("Red");
                if (redTransform != null)
                {
                    GameObject redObject = redTransform.gameObject;
                    redObject.SetActive(false);
                }
            }
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
        currentLevel++;
        Progress.Instance.PlayerInfo.Level = currentLevel;
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

    public bool TryGetFishValue(string fishTag, out int fishValue, out int scoreAdd)
    {
        if (fishValues.TryGetValue(fishTag, out var values))
        {
            fishValue = values.fishValue;
            scoreAdd = values.scoreAdd;
            return true;
        }

        fishValue = 0;
        scoreAdd = 0;
        return false;
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