using Movers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private FishSpawner[] fishSpawner;
    [SerializeField] private int[] scoreThreshold;
    [SerializeField] private Transform backgroundTransform;
    [SerializeField] private TerraWaveSpawner groundSpawner1;
    [SerializeField] private TerraWaveSpawner groundSpawner2;
    private TerraWaveSpawner colorGround1;
    private TerraWaveSpawner colorGround2;

    private readonly bool[] isWhiteDo = new bool[30];
    private readonly bool[] hasIncreasedSpawn =  new bool[30];

    private bool _isSceneChanging = false;
    private int currentLevel = 1;
    private float maxPlayerHeight = -0.5f; //4f
    private readonly float startBackgroundPosition = -2.5f; //1f
    private float depthCoeff = 0.0002f; 
    //private readonly float[] backgroundYOffsets = new float[] { -2f, -4f, -3.9f, -3.8f, 3.7f, 1f, 2f };

    private readonly Dictionary<string, (int fishValue, int scoreAdd)> fishValues = new Dictionary<string, (int fishValue, int scoreAdd)>
    {
        { "Fish_2", (2, 1) },
        { "Fish_10", (10, 5) },
        { "Fish_50", (50, 10) },
        { "Fish_150", (150, 15) },
        { "Fish_250", (250, 22) },
        { "Fish_375", (375, 25) },
        { "Fish_500", (500, 30) }
    };

    // Список настроек уровней
    private readonly List<LevelSettings> levelSettings = new List<LevelSettings>
    {
        // Настройки для уровня 0
        new LevelSettings(0, new[] { (1, 1f, 1) }),
        // Настройки для уровня 1
        new LevelSettings(3, new[] { (1, 3f, 1)}),
        /*
new LevelSettings(4, new[] { (1, 1f, 1)}),
new LevelSettings(5, new[] { (1, 1f, 1)}),
new LevelSettings(6, new[] { (1, 1f, 1)}),
new LevelSettings(7, new[] { (1, 1f, 1)}),
new LevelSettings(8, new[] { (1, 1f, 1)}),
new LevelSettings(9, new[] { (1, 1f, 1)}),
new LevelSettings(10, new[] { (1, 1f, 1)}),
new LevelSettings(11, new[] { (1, 1f, 1)}),
new LevelSettings(12, new[] { (1, 1f, 1)})*/
        // Настройки для уровня 2

        new LevelSettings(10, new[] { (1, 1f, 1), (2, 1f, 2), (3, 1f,2) }),
        // Настройки для уровня 3
        new LevelSettings(50, new[] { (2, 1f, 1), (3, 1f, 1), (4, 1f, 1) }),
        // Настройки для уровня 4
        new LevelSettings(150, new[] { (1, 0.5f, 3), (2, 1f, 1), (3, 3f, 1), (4, 1f, 1), (5, 1f, 1), (6, 3f, 3)}),
        // Настройки для уровня 5
        new LevelSettings(250, new[] { (1, 1f, 1), (2, 0f, 0), (3, 1f, 1), (4, 1f, 1), (5, 1f, 1), (6, 3f, 3) }),
        // Настройки для уровня 6
        new LevelSettings(375, new[] { (3, 1f, 3), (4, 1f, 1), (5, 1f, 1), (7, 3f, 3) }),
        // Настройки для уровня 7
        new LevelSettings(500, new[] { (2, 2f, 1), (3, 1f, 3), (4, 1f, 1) }) 

    };
    public void Start()
    {
        Time.timeScale = 1.0f;
        transform.parent = null;
        // Подписываемся на событие загрузки новой сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
        backgroundTransform.position = new Vector3(backgroundTransform.position.x, startBackgroundPosition, backgroundTransform.position.z);
        colorGround1 = groundSpawner1.GetComponent<TerraWaveSpawner>();
        colorGround2 = groundSpawner2.GetComponent<TerraWaveSpawner>();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Сбрасываем флаг при загрузке новой сцены
        _isSceneChanging = false;
    }
    private void Update()
    {
        if (_isSceneChanging) return; 

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            int currentScore = Score.instance.GetScore();
            // Проверяем, если очки игрока достигли следующего fishValue для текущего уровня
            ProcessLevelProgress(currentScore);

            if (currentScore >= scoreThreshold[currentLevel])
            {
                // Скрываем объекты "Red" для всех рыб текущего уровня
                HideRedObjectsIfScoreMet(scoreThreshold[currentLevel]);
                // Переходим на следующий уровень
                currentLevel++;
                Progress.Instance.PlayerInfo.Level = currentLevel;
                // Включаем флаг "isWhite" в спаунере рыб текущего уровня
                if (currentLevel < fishSpawner.Length)
                {
                    fishSpawner[currentLevel].IsWhiteSwitch();
                }
            }
            UpdateBackgroundPosition();
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
    private void ProcessLevelProgress(int currentScore)
    {
        if (currentLevel >= levelSettings.Count) return;

        var settings = levelSettings[currentLevel];
//Debug.Log("0 " + currentScore + "  : " + settings.RequiredScore);
        if (currentScore >= settings.RequiredScore && !hasIncreasedSpawn[currentLevel])
        {
            ApplyLevelSettings(settings);
            //UpdateBackgroundPosition();
            hasIncreasedSpawn[currentLevel] = true;
        }
    }

    private void ApplyLevelSettings(LevelSettings settings)
    {
        Debug.Log("6 " + currentLevel + "  : " + scoreThreshold[6]);
        foreach (var (spawnerIndex, spawnRate, spawnAmount) in settings.SpawnActions)
        {
            fishSpawner[spawnerIndex].IncreaseSpawnRate(spawnRate, spawnAmount);
        }
    }
    public void GameOver()
    {
        Score.instance.UpdateGold();
        SceneManager.LoadScene("2_GameOverScene");
        Time.timeScale = 0;
    }

    public void Replay()
    {
        SceneManager.LoadScene(1);
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
    public float GetMaxPlayerHeight()
    {
        return maxPlayerHeight;
    }
    private void UpdateBackgroundPosition()
    {
            Vector3 newPosition = backgroundTransform.position;
            newPosition.y += depthCoeff;
            backgroundTransform.position = newPosition;
            maxPlayerHeight += depthCoeff;

        colorGround1.ChangeGrayLevel(depthCoeff/5);
        colorGround2.ChangeGrayLevel(depthCoeff / 5);
    }
}

// Структура для описания действий на уровне
// Объединенный класс для настроек уровня и спавнера
[System.Serializable]
public class LevelSettings
{
    public int RequiredScore; // Порог очков для перехода на уровень
    public List<(int spawnerIndex, float spawnRate, int spawnAmount)> SpawnActions; // Действия для увеличения спавна

    public LevelSettings(int requiredScore, params (int spawnerIndex, float spawnRate, int spawnAmount)[] actions)
    {
        RequiredScore = requiredScore;
        SpawnActions = new List<(int spawnerIndex, float spawnRate, int spawnAmount)>(actions);
    }
}
//Time.timeScale = 0;
//AudioListener.pause = false;