using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    static UIManager manager;
    public TextMeshProUGUI orbText, timeText, deathText, gameOverText;
    private void Awake()
    {
        if (manager != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        manager = this;
    }
    public static void UpdateOrbUI(int orbCount)
    {
        manager.orbText.text = orbCount.ToString();
    }
    public static void UpdatePlayerDeath(int deathCount)
    {
        manager.deathText.text = deathCount.ToString();
    }
    public static void UpdateGameTime(float time)
    {
        int minute = (int)(time / 60);
        float second = time % 60;

        manager.timeText.text = minute.ToString("00") + ":" + second.ToString("00");
    }
    public static void DisplayGameOver()
    {
        manager.gameOverText.enabled = true;
    }
}
