using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseSystem : MonoBehaviour
{
    [SerializeField]
    resourceSystem resource;
    [SerializeField]
    float distanceUntilSleepOptionAppears;
    [SerializeField]
    GameObject sleepButton;
    [SerializeField]
    PlayerMainScript playerScript;
    [SerializeField]
    float yOffSet;
    public void Start()
    {
        playerScript = FindObjectOfType<PlayerMainScript>();
        resource = FindObjectOfType<resourceSystem>();
        sleepButton = GameObject.FindGameObjectWithTag("SleepButton");
    }
    public void Update()
    {
        float diffrence = Mathf.Sqrt((playerScript.gameObject.transform.position.x - transform.position.x) * (playerScript.gameObject.transform.position.x - transform.position.x) + (playerScript.gameObject.transform.position.y - (transform.position.y + yOffSet)) * (playerScript.gameObject.transform.position.y - (transform.position.y + yOffSet)));
        if(diffrence < distanceUntilSleepOptionAppears)
        {
            sleepButton.SetActive(true);
        }
        else
        {
            sleepButton.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerMainScript playerScript = collision.gameObject.GetComponent<PlayerMainScript>();
            if(playerScript != null && playerScript.getHandScript().getGrabbedObject() != null && playerScript.getHandScript().getGrabbedObject().tag == "Scrap")
            {
                GameObject storeObject = playerScript.getHandScript().getGrabbedObject();
                playerScript.getHandScript().releaseObject();
                Destroy(storeObject);
                resource.addScrap(1);
            }
        }
        if(collision.gameObject.tag == "Scrap")
        {
            Destroy(collision.gameObject);
            resource.addScrap(1);
        }
    }
}
