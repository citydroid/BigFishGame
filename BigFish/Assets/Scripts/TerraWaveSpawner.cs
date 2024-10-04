using Movers;
using UnityEngine;

public class TerraWaveSpawner : MonoBehaviour
{
    [SerializeField] private GameObject baseObject;
    [SerializeField] private float MaxTime = 1.0f;
    [SerializeField] private GameObject _terra;
    
    private float darkerColor = 1f;  // Новый цвет для спрайта

    private float height = 0;
    private float _timer = 0;
 
    //private int terra = 0;

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
        Vector3 position = transform.position + new Vector3(0, Random.Range(-height, height), 0);

        GameObject _newFish = Instantiate(_terra, position, Quaternion.identity);
        _newFish.transform.SetParent(baseObject.transform, false);
        ChangeObjectColor(_newFish, darkerColor);
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
}
