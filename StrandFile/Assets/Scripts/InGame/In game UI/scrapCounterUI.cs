using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class scrapCounterUI : MonoBehaviour
{
    TextMeshProUGUI getText;
    // Start is called before the first frame update
    void Start()
    {
        getText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        getText.text = ":" + PlayerPrefs.GetInt("Scrap", 0);
    }
}
