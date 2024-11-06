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
    private float depthCoeff = 0.05f;

    private readonly Dictionary<string, (int fishValue, int scoreAdd)> fishValues = new Dictionary<string, (int fishValue, int scoreAdd)>
    {
        { "Fish_1", (2, 1) },
        { "Fish_2", (20, 3) },
        { "Fish_3", (50, 5) },
        { "Fish_4", (90, 8) },
        { "Fish_5", (200, 15) },
        { "Fish_6", (400, 25) },
        { "Fish_7", (1000, 30) },
        { "Fish_8", (1600, 35) },
        { "Fish_9", (2300, 40) },
        { "Fish_10", (3100, 45) },
        { "Fish_11", (4100, 50) }
    };
    private List<KeyValuePair<string, (int fishValue, int scoreAdd)>> fishValuesList;

    private readonly bool[] hasIncreasedSpawn = new bool[30];
    private int currentLevel = 0;
    private readonly List<LevelSettings> levelSettings = new List<LevelSettings>
    {
        // 9f 
        new LevelSettings(0, 0f, new[] { (1, 0.5f, 1, 2), (2, 1.5f, 1, 1) }),
        new LevelSettings(10, 0.3f, new[] { (1, 1f, 1, 5), (3, 5f, 2, 2) }),
        new LevelSettings(20, 0.3f, new[] { (2, 10f, 1, 1), (3, 0.5f, 1, 3)}),
        new LevelSettings(40, 0.3f, new[] { (1, 3f, 2, 5), (2, 3f, 1, 1), (3, 3f, 1, 3), (4, 5f, 1,3), (5, 4f, 1, 5) }),
        new LevelSettings(50, 0.3f, new[] { (1, 3f, 2, 5), (2, 3f, 1, 1), (3, 3f, 1, 3), (4, 0f, 0,3), (6, 1f, 1, 5) }),
        new LevelSettings(90, 0.3f, new[] { (1, 0.5f, 3, 5), (2, 0f, 0, 0), (3, 3f, 1, 4), (4, 1f, 1, 4), (5, 10f, 1, 4), (7, 0.5f, 1, 1) }),
        new LevelSettings(200, 0.3f, new[] { (1, 5f, 10, 6), (3, 1f, 1,4), (4, 1f, 1, 4), (6, 0f, 0, 0), (8, 1f, 1, 1) }),
        new LevelSettings(400, 0.3f, new[] { (3, 1f, 3, 3), (4, 1f, 1, 4), (5, 1f, 1, 2), (9, 3f, 3, 5) }),
        new LevelSettings(1000, 0.3f, new[] { (2, 2f, 1, 1), (3, 1f, 1, 4), (4, 1f, 1, 5), (10, 3f, 3, 4) }),
        new LevelSettings(1600, 0.3f, new[] { (3, 1f, 3, 3), (4, 1f, 1, 4), (5, 1f, 1, 2), (8, 3f, 3, 5) }),
        new LevelSettings(2300, 0.3f, new[] { (3, 1f, 3, 3), (4, 1f, 1, 4), (5, 1f, 1, 2), (8, 3f, 3, 5) }),
        new LevelSettings(3100, 0.3f, new[] { (3, 1f, 3, 3), (4, 1f, 1, 4), (5, 1f, 1, 2), (8, 3f, 3, 5) })
    };
    private void Awake()
    {  
        InitializeFishValues();
        InitializeBackground();

        Time.timeScale = 1.0f;
        SceneManager.sceneLoaded += OnSceneLoaded;

        InitializeFishSpawnerValues();
    }
    private void InitializeFishValues()
    {
        fishValuesList = new List<KeyValuePair<string, (int fishValue, int scoreAdd)>>(fishValues);
    }
    private void InitializeBackground()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            backgroundManager.Initialize(depthCoeff);
        }
    }
    private void InitializeFishSpawnerValues()
    {
        for (int i = 1; i < fishSpawner.Length && i < fishValuesList.Count; i++)
        {
            fishSpawner[i].SetFishValue(fishValuesList[i - 1].Value.fishValue);
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _isSceneChanging = false;
    }
    private void Update()
    {
        if (_isSceneChanging)
            return;

        if (SceneManager.GetActiveScene().buildIndex == 1 && !gameOver)
        {
            int currentScore = Score.instance.GetScore();
            LevelProgressUpdate(currentScore);

            if (currentScore >= fishValuesList[currentFishLevel].Value.fishValue)
                FishLevelUpdate();

            maxPlayerHeight = backgroundManager.GetMaxPlayerHeight();
        }
    }
    private void LevelProgressUpdate(int currentScore)
    {
        if (currentLevel >= levelSettings.Count)
            return;

        var settings = levelSettings[currentLevel];
        if (currentScore >= settings.RequiredScore && !hasIncreasedSpawn[currentLevel])
        {
            BackgroundMove(settings.BackgroundMoveDistance);
            ApplyLevelSettings(settings);
            hasIncreasedSpawn[currentLevel] = true;
            currentLevel++;
        }
    }

     private void FishLevelUpdate()
     {
        Debug.Log("currentFishLevel  " + currentFishLevel);
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

        if (gameOverInstance == null)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            gameOverInstance = Instantiate(gameOverPrefab);
            gameOverInstance.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, 0f);
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
    public float GetMaxPlayerHeight() => maxPlayerHeight;
    private void BackgroundMove(float distance)
    {
        backgroundManager.MoveBackground(distance); 
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