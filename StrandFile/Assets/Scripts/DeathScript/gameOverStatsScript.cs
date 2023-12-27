using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class gameOverStatsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        TextMeshProUGUI textControl = gameObject.GetComponent<TextMeshProUGUI>();
        if (scrapStorer.checkInitialized())
        {
            scrapStorer.setScrap(scrapStorer.getScrap() + PlayerPrefs.GetInt("Scrap", 0));
        }
        textControl.text = "Scrap Collected: " + PlayerPrefs.GetInt("Scrap", 0) + "\nNights Spent: " + PlayerPrefs.GetInt("daysSpent", 0) + "\nTotal Scrap Stored: " + scrapStorer.getScrap();
        PlayerPrefs.DeleteAll();
    }
}
