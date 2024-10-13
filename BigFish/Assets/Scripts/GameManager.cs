using Movers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private FishSpawner[] fishSpawner;

    [SerializeField] private Transform backgroundTransform;
    [SerializeField] private TerraWaveSpawner groundSpawner1;
    [SerializeField] private TerraWaveSpawner groundSpawner2;
    [SerializeField] private BackgroundManager backgroundManager;

    [SerializeField] private GameObject gameOverPrefab;
    private GameObject gameOverInstance;
    private CanvasGroup canvasGroup;
    private float fadeDuration = 1f;
    private bool gameOver = false;
    public AudioSource gameMusic;

    private readonly bool[] isWhiteDo = new bool[30];
    private readonly bool[] hasIncreasedSpawn = new bool[30];

    private bool _isSceneChanging = false;
    private int currentLevel = 1;
    private float maxPlayerHeight = -0.5f; //4f
    private readonly float startBackgroundPosition = -2.5f; //1f
    private float depthCoeff = 0.0002f;

    private readonly Dictionary<string, (int fishValue, int scoreAdd)> fishValues = new Dictionary<string, (int fishValue, int scoreAdd)>
    {
        { "Fish_1", (2, 1) },
        { "Fish_2", (10, 5) },
        { "Fish_3", (50, 10) },
        { "Fish_4", (150, 15) },
        { "Fish_5", (250, 22) },
        { "Fish_6", (375, 25) },
        { "Fish_7", (500, 30) },
        { "Fish_8", (1000, 35) },
        { "Fish_9", (2250, 40) }
    };
    private List<KeyValuePair<string, (int fishValue, int scoreAdd)>> fishValuesList;

    // Список настроек уровней
    private readonly List<LevelSettings> levelSettings = new List<LevelSettings>
    {
        // Настройки для уровня 0 (# рыбы, пауза между спаунами, количество рыбы за спаун, мин.высота, макс.высота)
        new LevelSettings(0, new[] { (1, 2f, 1, 0f, 1f), (2, 5f, 1, 0f, 0.1f) }),
        new LevelSettings(3, new[] { (1, 5f, 10, 0f, 1f) }),
        new LevelSettings(10, new[] { (1, 1f, 1, 0.5f, 2f), (2, 3f, 1, 0f, 0.1f), (3, 5f, 2, 0f, 1f) }),
        new LevelSettings(50, new[] { (2, 10f, 1, 0f, 1f), (3, 10f, 1, 0f, 1f), (4, 2f, 1, 0f, 1f) }),
        new LevelSettings(100, new[] { (1, 3f, 2, 0f, 1f), (2, 3f, 1, 0f, 0.1f), (4, 5f, 1, 0f, 1f), (5, 1f, 2, 0f, 3f) }),
        new LevelSettings(150, new[] { (1, 0.5f, 3, 0f, 3f), (2, 1f, 1, 0f, 1f), (3, 3f, 1, 0f, 1f), (4, 1f, 1, 0f, 1f), (5, 0f, 0, 0f, 1f), (6, 1f, 3, 0f, 5f) }),
        new LevelSettings(250, new[] { (1, 1f, 1, 1f, 3f), (2, 0f, 0, 0f, 1f), (3, 1f, 1, 0f, 1f), (4, 1f, 1, 0f, 1f), (5, 1f, 1, 0f, 1f), (6, 1f, 1, 2f, 5f) }),
        new LevelSettings(375, new[] { (3, 1f, 3, 0f, 1f), (4, 1f, 1, 0f, 1f), (5, 1f, 1, 0f, 1f), (7, 3f, 3, 0f, 0f) }),
        new LevelSettings(500, new[] { (2, 2f, 1, 0f, 1f), (3, 1f, 1, 0f, 1f), (4, 1f, 1, 0f, 1f), (8, 3f, 3, 0.5f, 1f) })

    };
    public void Start()
    {   // Преобразуем словарь в список пар
        fishValuesList = new List<KeyValuePair<string, (int fishValue, int scoreAdd)>>(fishValues);

        Time.timeScale = 1.0f;
        transform.parent = null;
        // Подписываемся на событие загрузки новой сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            backgroundManager.Initialize(backgroundTransform, groundSpawner1, groundSpawner2, depthCoeff, maxPlayerHeight);
        }
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
            if (!gameOver)
            {
                int currentScore = Score.instance.GetScore();
                // Проверяем, если очки игрока достигли следующего fishValue для текущего уровня
                ProcessLevelProgress(currentScore);

                if (currentScore >= fishValuesList[currentLevel].Value.fishValue)
                {
                    Debug.Log("currentLevel " + currentLevel);
                    int fishValue = fishValuesList[currentLevel].Value.fishValue;
                    // Переходим на следующий уровень
                    currentLevel++;
                    // Скрываем объекты "Red" для всех рыб текущего уровня
                    HideRedObjectsIfScoreMet(currentLevel, fishValue);
                    FishVerticalControl(currentLevel); // Метод нужет только для создания события для после изменения уровня

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
    }
    private void HideRedObjectsIfScoreMet(int fishLevel, int fishValue)
    {
        // Логика для скрытия объектов "Red"
        GameObject[] allFishObjects = GameObject.FindGameObjectsWithTag($"Fish_{fishLevel}");

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
    private void FishVerticalControl(int fishValue)
    {
        // Логика для скрытия объектов "Red"
        GameObject[] allFishObjects = GameObject.FindGameObjectsWithTag($"Fish_{fishValue}");
        Vector3 playerPosition = playerObject.transform.position;
        foreach (GameObject fish in allFishObjects)
        {
            // fish.GetComponent<FishMover>().SetVerticalSpeed(playerPosition.y + 1f);
            fish.GetComponent<FishMover>().SetMaxVertical(maxPlayerHeight);
        }
    }
    private void ProcessLevelProgress(int currentScore)
    {
        if (currentLevel >= levelSettings.Count) return;

        var settings = levelSettings[currentLevel];
        if (currentScore >= settings.RequiredScore && !hasIncreasedSpawn[currentLevel])
        {
            ApplyLevelSettings(settings);
            //UpdateBackgroundPosition();
            hasIncreasedSpawn[currentLevel] = true;
            foreach (FishSpawner fish in fishSpawner)
            {
                fish.GetComponent<FishSpawner>().SetMaxHeght(maxPlayerHeight, depthCoeff);
            }
        }
    }
    private void ApplyLevelSettings(LevelSettings settings)
    {
        foreach (var (spawnerIndex, spawnRate, spawnAmount, hightMin, hightMax) in settings.SpawnActions)
        {
            fishSpawner[spawnerIndex].IncreaseSpawnRate(spawnRate, spawnAmount, hightMin, hightMax);
        }
    }
    public void GameOver()
    {
        // Останавливаем музыку
        if (gameMusic != null)
        {
            //gameMusic.Stop();
        }

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
        // Проверяем, существует ли панель на сцене
        if (gameOverInstance != null)
        {
            // Уничтожаем панель
            Destroy(gameOverInstance);
            gameOverInstance = null;

            Time.timeScale = 1;
            gameOver = false;

            if (gameMusic != null)
            {
                gameMusic.Play();
            }
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
    private void UpdateBackgroundPosition()
    {
        backgroundManager.UpdateBackgroundPosition();
        maxPlayerHeight = backgroundManager.GetMaxPlayerHeight();
    }
}

// Структура для описания действий на уровне
// Объединенный класс для настроек уровня и спавнера
[System.Serializable]
public class LevelSettings
{
    public int RequiredScore; // Порог очков для перехода на уровень
    public List<(int spawnerIndex, float spawnRate, int spawnAmount, float hightMin, float hightMax)> SpawnActions; // Действия для увеличения спавна

    public LevelSettings(int requiredScore, params (int spawnerIndex, float spawnRate, int spawnAmount, float hightMin, float hightMax)[] actions)
    {
        RequiredScore = requiredScore;
        SpawnActions = new List<(int spawnerIndex, float spawnRate, int spawnAmount, float hightMin, float hightMax)>(actions);
    }
}
//Time.timeScale = 0;
//AudioListener.pause = false;