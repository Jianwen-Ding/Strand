using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class onEscapeScript : textTyper
{
    [SerializeField]
    int escapeBonus;
    TextMeshProUGUI objectText;
    public override void setTextStart()
    {
        base.setTextStart();
        Time.timeScale = 1;
        objectText = gameObject.GetComponent<TextMeshProUGUI>();
        if (scrapStorer.checkInitialized())
        {
            scrapStorer.setScrap(scrapStorer.getScrap() + PlayerPrefs.GetInt("Scrap", 0) + escapeBonus);
            scrapStorer.setEscapes(scrapStorer.getEscapes() + 1);
        }
        string returnText = "Scrap Collected: " + PlayerPrefs.GetInt("Scrap", 0) + "\n + (ESCAPE BONUS)" + escapeBonus + "\n\nNights Spent: " + PlayerPrefs.GetInt("daysSpent", 0) + "\n\nTotal \nScrap Stored: " + scrapStorer.getScrap();
        setText(returnText);
        PlayerPrefs.DeleteAll();
    }
}
