using Movers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private FishSpawner[] fishSpawner;

    [SerializeField] private GameObject gameOverPrefab;
    private GameObject gameOverInstance;
    private bool gameOver = false;
    public AudioSource gameMusic;

    [SerializeField] private BackgroundManager backgroundManager;

    private bool _isSceneChanging = false;
    private int currentFishLevel = 1;

    private int currentLevelIndex = 0;
    private float maxPlayerHeight; 
    private float depthCoeff = 0.02f;

    private readonly Dictionary<string, (int fishValue, int scoreAdd)> fishValues = new Dictionary<string, (int fishValue, int scoreAdd)>
    {
        { "Fish_1", (2, 1) },
        { "Fish_2", (18, 3) },
        { "Fish_3", (50, 5) },
        { "Fish_4", (120, 8) },
        { "Fish_5", (200, 15) },
        { "Fish_6", (400, 25) },
        { "Fish_7", (1000, 30) },
        { "Fish_8", (2000, 35) },
        { "Fish_9", (5050, 40) }
    };
    private List<KeyValuePair<string, (int fishValue, int scoreAdd)>> fishValuesList;

    // Список настроек уровней и флаг для контроля их использования
    private readonly bool[] hasIncreasedSpawn = new bool[30];
    private int currentLevel = 0;
    private readonly List<LevelSettings> levelSettings = new List<LevelSettings>
    {
        // Настройки для уровня 0 (# рыбы, пауза между спаунами, количество рыбы за спаун, мин.высота, макс.высота)
        new LevelSettings(0, 0.1f, new[] { (1, 1f, 1, 2), (2, 3f, 1, 1) }),
        new LevelSettings(3, 0.1f, new[] { (1, 0.5f, 1, 2)}),
        new LevelSettings(10, 0.1f, new[] { (1, 1f, 1, 3), (3, 5f, 2, 3) }),
        new LevelSettings(18, -1f, new[] { (2, 10f, 1, 1), (3, 0.5f, 1, 3), (4, 2f, 1, 3) }),
        new LevelSettings(50, -1f, new[] { (1, 3f, 2, 5), (2, 3f, 1, 1), (3, 3f, 1, 3), (4, 5f, 1,3), (5, 1f, 2, 5) }),
        new LevelSettings(120, -1f, new[] { (1, 0.5f, 3, 5), (2, 1f, 1, 1), (3, 3f, 1, 4), (4, 1f, 1, 4), (5, 0f, 0, 0) }),
        new LevelSettings(200, 1f, new[] { (1, 5f, 10, 6), (2, 0f, 0, 1), (3, 1f, 1,4), (4, 1f, 1, 4), (5, 1f, 1, 4), (6, 0f, 0, 0), (7, 1f, 1, 1) }),
        new LevelSettings(500, 1f, new[] { (3, 1f, 3, 3), (4, 1f, 1, 4), (5, 1f, 1, 2), (8, 3f, 3, 5) }),
        new LevelSettings(1000, 1f, new[] { (2, 2f, 1, 1), (3, 1f, 1, 4), (4, 1f, 1, 5), (8, 3f, 3, 4) })

    };
    private void Awake()
    {  
        fishValuesList = new List<KeyValuePair<string, (int fishValue, int scoreAdd)>>(fishValues);

        Time.timeScale = 1.0f;
        transform.parent = null;
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SceneManager.GetActiveScene().buildIndex == 1)
            backgroundManager.Initialize(depthCoeff);

        for (int i = 0; i < fishSpawner.Length && i < fishValuesList.Count; i++)
        {
            if (i > 0)
                fishSpawner[i].SetFishValue(fishValuesList[i-1].Value.fishValue);
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Сбрасываем флаг при загрузке новой сцены
        _isSceneChanging = false;
    }
    private void Update()
    {
        if (_isSceneChanging)
            return;

        if (SceneManager.GetActiveScene().buildIndex == 1 && !gameOver)
        {
            int currentScore = Score.instance.GetScore();
            LevelProgress(currentScore);

            if (currentScore >= fishValuesList[currentFishLevel].Value.fishValue)
                FishNextLevel();

            maxPlayerHeight = backgroundManager.GetMaxPlayerHeight();
        }
    }
    private void LevelProgress(int currentScore)
    {
        if (currentLevel >= levelSettings.Count)
            return;

        var settings = levelSettings[currentLevel];
        if (currentScore >= settings.RequiredScore && !hasIncreasedSpawn[currentLevel])
        {
            CheckLevelProgress(settings.BackgroundMoveDistance);
            ApplyLevelSettings(settings);
            hasIncreasedSpawn[currentLevel] = true;
            currentLevel++;
        }
    }

     private void FishNextLevel()
     {
        currentFishLevel++;
        Progress.Instance.PlayerInfo.Level = currentFishLevel;

        if (currentFishLevel < fishSpawner.Length)
        {
            fishSpawner[currentFishLevel].IsWhiteSwitch();
            fishSpawner[currentFishLevel].HideRedObjects();
        }
     }

    private void ApplyLevelSettings(LevelSettings settings)
    {
        foreach (var (spawnerIndex, spawnRate, spawnAmount, levelHigh) in settings.SpawnActions)
        {
            fishSpawner[spawnerIndex].IncreaseSpawnRate(spawnRate, spawnAmount, levelHigh);
        }
    }
    public void GameOver()
    {
        // Останавливаем музыку
      //  if (gameMusic != null)   gameMusic.Stop(); 

        Time.timeScale = 0;
        Score.instance.UpdateGold();
        // Проверяем, если панель уже не создана
        if (gameOverInstance == null)
        {
            gameOverInstance = Instantiate(gameOverPrefab);
        }
    }
    public void ResumeGame()
    {
        if (gameOverInstance != null)
        {
            Destroy(gameOverInstance);
            gameOverInstance = null;
            Time.timeScale = 1;
            gameOver = false;
            gameMusic?.Play();
        }
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
    private void CheckLevelProgress(float distance)
    {
        backgroundManager.MoveBackground(distance); // Задаем смещение фона
    }
}

// Структура для описания действий на уровне
// Объединенный класс для настроек уровня и спавнера
[System.Serializable]
public class LevelSettings
{
    public int RequiredScore; // Порог очков для перехода на уровень
    public float BackgroundMoveDistance; 
    public List<(int spawnerIndex, float spawnRate, int spawnAmount, int levelHight)> SpawnActions; // Действия для увеличения спавна

    public LevelSettings(int requiredScore, float backgroundMoveDistance, params (int spawnerIndex, float spawnRate, int spawnAmount, int levelHigh)[] actions)
    {
        RequiredScore = requiredScore;
        BackgroundMoveDistance = backgroundMoveDistance;
        SpawnActions = new List<(int spawnerIndex, float spawnRate, int spawnAmount, int levelHigh)>(actions);
    }
}