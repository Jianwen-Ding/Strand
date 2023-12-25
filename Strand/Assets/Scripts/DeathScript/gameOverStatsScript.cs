using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class gameOverStatsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI textControl = gameObject.GetComponent<TextMeshProUGUI>();
        textControl.text = "Scrap Collected: " + PlayerPrefs.GetInt("Scrap", 0) + "\nNights Spent: " + PlayerPrefs.GetInt("daysSpent", 0);
        PlayerPrefs.DeleteAll();
    }
}
