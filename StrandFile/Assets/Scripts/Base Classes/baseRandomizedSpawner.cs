using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseRandomizedSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] prefabList;
    [SerializeField]
    float[] chanceList;
    Vector3 positionSpawn;

    //Changes prefabList on start
    public virtual GameObject[] prefabGatherers()
    {
        return prefabList;
    }

    //Changes chanceList on start
    public virtual float[] chanceGatherers()
    {
        return chanceList;
    }

    //Sets positionspawn on start
    public virtual Vector3 positionGatherers()
    {
        return gameObject.transform.position;
    }

    //Returns object spawned
    public virtual GameObject gameObjectSpawn(GameObject prefab ,Vector3 position)
    {
        return Instantiate(prefab, position, Quaternion.identity.normalized);
    }

    // Start is called before the first frame update
    void Start()
    {
        prefabList = prefabGatherers();
        chanceList = chanceGatherers();
        positionSpawn = positionGatherers();
        //Combines value of chances
        float chanceTotal = 0;
        for(int i = 0; i < chanceList.Length; i++)
        {
            chanceTotal += chanceList[i];
            chanceList[i] = chanceTotal;
        }
        float chanceLand = Random.Range(0, chanceTotal);
        for(int i = 0; i < chanceList.Length; i++)
        {
            if(chanceLand <= chanceList[i])
            {
                gameObjectSpawn(prefabList[i], positionSpawn);
                Destroy(gameObject);
                break;
            }
        }
    }
}
