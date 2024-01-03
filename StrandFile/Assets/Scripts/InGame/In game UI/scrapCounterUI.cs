using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class scrapCounterUI : MonoBehaviour
{
    TextMeshProUGUI getText;
    resourceSystem GetResourceSystem;
    // Start is called before the first frame update
    void Start()
    {
        GetResourceSystem = FindAnyObjectByType<resourceSystem>();
        getText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        getText.text = ": " + PlayerPrefs.GetInt("Scrap", 0) + "/" + (GetResourceSystem.getWinCondition() + 1);
    }
}
