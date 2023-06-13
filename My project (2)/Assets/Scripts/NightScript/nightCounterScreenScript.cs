using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class nightCounterScreenScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = "Night " + PlayerPrefs.GetInt("daysSpent", 0);
    }
}
