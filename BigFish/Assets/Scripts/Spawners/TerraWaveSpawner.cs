using Movers;
using UnityEngine;

public class TerraWaveSpawner : MonoBehaviour
{
    [SerializeField] private GameObject baseObject;
    [SerializeField] private float MaxTime = 1.0f;
    [SerializeField] private GameObject[] terraPrefabs;
    [SerializeField] private int currentLevel = 0;

    private float darkerColor = 1f;  // Новый цвет для спрайта

    private float height = 0;
    private float _timer = 0;

    void Start()
    {
        Spawner();
    }

    void Update()
    {
        if (_timer > MaxTime)
        {
            Spawner();
            _timer = 0;
        }

        _timer += Time.deltaTime;
    }
    private void Spawner()
    {
        if (terraPrefabs == null || terraPrefabs.Length == 0)
            return;

        Vector3 position = transform.position + new Vector3(0, Random.Range(-height, height), 0);

        GameObject _newTerra = Instantiate(terraPrefabs[currentLevel], position, Quaternion.identity);
        _newTerra.transform.SetParent(baseObject.transform, false);
        ChangeObjectColor(_newTerra, darkerColor);
    }
    private void ChangeObjectColor(GameObject existingObject, float grayLevel)
    {
        TerraMover mover = existingObject.GetComponent<TerraMover>();
        if (mover != null)
        {
            mover.SetGrayScale(grayLevel);  // Устанавливаем новый уровень серого
        }
    }
    public void ChangeGrayLevel(float depthCoeff)
    {
        darkerColor -= depthCoeff;
    }
    public void SetLevel(int level)
    {
        Debug.Log(level);

        currentLevel = level;
        Spawner();
    }
}
