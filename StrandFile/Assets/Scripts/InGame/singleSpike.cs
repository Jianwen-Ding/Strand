using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleSpike : MonoBehaviour
{
    private spikeSystem system = null;
    private void Start()
    {
        system = Camera.main.GetComponent<spikeSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(system != null)
        {
            system.attemptAdd(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(system != null)
        {
            system.attemptDelete(collision.gameObject);
        }
    }
}
