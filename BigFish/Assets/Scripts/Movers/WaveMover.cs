using UnityEngine;

namespace Movers
{
    public class WaveMover : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float targetX = -5f;

        private float _verticalSpeed = 0f;
        void Update()
        {
            transform.position += Vector3.left * _speed * Time.deltaTime;

            // ”ничтожаем объект, если он выходит за пределы экрана
            if (transform.position.x <= targetX)
            {
                Destroy(gameObject);
            }
        }
        public void SetVerticalSpeed(float verticalSpeed)
        {
            _verticalSpeed = verticalSpeed;
        }
    }
}