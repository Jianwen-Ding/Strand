using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class despawnTimer : MonoBehaviour
{
    [SerializeField]
    float timeUntilSelfDestruct;
    // Update is called once per frame
    void Update()
    {
        timeUntilSelfDestruct -= Time.deltaTime;
        if(timeUntilSelfDestruct < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
