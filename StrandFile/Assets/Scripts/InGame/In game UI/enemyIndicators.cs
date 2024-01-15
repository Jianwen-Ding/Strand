using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyIndicators : MonoBehaviour
{
    [SerializeField]
    int health;
    [SerializeField]
    int grab;

    // Prefabs
    [SerializeField]
    GameObject healthBarPrefab;
    [SerializeField]
    GameObject grabBarPrefab;

    // Set vars
    [SerializeField]
    Vector2 healthBarCenter;
    [SerializeField]
    float xDistPerHealthSeg = (float)0.0989;
    [SerializeField]
    Vector2 grabBarAdjust = new Vector2(0, (float)-0.0989347);
    [SerializeField]
    float xDistPerGrabSeg = (float)0.0989;


    // Other vars
    [SerializeField]
    bool hasLoaded = false;
    [SerializeField]
    bool isHealthInvin = false;
    [SerializeField]
    bool isGrabInvin = false;
    GameObject[] healthBarSegments;
    SpriteRenderer[] healthBarSegSprites;
    Animator[] healthBarSegAnimators;
    GameObject[] grabBarSegments;
    SpriteRenderer[] grabBarSegSprites;
    Animator[] grabBarSegAnimators;

    // Hides the bars
    public void hideBars()
    {
        foreach (SpriteRenderer bar in healthBarSegSprites)
        {
            bar.enabled = false;
        }
        foreach (SpriteRenderer bar in grabBarSegSprites)
        {
            bar.enabled = false;
        }
    }

    // Shows the bars
    public void showBars()
    {
        foreach (SpriteRenderer bar in healthBarSegSprites)
        {
            bar.enabled = true;
        }
        foreach (SpriteRenderer bar in grabBarSegSprites)
        {
            bar.enabled = true;
        }
    }

    // Instantiates the bars
    public void load(int healthAmount, int grabAmount)
    {
        health = healthAmount;
        grab = grabAmount;
        if (!hasLoaded)
        {
            hasLoaded = true;
            // loads health
            if (!isHealthInvin)
            {
                healthBarSegments = new GameObject[healthAmount];
                healthBarSegAnimators = new Animator[healthAmount];
                healthBarSegSprites = new SpriteRenderer[healthAmount];
                bool alternate = false;
                for (int i = 0; i < healthAmount; i++)
                {
                    Vector2 adjustedCenter = healthBarCenter + new Vector2(transform.position.x, transform.position.y);
                    GameObject createdObject = Instantiate(healthBarPrefab, new Vector3((adjustedCenter.x - ((healthAmount - 1) * xDistPerHealthSeg) / 2) + (i * xDistPerHealthSeg), adjustedCenter.y), Quaternion.identity.normalized);
                    healthBarSegAnimators[i] = createdObject.GetComponent<Animator>();
                    healthBarSegSprites[i] = createdObject.GetComponent<SpriteRenderer>();
                    healthBarSegments[i] = createdObject;
                    // Alternates symbols
                    healthBarSegAnimators[i].SetBool("isAlt", alternate);
                    alternate = !alternate;
                    healthBarSegments[i].transform.SetParent(transform);
                }
                if (healthAmount > 0)
                {
                    healthBarSegAnimators[0].SetBool("isLeft", true);
                }
                if (healthAmount > 1)
                {
                    healthBarSegAnimators[healthAmount - 1].SetBool("isRight", true);
                }
            }
            else
            {
                healthBarSegments = new GameObject[3];
                healthBarSegAnimators = new Animator[3];
                healthBarSegSprites = new SpriteRenderer[3];
                for (int i = 0; i < 3; i++)
                {
                    Vector2 adjustedCenter = healthBarCenter + new Vector2(transform.position.x, transform.position.y);
                    GameObject createdObject = Instantiate(healthBarPrefab, new Vector3((adjustedCenter.x - (2 * xDistPerHealthSeg) / 2) + (i * xDistPerHealthSeg), adjustedCenter.y), Quaternion.identity.normalized);
                    healthBarSegAnimators[i] = createdObject.GetComponent<Animator>();
                    healthBarSegSprites[i] = createdObject.GetComponent<SpriteRenderer>();
                    healthBarSegments[i] = createdObject;
                    healthBarSegments[i].transform.SetParent(transform);
                }
                healthBarSegAnimators[0].SetBool("isLeft", true);
                healthBarSegAnimators[1].SetBool("isInvin", true);
                healthBarSegAnimators[2].SetBool("isRight", true);
            }
            // loads grab armor
            if (!isGrabInvin)
            {
                grabBarSegments = new GameObject[grabAmount];
                grabBarSegAnimators = new Animator[grabAmount];
                grabBarSegSprites = new SpriteRenderer[grabAmount];
                bool alternate = false;
                for (int i = 0; i < grabAmount; i++)
                {
                    Vector2 adjustedCenter = healthBarCenter + grabBarAdjust + new Vector2(transform.position.x, transform.position.y);
                    GameObject createdObject = Instantiate(grabBarPrefab, new Vector3((adjustedCenter.x - ((grabAmount - 1) * xDistPerGrabSeg) / 2) + (i * xDistPerGrabSeg), adjustedCenter.y), Quaternion.identity.normalized);
                    grabBarSegAnimators[i] = createdObject.GetComponent<Animator>();
                    grabBarSegments[i] = createdObject;
                    grabBarSegSprites[i] = createdObject.GetComponent<SpriteRenderer>();
                    // Alternates symbols
                    grabBarSegAnimators[i].SetBool("isAlt", alternate);
                    alternate = !alternate;
                    grabBarSegments[i].transform.SetParent(transform);
                }
                if (grabAmount > 0)
                {
                    grabBarSegAnimators[0].SetBool("isLeft", true);
                }
                if (grabAmount > 1)
                {
                    grabBarSegAnimators[grabAmount - 1].SetBool("isRight", true);
                }
            }
            else
            {
                grabBarSegments = new GameObject[3];
                grabBarSegAnimators = new Animator[3];
                grabBarSegSprites = new SpriteRenderer[3];
                for (int i = 0; i < 3; i++)
                {
                    Vector2 adjustedCenter = healthBarCenter + grabBarAdjust + new Vector2(transform.position.x, transform.position.y);
                    GameObject createdObject = Instantiate(grabBarPrefab, new Vector3((adjustedCenter.x - (2 * xDistPerGrabSeg) / 2) + (i * xDistPerGrabSeg), adjustedCenter.y), Quaternion.identity.normalized);
                    grabBarSegAnimators[i] = createdObject.GetComponent<Animator>();
                    grabBarSegments[i] = createdObject;
                    grabBarSegSprites[i] = createdObject.GetComponent<SpriteRenderer>();
                    grabBarSegments[i].transform.SetParent(transform);

                }
                grabBarSegAnimators[0].SetBool("isLeft", true);
                grabBarSegAnimators[1].SetBool("isInvin", true);
                grabBarSegAnimators[2].SetBool("isRight", true);
            }
        }
    }

    // Adjusts health bar to new health
    public void updateHealth(int newHealth)
    {
        if (hasLoaded && !isHealthInvin)
        {
            for(int i = 0; i < healthBarSegAnimators.Length; i++)
            {
                if (i >= newHealth)
                {
                    healthBarSegAnimators[i].SetBool("isBroken", true);
                }
            }
        }
    }

    // Adjusts grab armor bar to new grab armor
    public void updateGrabArmor(int newGrab)
    {
        if(hasLoaded && !isGrabInvin)
        {
            for (int i = 0; i < grabBarSegAnimators.Length; i++)
            {
                grabBarSegAnimators[i].SetBool("isBroken", i >= newGrab);
            }
        }
    }
}
