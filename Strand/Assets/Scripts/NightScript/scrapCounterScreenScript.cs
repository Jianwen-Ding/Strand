using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class scrapCounterScreenScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = "Scrap Collected: " + PlayerPrefs.GetInt("Scrap", 0);
    }
}
