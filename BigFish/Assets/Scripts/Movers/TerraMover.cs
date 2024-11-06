using UnityEngine;

namespace Movers
{
    public class TerraMover : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float targetX = -5.3f;
        [SerializeField] private SpriteRenderer spriteRenderer;
        void Update()
        {
            transform.position += Vector3.left * _speed * Time.deltaTime;

            if (transform.position.x <= targetX)
            {
                Destroy(gameObject);
            }
        }
        // Метод для изменения цвета спрайта с указанием градации серого
        public void SetGrayScale(float grayLevel)
        {
            // Ограничиваем уровень серого от 0 (черный) до 1 (белый)
            grayLevel = Mathf.Clamp01(grayLevel);
            // Устанавливаем одинаковые значения R, G, B для создания серого оттенка
            Color grayColor = new Color(grayLevel, grayLevel, grayLevel);
            if (spriteRenderer != null)
            {
                spriteRenderer.color = grayColor;
            }
        }
    }
}