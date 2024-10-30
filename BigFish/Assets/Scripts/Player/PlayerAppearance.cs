using UnityEngine;
public class PlayerAppearance : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    private Transform transformPlayer;
    private Animator animator;
    protected bool mirrorPlayer = false;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transformPlayer = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
    public void MirrorPlayer(float lastX, float currentX)
    {
        bool shouldMirror = (currentX > lastX && mirrorPlayer) || (currentX < lastX && !mirrorPlayer);

        if (shouldMirror)
        {
            Vector3 currentScale = transformPlayer.localScale;
            currentScale.x = -currentScale.x;
            transformPlayer.localScale = currentScale;
            mirrorPlayer = !mirrorPlayer;
        }
    }
    public void GameOverPlayer() // Включается в конце анимации PlayerDead
    {
        gameManager.GameOver();
    }
}