using Movers;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private float maxTime;
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private float heightMin = 0;
    [SerializeField] private float heightMax;
    [SerializeField] private int spawnCount = 0;
    [SerializeField] private bool isWhite = false;
    [SerializeField] private GameManager gameManager;

    private float _timer = 0f;
    private GameObject _baseObject;
    private float heightStoper = 1;

    private FishMover fishMover;
    private float maxHeight = -0.5f;
    private float depthCoeff = 0.0002f;

    void Start()
    {
        _baseObject = GameObject.Find("Base");
        SpawnFish();
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > maxTime)
        {
            SpawnFish();
            _timer = 0;
        }

        maxHeight += depthCoeff;
    }

    private void SpawnFish()
    {
        heightStoper = gameManager.GetMaxPlayerHeight() + 0.9f;
        for (int i = 0; i < spawnCount; i++)
        {
            if (heightStoper < heightMax) heightMax = heightStoper;

            Vector3 position = transform.position + new Vector3(0, Random.Range(heightMin, heightMax), 0);

            GameObject newFish = Instantiate(fishPrefab, position, Quaternion.identity);
            newFish.transform.SetParent(_baseObject.transform, false);

            //fishMover.SetDepth(maxHeight,depthCoeff);
            newFish.GetComponent<FishMover>().SetDepth(maxHeight, depthCoeff);

            //Ищем Fish и задаём случайный размер в процентах от существующего
            Transform scaleTransform = newFish.transform.Find("Fish");

            if (scaleTransform != null)
            {
                // Получаем текущий масштаб объекта Fish
                Vector3 currentScale = scaleTransform.localScale;
                float randCoef = Random.Range(0.6f, 1f);// Масштаб X от 60% до 100%
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
        }
    }
    public void SetMaxHeght(float newMaxHeight, float newDepthCoeff)
    {
        maxHeight = newMaxHeight;
        depthCoeff = newDepthCoeff;
    }
    public void IsWhiteSwitch()
    {
        isWhite = true;
    }
    public void IncreaseSpawnRate(float newMaxTime, int newSpawnCount, float newhightMin, float newhightMax)
    {
        maxTime = newMaxTime;
        spawnCount = newSpawnCount;
        heightMin = newhightMin;
        heightMax = newhightMax;
    }
}
