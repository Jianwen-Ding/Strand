using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class scrapUnlock : MonoBehaviour
{
    [SerializeField]
    bool isGolden;
    [SerializeField]
    string scrapName;
    [SerializeField]
    AudioClip successClip;
    [SerializeField]
    AudioClip failClip;
    AudioSource audioPlayer;
    [SerializeField]
    int scrapCost;
    private void Start()
    {
        audioPlayer = gameObject.GetComponent<AudioSource>();
        if((isGolden && scrapStorer.getGoldenScrapEnabled(scrapName)) || (!isGolden && scrapStorer.getNormalScrapEnabled(scrapName)))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public void attemptUnlock()
    {
        if(scrapStorer.getScrap() >= scrapCost)
        {
            audioPlayer.clip = successClip;
            audioPlayer.Play();
            scrapStorer.setScrap(scrapStorer.getScrap() - scrapCost);
            if (isGolden)
            {
                scrapStorer.setGoldenScrapEnabled(scrapName, true);
            }
            else {
                scrapStorer.setNormalScrapEnabled(scrapName, true);
            }

            transform.GetChild(0).gameObject.SetActive(true);
            gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            audioPlayer.clip = failClip;
            audioPlayer.Play();
        }
    }
}
