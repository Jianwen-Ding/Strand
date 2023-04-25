using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clockUI : MonoBehaviour
{
    [SerializeField]
    private float startPoint;
    [SerializeField]
    private float endPoint;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(nightSystem.getCurrentTimePassed() < nightSystem.getTimeUntilNight())
        {
            gameObject.transform.localPosition = new Vector3((endPoint - startPoint) * (nightSystem.getCurrentTimePassed() / nightSystem.getTimeUntilNight()) + startPoint, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        }  
    }
}
