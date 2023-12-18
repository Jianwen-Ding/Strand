using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleGenerator : MonoBehaviour
{
    [SerializeField]
    bool centeredAroundObject;
    [SerializeField]
    Vector2 topRightCorner;
    [SerializeField]
    Vector2 botLeftCorner;
    [SerializeField]
    int spawnedPerWave;
    [SerializeField]
    float timeUntilWave;
    float waveTimeLeft = 0; 
    [SerializeField]
    GameObject particleEffect;

    // Update is called once per frame
    void Update()
    {
        waveTimeLeft -= Time.deltaTime;
        if(waveTimeLeft <= 0)
        {
            waveTimeLeft = timeUntilWave;
            for(int i = 0; i < spawnedPerWave; i++)
            {
                float xRandom;
                float yRandom;
                if (!centeredAroundObject)
                {
                    xRandom = Random.Range(botLeftCorner.x, topRightCorner.x);
                    yRandom = Random.Range(botLeftCorner.y, topRightCorner.y);
                }
                else
                {
                    xRandom = Random.Range(botLeftCorner.x + transform.position.x, topRightCorner.x + transform.position.x);
                    yRandom = Random.Range(botLeftCorner.y + transform.position.y, topRightCorner.y + transform.position.y);
                }
                Vector2 randomVect = new Vector2(xRandom, yRandom);
                Instantiate(particleEffect, randomVect, Quaternion.identity.normalized);
            }
        }
    }
}
