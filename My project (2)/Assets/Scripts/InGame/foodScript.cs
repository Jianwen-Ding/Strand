using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodScript : MonoBehaviour
{
    [SerializeField]
    float foodFill;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Camera.main.GetComponent<resourceSystem>().fillHunger(foodFill);
            Destroy(gameObject);
        }
    }
}
