using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static Score instance;
    private int _score = 5;

    [SerializeField] private TextMeshProUGUI _playerScoreText;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _highScoreText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        _playerScoreText.text = _score.ToString();
        _goldText.text = PlayerPrefs.GetInt("GoldScore", 0).ToString();
        _highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        UpdateHighScore();
    }

    private void UpdateHighScore()
    {
        if(_score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", _score);
            _highScoreText.text = _score.ToString();
        }
    }

    public void UpdateScore(int value)
    {
        _score += value;
        _playerScoreText.text = _score.ToString();
        UpdateHighScore();
    }
    public int GetScore()
    {
        return _score;
    }

    public void UpdateGold()
    {
        int _gold = PlayerPrefs.GetInt("GoldScore", 0);
        _gold += _score;
        PlayerPrefs.SetInt("GoldScore", _gold);
        _goldText.text = _gold.ToString();
    }
    public void Obnulit()
    {
        PlayerPrefs.SetInt("GoldScore", 0);
        PlayerPrefs.SetInt("HighScore", 0);
    }
}
