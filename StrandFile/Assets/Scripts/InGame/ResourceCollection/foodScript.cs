using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodScript : MonoBehaviour
{
    [SerializeField]
    float foodFill;
    [SerializeField]
    GameObject foodMarker;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(foodMarker, gameObject.transform.position, Quaternion.identity.normalized);
            collision.gameObject.GetComponent<playerAudio>().triggerActiveAudioState("pickUp");
            Camera.main.GetComponent<resourceSystem>().fillHunger(foodFill);
            Destroy(gameObject);
        }
    }
}
