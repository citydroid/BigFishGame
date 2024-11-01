
using TMPro;
using UnityEngine;

public class FishValueSetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fishValueText;
    public void SetFishValue(int value)
    {
        if (fishValueText != null)
        {
            fishValueText.text = value.ToString();
        }
    }
}
