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

    private readonly bool[] hasIncreasedSpawn = new bool[30];

    private bool _isSceneChanging = false;
    private int currentLevel = 1;
    private float maxPlayerHeight; //4f
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

    // ������ �������� �������
    private readonly List<LevelSettings> levelSettings = new List<LevelSettings>
    {
        // ��������� ��� ������ 0 (# ����, ����� ����� ��������, ���������� ���� �� �����, ���.������, ����.������)
        new LevelSettings(0, new[] { (1, 2f, 1, 0f, 1f), (2, 5f, 1, 0f, 0.1f) }),
        new LevelSettings(3, new[] { (1, 5f, 10, 0f, 1f) }),
        new LevelSettings(10, new[] { (1, 1f, 1, 0.5f, 5f), (2, 3f, 1, 0f, 0.1f), (3, 5f, 2, 0f, 1f) }),
        new LevelSettings(50, new[] { (2, 10f, 1, 0f, 1f), (3, 10f, 1, 0f, 1f), (4, 2f, 1, 0f, 1f) }),
        new LevelSettings(100, new[] { (1, 3f, 2, 3f, 10f), (2, 3f, 1, 0f, 0.1f), (4, 5f, 1, 0f, 1f), (5, 1f, 2, 0f, 3f) }),
        new LevelSettings(150, new[] { (1, 0.5f, 3, 4f, 10f), (2, 1f, 1, 0f, 1f), (3, 3f, 1, 0f, 1f), (4, 1f, 1, 0f, 1f), (5, 0f, 0, 0f, 1f), (6, 1f, 3, 0f, 5f) }),
        new LevelSettings(250, new[] { (1, 1f, 1, 5f, 15f), (2, 0f, 0, 0f, 1f), (3, 1f, 1, 0f, 1f), (4, 1f, 1, 0f, 1f), (5, 1f, 1, 0f, 1f), (6, 1f, 1, 2f, 5f) }),
        new LevelSettings(375, new[] { (3, 1f, 3, 0f, 1f), (4, 1f, 1, 0f, 1f), (5, 1f, 1, 0f, 1f), (7, 3f, 3, 0f, 0f) }),
        new LevelSettings(500, new[] { (2, 2f, 1, 0f, 1f), (3, 1f, 1, 0f, 1f), (4, 1f, 1, 0f, 1f), (8, 3f, 3, 0.5f, 1f) })

    };
    public void Start()
    {   // ����������� ������� � ������ ���
        fishValuesList = new List<KeyValuePair<string, (int fishValue, int scoreAdd)>>(fishValues);

        Time.timeScale = 1.0f;
        transform.parent = null;
        // ������������� �� ������� �������� ����� �����
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            backgroundManager.Initialize(depthCoeff);
        }

        // �������� ������� ���������� ������
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        // ����������� ������ ������
        float aspectRatio = screenWidth / screenHeight;
        // ������� ����������� �����������, ������������ ��� ������������ ��������
        float normalizedSpeed = depthCoeff * screenWidth / 1920f; // 1920 - ������� ���������� (��������, ��� Full HD)
        Debug.Log("normalizedSpeed " + normalizedSpeed + " aspectRatio " + aspectRatio + " screenWidth " + screenWidth);
      //  depthCoeff = normalizedSpeed;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���������� ���� ��� �������� ����� �����
        _isSceneChanging = false;
    }
    private void Update()
    {
        if (_isSceneChanging) return;

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (!gameOver)
            {
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    int currentScore = Score.instance.GetScore();
                    ProcessLevelProgress(currentScore);

                    if (currentScore >= fishValuesList[currentLevel].Value.fishValue)
                    {
                        ProgressToNextLevel();
                    }

                    UpdateBackgroundPosition();
                }
            }
        }
    }
            private void ProcessLevelProgress(int currentScore)
            {
                if (currentLevel >= levelSettings.Count) return;

                var settings = levelSettings[currentLevel];
                if (currentScore >= settings.RequiredScore && !hasIncreasedSpawn[currentLevel])
                {
                    ApplyLevelSettings(settings);
                    hasIncreasedSpawn[currentLevel] = true;
                    UpdateFishSpawners();
                }
            }

            private void ProgressToNextLevel()
            {
                currentLevel++;
                Progress.Instance.PlayerInfo.Level = currentLevel;

                if (currentLevel < fishSpawner.Length)
                {
                    fishSpawner[currentLevel].IsWhiteSwitch();
                }

                HideRedObjects(currentLevel);
                AdjustFishVerticalControl(currentLevel);
            }

            private void UpdateFishSpawners()
            {
                foreach (FishSpawner spawner in fishSpawner)
                {
                    spawner.SetMaxHeght(maxPlayerHeight, depthCoeff);
                }
            }

            private void ApplyLevelSettings(LevelSettings settings)
            {
                foreach (var (spawnerIndex, spawnRate, spawnAmount, hightMin, hightMax) in settings.SpawnActions)
                {
                    fishSpawner[spawnerIndex].IncreaseSpawnRate(spawnRate, spawnAmount, hightMin, hightMax);
                }
            }

            private void HideRedObjects(int fishLevel)
            {
                GameObject[] fishObjects = GameObject.FindGameObjectsWithTag($"Fish_{fishLevel}");

                foreach (GameObject fish in fishObjects)
                {
                    Transform redTransform = fish.transform.Find("Red");
                    if (redTransform != null)
                    {
                        redTransform.gameObject.SetActive(false);
                    }
                }
            }

            private void AdjustFishVerticalControl(int fishLevel)
            {
                GameObject[] fishObjects = GameObject.FindGameObjectsWithTag($"Fish_{fishLevel}");
                foreach (GameObject fish in fishObjects)
                {
                    fish.GetComponent<FishMover>().SetMaxVertical(maxPlayerHeight);
                }
            }


    public void GameOver()
    {
        // ������������� ������
      //  if (gameMusic != null)   gameMusic.Stop(); 

        Time.timeScale = 0;
        Score.instance.UpdateGold();
        // ���������, ���� ������ ��� �� �������
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
    private void UpdateBackgroundPosition()
    {
        backgroundManager.UpdateBackgroundPosition();
        maxPlayerHeight = backgroundManager.GetMaxPlayerHeight();
    }
}

// ��������� ��� �������� �������� �� ������
// ������������ ����� ��� �������� ������ � ��������
[System.Serializable]
public class LevelSettings
{
    public int RequiredScore; // ����� ����� ��� �������� �� �������
    public List<(int spawnerIndex, float spawnRate, int spawnAmount, float hightMin, float hightMax)> SpawnActions; // �������� ��� ���������� ������

    public LevelSettings(int requiredScore, params (int spawnerIndex, float spawnRate, int spawnAmount, float hightMin, float hightMax)[] actions)
    {
        RequiredScore = requiredScore;
        SpawnActions = new List<(int spawnerIndex, float spawnRate, int spawnAmount, float hightMin, float hightMax)>(actions);
    }
}
//Time.timeScale = 0;
//AudioListener.pause = false;