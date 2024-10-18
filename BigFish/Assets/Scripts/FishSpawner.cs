using Movers;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private float maxTime;
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private int spawnCount = 0;
    [SerializeField] private bool isWhite = false;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private int maxFish;

    [SerializeField] private int level = 1; // Уровень спауна
    private SpawnLevelManager spawnLevelManager;

    private float _timer = 0f;
    private GameObject _baseObject;
    private float heightStoper = 1;

    private FishMover fishMover;
    private float maxHeight = 1f;
    private float depthCoeff = 0.0002f;

        // Список для отслеживания активных объектов рыб
    private List<GameObject> activeFishObjects = new List<GameObject>();
    void Start()
    {
        _baseObject = GameObject.Find("Base");
        spawnLevelManager = new SpawnLevelManager(maxHeight);
        SpawnFish();
    }

    void Update()
    {
        _timer += Time.deltaTime;
        // Удаляем уничтоженные рыбы из списка
        RemoveDestroyedFish();
        // Спавним новых рыб только если количество активных меньше maxFish
        if (_timer > maxTime && activeFishObjects.Count <= maxFish)
        {
            maxHeight = gameManager.GetMaxPlayerHeight();
            spawnLevelManager.SetMaxHeight(maxHeight);
            SpawnFish();
            _timer = 0;
        }
    }

    private void SpawnFish()
    {
        //heightStoper = gameManager.GetMaxPlayerHeight() + 0.9f;
        var (heightMin, heightMax) = spawnLevelManager.GetHeightRange(level);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 position = transform.position + new Vector3(0, Random.Range(heightMin, heightMax), 0);
Debug.Log("heightMin " + heightMin + " heightMax " + heightMax);
            GameObject newFish = Instantiate(fishPrefab, position, Quaternion.identity);
                newFish.transform.SetParent(_baseObject.transform, false);

                newFish.GetComponent<FishMover>().SetDepth(maxHeight, depthCoeff);

                //Ищем Fish и задаём случайный размер в процентах от существующего
                Transform scaleTransform = newFish.transform.Find("Fish");

                if (scaleTransform != null)
                {
                    // Получаем текущий масштаб объекта Fish
                    Vector3 currentScale = scaleTransform.localScale;
                    float randCoef = Random.Range(0.7f, 1f);// Масштаб X от 60% до 100%
                    float randomScaleX = currentScale.x * randCoef;
                    float randomScaleY = currentScale.y * randCoef;

                    scaleTransform.localScale = new Vector3(randomScaleX, randomScaleY, currentScale.z);
                }

                if (isWhite)
                {
                    Transform textTransform = newFish.transform.Find("Red");
                    GameObject valueObject = textTransform.gameObject;
                    if (valueObject != null)
                    {
                        valueObject.SetActive(false);
                    }
                }
                // Добавляем новую рыбу в список активных объектов
                activeFishObjects.Add(newFish);
        }
    }
    private void RemoveDestroyedFish()
    {
        // Очищаем список от уничтоженных рыб
        activeFishObjects.RemoveAll(fish => fish == null);
    }
    public void IsWhiteSwitch()
    {
        isWhite = true;
    }
    public void IncreaseSpawnRate(float newMaxTime, int newSpawnCount, int newLevel)
    {
        maxTime = newMaxTime;
        spawnCount = newSpawnCount;
        level = newLevel;  // Устанавливаем новый уровень
    }
}

