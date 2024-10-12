using UnityEngine;
using TMPro;

public class Lives : MonoBehaviour
{
    public static Lives instance;
    private int _live= 0;
    public int NumberOfLives;

    [SerializeField] private TextMeshProUGUI _livesText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        NumberOfLives = Progress.Instance.PlayerInfo.Lives;
        _livesText.text = NumberOfLives.ToString();
        UpdateLives(0);
    }
    public void UpdateLives(int value)
    {
        _live += value;
        _livesText.text = _live.ToString();
    }
    public int GetScore()
    {
        return _live;
    }
    public void UpdateGold()
    {
        NumberOfLives += _live;
        SaveProgress(NumberOfLives);
    }
    public void SaveProgress(int lives)
    {
        Progress.Instance.PlayerInfo.Lives = lives;
    }
}
