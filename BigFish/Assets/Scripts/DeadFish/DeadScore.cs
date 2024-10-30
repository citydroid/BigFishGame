using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeadScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI addScoreText;

    public void ScoreValue(int value)
    {
        addScoreText.text = value.ToString();
    }
}
