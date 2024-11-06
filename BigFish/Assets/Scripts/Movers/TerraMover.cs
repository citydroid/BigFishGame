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
        // ����� ��� ��������� ����� ������� � ��������� �������� ������
        public void SetGrayScale(float grayLevel)
        {
            // ������������ ������� ������ �� 0 (������) �� 1 (�����)
            grayLevel = Mathf.Clamp01(grayLevel);
            // ������������� ���������� �������� R, G, B ��� �������� ������ �������
            Color grayColor = new Color(grayLevel, grayLevel, grayLevel);
            if (spriteRenderer != null)
            {
                spriteRenderer.color = grayColor;
            }
        }
    }
}