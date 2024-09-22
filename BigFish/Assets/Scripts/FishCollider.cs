using System.Collections.Generic;
using UnityEngine;

public class FishCollider : MonoBehaviour
{
    private readonly Dictionary<string, (int fishValue, int scoreAdd)> fishValues = new Dictionary<string, (int fishValue, int scoreAdd)>
    {
        { "Fish_2", (2, 1) },
        { "Fish_10", (10, 5) },
        { "Fish_50", (50, 10) },
        { "Fish_150", (150, 15) },
        { "Fish_250", (250, 22) },
        { "Fish_375", (375, 25) },
        { "Fish_500", (500, 30) }
        // ћожно добавить другие рыбы по аналогии
    };

    public bool TryGetFishValue(string fishTag, out int fishValue, out int scoreAdd)
    {
        if (fishValues.TryGetValue(fishTag, out var values))
        {
            fishValue = values.fishValue;
            scoreAdd = values.scoreAdd;
            return true;
        }

        fishValue = 0;
        scoreAdd = 0;
        return false;
    }
}        /*
        else if (collision.gameObject.CompareTag("Fish_750"))  {
            fishValue = 750;
            scoreAdd = 35;
        }
        else if (collision.gameObject.CompareTag("Fish_1000")) {
            fishValue = 1000;
            scoreAdd = 40;
        }
        else if (collision.gameObject.CompareTag("Fish_1500")) {
            fishValue = 1500;
            scoreAdd = 45;
        }
        else if (collision.gameObject.CompareTag("Fish_2250")) {
            fishValue = 2250;
            scoreAdd = 50;
        }
        else if (collision.gameObject.CompareTag("Fish_3000")) {
            fishValue = 3000;
            scoreAdd = 60;
        }
        else if (collision.gameObject.CompareTag("Fish_4000")) {
            fishValue = 4000;
            scoreAdd = 70;
        }       
        else if (collision.gameObject.CompareTag("Fish_5500")) {
             fishValue = 5500;
             scoreAdd = 80;
        }
        else if (collision.gameObject.CompareTag("Fish_7500")) {
             fishValue = 7500;
             scoreAdd = 90;
        }
        else if (collision.gameObject.CompareTag("Fish_10000")) {
             fishValue = 10000;
             scoreAdd = 100;
        }
        */