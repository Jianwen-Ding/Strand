using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class resourceSystem : MonoBehaviour
{
    //once it reacher scrap victory threshold the gane is won
    [SerializeField]
    int scrapCollected = 0;
    [SerializeField]
    int scrapVictoryThreshold;
    //once it reaches zero the player
    [SerializeField]
    float hungerMeter = 0;
    [SerializeField]
    float maxHunger;
    [SerializeField]
    float hungerHurtRate;
    [SerializeField]
    float hungerHurtTimeLeft = 0;
    [SerializeField]
    float hungerOverfillUntilRegen;
    [SerializeField]
    float hungerOverfillUntilRegenLeft = 0;
    [SerializeField]
    PlayerMainScript playerScript;
    // once a stalker infected tile has been entered increase hunger rate
    [SerializeField]
    bool stalkHunger;
    [SerializeField]
    float stalkHungerAccelerateRatio;
    [SerializeField]
    GameObject getWinGameObject;
    Image getWinImage;
    [SerializeField]
    string winScene;
    [SerializeField]
    float transitionSpeed;
    bool isWinning = false;
    //public functions
    //public get/set
    public void fillHunger(float amount)
    {
        hungerMeter += amount;
        if(hungerMeter > maxHunger)
        {
            hungerOverfillUntilRegenLeft += hungerMeter - maxHunger;
            hungerMeter = maxHunger;
            playerScript.healPlayer((int)(hungerOverfillUntilRegenLeft / hungerOverfillUntilRegen));
            hungerOverfillUntilRegenLeft = hungerOverfillUntilRegenLeft % hungerOverfillUntilRegen;
        }
    }
    public void addScrap(int amount)
    {
        scrapCollected += amount;
        PlayerPrefs.SetInt("Scrap", scrapCollected);
        if(scrapVictoryThreshold < scrapCollected)
        {
            triggerWinCondition();
        }
    }
    public float getHungerMeter()
    {
        return hungerMeter;
    }
    public float getMaxHungerMeter()
    {
        return maxHunger;
    }
    public float getHungerOverfillLeft()
    {
        return hungerOverfillUntilRegenLeft;
    }
    public float getHungerOverfillUntilHeal()
    {
        return hungerOverfillUntilRegen;
    }
    public int getScrapCollected()
    {
        return scrapCollected;
    }
    public bool getStalkHunger()
    {
        return stalkHunger;
    }
    public void setStalkHunger(bool setBool)
    {
        stalkHunger = setBool;
    }
    public void triggerWinCondition()
    {
        Time.timeScale = 0;
        getWinGameObject.SetActive(true);
        isWinning = true;
        getWinImage = getWinGameObject.GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        hungerMeter = maxHunger;
        scrapCollected = PlayerPrefs.GetInt("Scrap", 0);
        playerScript = FindObjectOfType<PlayerMainScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hungerMeter < 0)
        {
            hungerHurtTimeLeft -= Time.deltaTime;
            if(hungerHurtTimeLeft <= 0)
            {
                hungerHurtTimeLeft = hungerHurtRate;
                playerScript.damagePlayer(1);
            }
        }
        else
        {
            if (stalkHunger)
            {
                hungerMeter -= Time.deltaTime * stalkHungerAccelerateRatio;
            }
            else
            {
                hungerMeter -= Time.deltaTime;
            }
        }
        if (isWinning)
        {
            if (getWinImage.color.a + Time.unscaledDeltaTime * transitionSpeed >= 1)
            {
                getWinImage.color = new Color(getWinImage.color.r, getWinImage.color.g, getWinImage.color.b, 1);
                SceneManager.LoadScene(winScene);
            }
            else
            {
                getWinImage.color = new Color(getWinImage.color.r, getWinImage.color.g, getWinImage.color.b, getWinImage.color.a + Time.unscaledDeltaTime * transitionSpeed);
            }
        }
    }
}
