using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class gameOverStatsScript : textTyper
{
    public override void setTextStart()
    {
        Time.timeScale = 1;
        TextMeshProUGUI textControl = gameObject.GetComponent<TextMeshProUGUI>();
        if (scrapStorer.checkInitialized())
        {
            scrapStorer.setScrap(scrapStorer.getScrap() + PlayerPrefs.GetInt("Scrap", 0));
        }
        string retText = "Scrap Collected: " + PlayerPrefs.GetInt("Scrap", 0) + "\nNights Spent: " + PlayerPrefs.GetInt("daysSpent", 0) + "\nTotal Scrap Stored: " + scrapStorer.getScrap();
        setText(retText);
        PlayerPrefs.DeleteAll();
        base.setTextStart();
    }
}
