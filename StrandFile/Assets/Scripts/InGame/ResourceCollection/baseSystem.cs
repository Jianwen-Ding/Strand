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
    AudioSource objectAudio;
    [SerializeField]
    float yOffSet;
    [SerializeField]
    GameObject regScrapParticle;
    [SerializeField]
    GameObject gold2ScrapParticle;
        [SerializeField]
    GameObject gold4ScrapParticle;
    public void Start()
    {
        playerScript = FindObjectOfType<PlayerMainScript>();
        resource = FindObjectOfType<resourceSystem>();
        sleepButton = GameObject.FindGameObjectWithTag("SleepButton");
        objectAudio = gameObject.GetComponent<AudioSource>();
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

    public void addScrap(int addScrap)
    {
        resource.addScrap(addScrap);
        objectAudio.Play();
    }

    public void checkScrap(GameObject potentialScrap)
    {
        if (potentialScrap.tag == "Scrap" && potentialScrap.layer != 8)
        {
            Instantiate(regScrapParticle, potentialScrap.transform.position, Quaternion.identity.normalized);
            Destroy(potentialScrap);
            addScrap(1);
        }
        if (potentialScrap.tag == "GoldScrap" && potentialScrap.layer != 8)
        {
            Instantiate(gold2ScrapParticle, potentialScrap.transform.position, Quaternion.identity.normalized);
            Destroy(potentialScrap);
            addScrap(2);
        }
        if (potentialScrap.tag == "GoldGearScrap" && potentialScrap.layer != 8)
        {
            Instantiate(gold4ScrapParticle, potentialScrap.transform.position, Quaternion.identity.normalized);
            Destroy(potentialScrap);
            addScrap(4);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkScrap(collision.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        checkScrap(collision.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        checkScrap(collision.gameObject);
    }
}
