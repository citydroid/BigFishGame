using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSpawner : MonoBehaviour
{
    public GameObject _fish; // Префаб рыбы
    public float destroyDelay = 1f;  // Время до уничтожения объекта

    // Этот метод вызывается для создания объекта и его уничтожения
    public void SpawnAndDestroy(Vector3 spawnPosition)
    {
        // Создаём объект при поедании
        GameObject _newFish = Instantiate(_fish, spawnPosition, Quaternion.identity);

        // Анимация включается
        Animator animator = _newFish.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("FishAnimation"); // Название вашей анимации
        }

        // Уничтожаем объект через destroyDelay секунд
        Destroy(_newFish, destroyDelay);
    }
}
